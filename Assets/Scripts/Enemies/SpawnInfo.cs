using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnInfo
{
    public float time;
    public int enemyIndex; // or enemy id
    public int spawnPositionIndex;
    public int rank;
}
