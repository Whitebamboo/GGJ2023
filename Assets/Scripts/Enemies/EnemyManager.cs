using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<WaveInfo> waveInfos;
    private List<Enemy> enemies;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
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
            while (spawnInfos[i].time <= timer)
            {
                // spawn for spawnInfos[i]
                var go = Instantiate(enemyPrefabs[spawnInfos[i].enemyIndex],
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.position,
                    spawnPositions[spawnInfos[i].spawnPositionIndex].transform.rotation, transform);
                enemies.Add(go.GetComponent<Enemy>());
                i += 1;
            }
            
            yield return null;
            timer += Time.deltaTime;
        }
    }

    private int SortByTime(SpawnInfo a, SpawnInfo b)
    {
        return a.time - b.time;
    }
}
