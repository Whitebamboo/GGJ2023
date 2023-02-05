using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tree : CSingletonMono<Tree>
{
    public float Health;
    public float NetworkProcessInterval;

    private float processTimer;

    public SkillConfig defaultSkill;

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

        NetworkModule = new TreeNetworkModule();
        TreeNode bulletNode = new TreeNode(defaultSkill);
        NetworkModule.AddNodeToLayer(1, bulletNode);

        
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

        TreeNodeChain chain = NetworkModule.GetTreeNodeChain();
        currentChain = chain;

        string text = "";

        foreach(TreeNode node in chain.treeNodeList)
        {
            if(node.skillConfig == null)
            {
                continue;
            }

            text += node.skillConfig.skilltype + " | ";
        }
        print(text);

        AttackModule.ProcessTreeNodes(chain.treeNodeList);
    }


}
