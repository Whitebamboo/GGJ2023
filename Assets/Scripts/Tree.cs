using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tree : CSingletonMono<Tree>
{
    public float Health;
    private float initHealth;
    public float NetworkProcessInterval;

    private float processTimer;

    public SkillConfig defaultSkill;
    public SkillConfig defaultSkill2;

    public bool ProcessingStart { get; set; }

    public TreeNetworkModule NetworkModule { get; private set; }

    public TreeAttackModule AttackModule;

    private TreeNodeChain currentChain;

    //true is yes, false is no
    public bool yesOrNo;

    public bool yesOrNoClicked;

    private void Start()
    {
        GetComponentInChildren<HPBar>().InitialHP(Health);
        processTimer = 1f;
        Init();
    }

    private void Init()
    {
        ProcessingStart = true;
        initHealth = Health;
        NetworkModule = new TreeNetworkModule();
        TreeNode bulletNode = new TreeNode(defaultSkill);
        TreeNode sheidNode = new TreeNode(defaultSkill2);
        NetworkModule.AddNodeToLayer(1, bulletNode);
        NetworkModule.AddNodeToLayer(1, sheidNode);
    }

    /// <summary>
    /// on hit take damage
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageType"></param>
    public void TakeDamage(float damage,DmgType damageType)
    {
        Health -= damage;
        GetComponentInChildren<HPBar>().UpdateHP(Health);
    }

    /// <summary>
    /// vampire or other's healing
    /// </summary>
    /// <param name="heal"></param>
    public void GetHeal(float heal)
    {
        heal = Mathf.Abs(heal);
        Health += heal;
        Health = Mathf.Clamp(Health, 0, initHealth);
        GetComponentInChildren<HPBar>().UpdateHP(Health);

    }

    private void Update()
    {
        if (ProcessingStart)
        {
            processTimer -= Time.deltaTime; 
            
            if(processTimer <= 0)
            {
                ProcessNetwork();
                processTimer = NetworkProcessInterval;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnYesClicked();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            OnNoClicked();
        }

        CheckDead();
    }

    public void CheckDead()
    {
        if (Health <= 0)
        {
            print("loss game");
            GameManager.instance.SetState(GameState.Loss);
        }
    }


    public void OnYesClicked()
    {
        yesOrNo = true;
        yesOrNoClicked = true;
    }

    public void OnNoClicked()
    {
        yesOrNo = false;
        yesOrNoClicked = true;
    }

    void ProcessNetwork()
    {
        if(yesOrNoClicked && currentChain != null)
        {
            currentChain.UpdateWeight(yesOrNo);
        }
        yesOrNoClicked = false;
        GameManager.instance.ResetYesNo();
        GameManager.instance.networkUI.ProcessLineRender(NetworkModule.Layers);

        TreeNodeChain chain = NetworkModule.GetTreeNodeChain();
        currentChain = chain;
        GameManager.instance.networkUI.ProcessTreeNodeChain(currentChain);

        string text = "";

        foreach(TreeNode node in chain.treeNodeList)
        {
            if(node.skillConfig == null)
            {
                continue;
            }

            //text += node.skillConfig.skilltype + " | ";
        }
        //print(text);

        //TODO unlock
        //AttackModule.ProcessTreeNodes(chain.treeNodeList);
    }


}
