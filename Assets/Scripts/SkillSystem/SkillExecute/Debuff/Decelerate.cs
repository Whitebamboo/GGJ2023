using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decelerate : DebuffExecute
{
    public Decelerate()
    {
        num = 1;
        layer = 0;
        value = 15;
        time = 4;
    }

    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponentInParent<Enemy>();
        DecelerateDebuff decelerateDebuff = e.debuffContainer.GetAspect<DecelerateDebuff>();
        if(decelerateDebuff == null)
        {
            //create a new instance
            decelerateDebuff = new DecelerateDebuff(num * layer * value * 0.01f, time);
            e.debuffContainer.AddAspect<DecelerateDebuff>(decelerateDebuff);
            decelerateDebuff.OnApply(e);
        }
        else
        {
            //rewrite old instance
            decelerateDebuff.value = num * layer * value * 0.01f;
            decelerateDebuff.times = time;
            decelerateDebuff.OnApply(e);
        }
        print("decelerate num * layer :" + num * layer);
    }
}
