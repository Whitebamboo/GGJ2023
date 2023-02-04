using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            
            // update non-attacking enemy movement
            enemies.Where(enemy => !enemy.isAttacking).ToList().ForEach(enemy =>
            {
                var dir = transform.position - enemy.gameObject.transform.position;
                dir.z = 0;
                dir = dir.normalized;
                enemy.gameObject.GetComponent<Rigidbody>().velocity = dir * enemy.speed;
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
                GameManager.instance.tree.Health -= enemy.attack;
            });
            
            // handle debuffs
            enemies.ForEach(enemy =>
            {
                int fireDebuffIndex = enemy.debuffs.FindIndex(debuff => debuff.elementType == ElementsType.Fire);
                if (fireDebuffIndex == -1) return;
                var debuff = enemy.debuffs[fireDebuffIndex];
                if (debuff.times == 0) return;
                debuff.coolDown -= Time.deltaTime;
                if (debuff.coolDown <= 0)
                {
                    enemy.health -= debuff.value;
                    debuff.times -= 1;
                    debuff.coolDown += debuff.interval;
                }
            });
            
            
            yield return null;
            timer += Time.deltaTime;
        }
    }

    private int SortByTime(SpawnInfo a, SpawnInfo b)
    {
        return a.time - b.time;
    }
}