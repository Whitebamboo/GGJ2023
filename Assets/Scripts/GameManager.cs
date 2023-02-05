using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    public NetworkUI networkUI;
    public float dropChance;

    public void SetState(GameState state)
    {
        this.state = state;

        if(state == GameState.InGame)
        {
            YesOrNoUI.SetActive(true);
            enemyManager.StartWave();
        }
    }

    public void ProcessDropItemList(List<SkillConfig> skills)
    {
        if(skills == null || skills.Count == 0)
        {
            Debug.LogError("No skill drops in enemey");
        }

        float randomNumber = Random.value;
        if(dropChance > randomNumber)
        {
            SkillConfig skillConfig = skills[Random.Range(0, skills.Count)];
            networkUI.AddNewItem(skillConfig);
        }
    }
}

public enum GameState
{
    StartMenu,
    InGame,
}