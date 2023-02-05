using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingletonMono<GameManager>
{
    public float initialWeight;
    public float maxWeight;
    public float minWeight; 
    public Tree tree;
    public EnemyManager enemyManager;
    public DmgTextManager dmgTextManager;
    public GameState state = GameState.StartMenu;
    public GameObject YesOrNoUI;

    public void SetState(GameState state)
    {
        this.state = state;

        if(state == GameState.InGame)
        {
            YesOrNoUI.SetActive(true);
            enemyManager.StartWave();
        }
    }
}

public enum GameState
{
    StartMenu,
    InGame,
}