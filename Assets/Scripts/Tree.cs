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

    public TreeAttackModule AttackModule { get; private set; }  

    private void Start()
    {
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
        
    }


}
