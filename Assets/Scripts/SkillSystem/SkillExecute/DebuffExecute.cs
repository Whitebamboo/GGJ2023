using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffExecute : SkillExecute
{
    public int num;//how many this node
    public int layer;//a multiple layer
    public float base_value = 15;//15%
    public float base_time = 4;
    
    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponent<Enemy>();
        Debug.Log("make debuff" + num * layer * base_value * 0.01);
    }
}
