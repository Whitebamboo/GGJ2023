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

    public void UpdateWeight(float value)
    {
        Weight += value;

        Weight = Mathf.Clamp(Weight, GameManager.instance.minWeight, GameManager.instance.maxWeight);
    }
}
