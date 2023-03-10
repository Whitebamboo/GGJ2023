using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthBuff : Buff
{

    public GrowthBuff(float _value, int _times)
    {
        value = _value;
        times = _times;
    }

    /// <summary>
    /// each reapeat 3 times the original wood buff
    /// </summary>
    /// <param name="tree"></param>
    public override void OnCompileRepeat(TreeAttackModule tree)
    {
        base.OnCompileRepeat(tree);
        //hard code later porbably can use pointer to do this
        Wood w = tree.new_ability.GetAspect<Wood>();
        Vulnerable v = tree.new_ability.GetAspect<Vulnerable>();
        Vampire vam = tree.new_ability.GetAspect<Vampire>();
        //this buff happens after compile, so its will affect all var connect to wood
        int growth_mul = (int)Mathf.Max(1, value);
        if (w != null)
        {
            w.growthLayer = (int)growth_mul;
            if(v != null)
            {
                v.growthLayer = (int)growth_mul;
            }
            if(vam != null)
            {
                vam.growthLayer = (int)growth_mul;
            }
        }
        print("current growth layer : " + growth_mul);
    }

    public override void OnRemove(TreeAttackModule tree)
    {
        base.OnRemove(tree);
        value = 0;
    }
}
