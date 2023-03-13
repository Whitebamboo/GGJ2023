using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralysis : DebuffExecute
{
    public Paralysis()
    {
        num = 1;
        layer = 0;
        value = 30;
        time = 4;
    }

    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponentInParent<Enemy>();
        ParalysisDebuff paralysisDebuff = e.debuffContainer.GetAspect<ParalysisDebuff>();
        if (paralysisDebuff == null)
        {
            //create a new instance
            paralysisDebuff = new ParalysisDebuff(num * layer * value * 0.01f, time);
            e.debuffContainer.AddAspect<ParalysisDebuff>(paralysisDebuff);
            paralysisDebuff.OnApply(e);
        }
        else
        {
            //rewrite old instance
            paralysisDebuff.value = num * layer * value * 0.01f;
            paralysisDebuff.times = time;
            paralysisDebuff.OnApply(e);
        }
        print("paralysis num * layer :" + num * layer);
    }
}
