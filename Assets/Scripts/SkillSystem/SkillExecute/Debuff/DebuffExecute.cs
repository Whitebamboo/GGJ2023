using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffExecute : SkillExecute
{
    public int num;//how many this node
    public int layer;//a multiple layer
    public float value = 15;//15%
    public int time = 4;
    
    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        //Debug.Log("debuff number :" + num);
        //Debug.Log("debuff layer :" + layer);
        //Debug.Log("make debuff base value: " + value * 0.01);
        //Debug.Log("debuff duration : " + time);
    }
}
