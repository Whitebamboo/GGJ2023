using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : SkillExecute
{
    public int growthLayer;//for growth buff
    public int layer;//a multiple layer relative to a num
    public float value = 5;//5%

    public Vampire()
    {
        num = 1;
        layer = 0;
        value = 5;
        growthLayer = 1;
    }

    public override void AfterOnHitExec(GameObject target)
    {
        base.AfterOnHitExec(target);
        
        Enemy e = target.GetComponent<Enemy>();
        if (e)
        {
            int heal = (int)(e.finalDamage * num * layer * value * 0.01f * growthLayer);
            Tree tree = GameObject.FindObjectOfType<Tree>();
            if (tree)
            {
                tree.GetHeal((float)heal);
            }
            //Debug.Log("from enemy damage : "+ e.finalDamage + " life steal : " + heal);
            //print("vampire layer num : " + layer);
        }

    }
}
