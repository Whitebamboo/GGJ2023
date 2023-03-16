using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : SkillExecute
{
    public int times;

    public Growth()
    {
        num = 1;
        times = 2;
    }
    public override void OnCompileExec(GameObject target)
    {
        base.OnCompileExec(target);
        TreeAttackModule t = target.GetComponent<TreeAttackModule>();
        if (t)
        {
            GrowthBuff gb = t.buffContainer.GetAspect<GrowthBuff>();
            if(gb == null)
            {
                gb = new GrowthBuff(num, times);
                t.buffContainer.AddAspect<GrowthBuff>(gb);
            }
            else
            {
                gb.value += num;
                gb.times = times;
            }
        }

    }
}
