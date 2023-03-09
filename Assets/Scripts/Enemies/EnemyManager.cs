using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<WaveInfo> waveInfos;
    public List<Enemy> enemies;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave()
    {
        // clean enemy list, in case there's any left
        enemies.ForEach(enemy => Destroy(enemy.gameObject));
        enemies = new List<Enemy>();
        StartCoroutine(WaveCoroutine());
    }

    public IEnumerator WaveCoroutine()
    {
        int waveIndex = 0;
        var waveInfo = waveInfos[waveIndex];
        var spawnInfos = waveInfo.spawnInfos;
        int i = 0;
        while (waveIndex < waveInfos.Count || enemies.Count > 0)
        {
            if (waveIndex < waveInfos.Count && timer >= waveInfo.time)
            {
                // read next wave info
                waveIndex += 1;
                if (waveIndex != waveInfos.Count)
                {
                    Debug.Log($"Round {waveIndex + 1} ");
                    timer = 0;
                    waveInfo = waveInfos[waveIndex];
                    spawnInfos = waveInfo.spawnInfos;
                    spawnInfos.Sort(SortByTime);
                    i = 0;
                }
            }

            while (i < spawnInfos.Count && spawnInfos[i].time <= timer)
            {
                // spawn for spawnInfos[i]
                var go = Instantiate(enemyPrefabs[spawnInfos[i].enemyIndex],
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.position,
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.rotation, transform);
                var enemy = go.GetComponent<Enemy>();
                enemy.AddRank(spawnInfos[i].rank);
                enemies.Add(enemy);
                i += 1;
            }

            // handle debuffs

            foreach(Enemy e in enemies)
            {
                foreach(Debuff d in e.debuffContainer.Aspects())
                {
                    if(d.times > 0)
                    {
                        d.coolDown -= Time.deltaTime;
                        if (d.coolDown <= 0)
                        {
                            d.OnRepeat(e);//repeat behavior
                            d.times -= 1;//exist times
                            if (d.times == 0)
                            {
                                d.OnRemove(e);//remove behavior
                            }

                            d.coolDown += d.configInterval;
                        }
                    }
           
                }
            }


          

            enemies.Where(enemy => enemy.health <= 0).ToList().ForEach(enemy =>
            {
                enemy.DropSkills();
                Destroy(enemy.gameObject);
            });
            enemies.RemoveAll(enemy => enemy.health <= 0);


            // update non-attacking enemy movement
            enemies.Where(enemy => !enemy.isAttacking).ToList().ForEach(enemy =>
            {
                var dir = transform.position - enemy.gameObject.transform.position;
                dir.z = 0;
                dir = dir.normalized;
                var localScale = enemy.model.transform.localScale;
                enemy.model.transform.localScale = new Vector3(localScale.x, localScale.y,
                    Mathf.Abs(localScale.z) * (dir.x < 0 ? -1 : 1));
                enemy.gameObject.GetComponent<Rigidbody>().velocity =
                    dir * enemy.speed * Mathf.Clamp01(1 - enemy.speedDecreaseRate);
            });

            // update cool down
            enemies.Where(enemy => enemy.attackCoolDown > 0).ToList().ForEach(enemy =>
            {
                enemy.attackCoolDown -= Time.deltaTime;
                if (enemy.attackCoolDown < 0)
                {
                    enemy.attackCoolDown = 0;
                }
            });

            // update attack
            enemies.Where(enemy => enemy.isAttacking && enemy.attackCoolDown == 0).ToList().ForEach(enemy =>
            {
                enemy.attackCoolDown = enemy.attack_interval;
                var dir = transform.position - enemy.gameObject.transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x);
                enemy.slashEffect.transform.localEulerAngles = new Vector3(0, 0,
                    Mathf.Rad2Deg * angle * (enemy.gameObject.transform.localScale.x > 0 ? 1 : -1));
                enemy.slashEffect.GetComponent<ParticleSystem>().Play();
                if (enemy.attack_target && enemy.attack_target.activeSelf)
                {
                    //attack target
                    if (enemy.attack_target.tag == "Tree")
                    {
                        GameManager.instance.tree.TakeDamage(enemy.attack, DmgType.PlayerNormal);
                    }
                    else if (enemy.attack_target.tag == "Shield")
                    {
                        enemy.attack_target.GetComponent<shield>().TakeDamage(enemy.attack, enemy);
                    }
                    else if (enemy.attack_target.tag == "Drone")
                    {
                        enemy.attack_target.GetComponent<DroneBase>().TakeDamage(enemy.attack, DmgType.EnemyNormal);
                    }

                    GameManager.instance.dmgTextManager.AddDmgText(enemy.attack, DmgType.PlayerNormal,
                        enemy.attack_target.transform.position);
                }
                else
                {
                    enemy.isAttacking = false;
                }

            });

            yield return null;
            timer += Time.deltaTime;

        }

        GameManager.instance.ShowEndPanel(GameState.Win);
    }

    private int SortByTime(SpawnInfo a, SpawnInfo b)
    {
        return (int) (a.time - b.time);
    }
}
