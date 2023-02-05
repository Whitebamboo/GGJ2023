using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingletonMono<GameManager>
{
    public float initialWeight;
    public Tree tree;
    public EnemyManager enemyManager;
    public GameState state = GameState.StartMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

}

public enum GameState
{
    StartMenu,
    InGame,
}