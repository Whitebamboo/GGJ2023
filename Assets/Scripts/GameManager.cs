using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingletonMono<GameManager>
{
    public float initialWeight;
    public Tree tree;
    public EnemyManager enemyManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
