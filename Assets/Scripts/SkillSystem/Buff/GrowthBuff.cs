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
       
        //this buff happens before compile, add wood node to tree skill config
        if(value > 10)
        {
            value = 10;
        }
        int growth_mul = (int)Mathf.Max(1, value);
        int count = 0;
        SkillConfig wood = null;
        foreach(SkillConfig s in tree.turn_skill)
        {
            if(s.id == 1011)
            {
                if (wood == null)
                {
                    wood = s;
                }
                count++;
            }
        }
        if (count > 0)
        {
            for(int i = 0; i < count * growth_mul; i++)
            {
                tree.turn_skill.Add(wood);
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
