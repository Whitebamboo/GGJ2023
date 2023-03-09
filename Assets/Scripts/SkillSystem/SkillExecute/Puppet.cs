using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : SkillExecute
{
    public int puppetThreshold = 100;
    public int inherit_Mul = 1;// when create a puppet how much value will it inherit from the enemy
    public override void AfterOnHitExec(GameObject target)
    {
        base.AfterOnHitExec(target);
        
        if(target!= null)
        {
            Enemy e = target.GetComponentInParent<Enemy>();
            if(e.speedDecreaseRate >= (puppetThreshold * 0.01f))
            {
                GameObject.FindObjectOfType<TreeAttackModule>().CreatePuppet(e, inherit_Mul);//create puppet
                //Debug.Log("make this puppet");
            }
        }
    }
}
