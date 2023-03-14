using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public YesNoUI YesNoUI;
    public NetworkUI networkUI;
    public DescriptionUI descriptionUI;
    public float dropChance;

    public GameObject winPanel;
    public GameObject lossPanel;
    public void SetState(GameState state)
    {
        this.state = state;

        if(state == GameState.InGame)
        {
            YesOrNoUI.SetActive(true);
            enemyManager.StartWave();
        }
        if((state == GameState.Win) || (state == GameState.Loss))
        {
            ShowEndPanel(state);
        }
                
    }

    /// <summary>
    /// drop skill random base on enemy kill
    /// </summary>
    /// <param name="skills"></param>
    /// <param name="dropChance"></param>
    public void ProcessDropItemList(List<SkillConfig> skills, float dropChance)
    {
        //Debug.Log("Add Items!!!!");

        if(skills == null || skills.Count == 0)
        {
            Debug.LogError("No skill drops in enemey");
        }

        dropChance /= Mathf.Sqrt(enemyManager.enemies.Count);
        float randomNumber = Random.value;
        if(dropChance > randomNumber)
        {
            SkillConfig skillConfig = skills[Random.Range(0, skills.Count)];
            networkUI.AddNewItem(skillConfig);
        }
    }

    /// <summary>
    /// drop skill base on experience system
    /// </summary>
    /// <param name="skills"></param>
    public void ProcessDropItemList(List<SkillConfig> skills)
    {
        //Debug.Log("Add Items!!!!");

        if (skills == null || skills.Count == 0)
        {
            Debug.LogError("No skill drops in enemey");
        }
        else
        {
            SkillConfig skillConfig = skills[Random.Range(0, skills.Count)];
            networkUI.AddNewItem(skillConfig);
        }
      
        
    }

    public void ShowDescription(string des)
    {
        descriptionUI.SetText(des);
    }

    public void DisableDescription()
    {
        descriptionUI.DisableBox();
    }

    public void ShowEndPanel(GameState gameState)
    {
        if (gameState == GameState.Win)
        {
            UIUtils.instance.showUi(winPanel);
        }
        else if (gameState == GameState.Loss)
        {
            UIUtils.instance.showUi(lossPanel);
        }
    }

    public void ResetYesNo()
    {
        YesNoUI.ResetUI();
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    StartMenu,
    InGame,
    Win,
    Loss,
}