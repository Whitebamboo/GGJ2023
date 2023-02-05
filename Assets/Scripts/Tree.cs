using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;

public class Tree : CSingletonMono<Tree>
{
    public float Health;
    public float NetworkProcessInterval;

    private float processTimer;

    public SkillConfig defaultSkill;

    public bool ProcessingStart { get; set; }

    public TreeNetworkModule NetworkModule { get; private set; }

    private TreeAttackModule AttackModule;

    private void OnEnable()
    {
        AttackModule = GetComponent<TreeAttackModule>();
    }

    private void Start()
    {
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
    
    void ProcessNetwork()
    {
        TreeNodeChain chain = NetworkModule.GetTreeNodeChain();

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

        //AttackModule.ProcessTreeNodes(chain.treeNodeList);
    }


}
