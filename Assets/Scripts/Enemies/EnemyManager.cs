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
        StartCoroutine(WaveCoroutine(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int waveIndex)
    {
        StartCoroutine(WaveCoroutine(waveIndex));
    }

    public IEnumerator WaveCoroutine(int waveIndex)
    {
        var spawnInfos = waveInfos[waveIndex].spawnInfos;
        spawnInfos.Sort(SortByTime);
        timer = 0;
        int i = 0;
        
        // clean enemy list, in case there's any left
        enemies.ForEach(enemy => Destroy(enemy.gameObject));
        enemies = new List<Enemy>();
        
        while (timer < waveInfos[waveIndex].time)
        {
            while (i < spawnInfos.Count && spawnInfos[i].time <= timer)
            {
                // spawn for spawnInfos[i]
                var go = Instantiate(enemyPrefabs[spawnInfos[i].enemyIndex],
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.position,
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.rotation, transform);
                enemies.Add(go.GetComponent<Enemy>());
                i += 1;
            }
            
            // handle debuffs
            enemies.ForEach(enemy =>
            {
                enemy.debuffs.ForEach(debuff =>
                {
                    debuff.coolDown -= Time.deltaTime;
                    if (debuff.coolDown <= 0)
                    {
                        debuff.OnRepeat(enemy);
                        debuff.times -= 1;
                        if (debuff.times == 0)
                        {
                            debuff.OnRemove(enemy);
                        }
                        debuff.coolDown += debuff.configInterval;
                    }
                });
                enemy.debuffs.RemoveAll(debuff => debuff.times == 0);
            });
            
            enemies.Where(enemy => enemy.health <= 0).ToList().ForEach(enemy => Destroy(enemy.gameObject)); 
            enemies.RemoveAll(enemy => enemy.health <= 0);

            
            // update non-attacking enemy movement
            enemies.Where(enemy => !enemy.isAttacking).ToList().ForEach(enemy =>
            {
                var dir = transform.position - enemy.gameObject.transform.position;
                dir.z = 0;
                dir = dir.normalized;
                var localScale = enemy.gameObject.transform.localScale;
                enemy.gameObject.transform.localScale = new Vector3(Mathf.Abs(localScale.x) * (dir.x < 0 ? -1 : 1), localScale.y, localScale.z);
                enemy.gameObject.GetComponent<Rigidbody>().velocity = dir * enemy.speed * (1 - enemy.speedDecreaseRate / 100);
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
                enemy.slashEffect.transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle * (enemy.gameObject.transform.localScale.x > 0 ? 1 : -1));
                enemy.slashEffect.GetComponent<ParticleSystem>().Play();
                if (enemy.attack_target)
                {
                    //attack target
                    if(enemy.attack_target.tag == "Tree")
                    {
                        GameManager.instance.tree.Health -= enemy.attack;
                    }
                    else if(enemy.attack_target.tag == "Shield")
                    {
                        enemy.attack_target.GetComponent<shield>().TakeDamage(enemy.attack, enemy.element);
                    }
                    
                }
                else
                {
                    enemy.isAttacking = false;
                }
               
            });
            
            

            yield return null;
            timer += Time.deltaTime;
        }
    }

    private int SortByTime(SpawnInfo a, SpawnInfo b)
    {
        return (int) (a.time - b.time);
    }
}
