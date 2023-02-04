using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float Health;
    public float NetworkProcessInterval;

    private float processTimer;

    public bool ProcessingStart { get; set; }

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
        Debug.Log("Process");
    }

    
}
