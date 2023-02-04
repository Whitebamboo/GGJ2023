using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tree : MonoBehaviour
{
    public float Health;
    public float NetworkProcessInterval;

    private float processTimer;

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
