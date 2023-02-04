using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge 
{
    public float Weight { get; private set; }

    public Edge()
    {
        Weight = GameManager.instance.initialWeight;
    }
}
