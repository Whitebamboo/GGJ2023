using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : DebuffExecute
{
    public Burn()
    {
        num = 1;
        layer = 0;
        value = 3;
        time = 4;
    }

    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponentInParent<Enemy>();
        BurnDebuff burnDebuff = e.debuffContainer.GetAspect<BurnDebuff>();
      
        if (burnDebuff == null)
        {
            //create a new instance
            burnDebuff = new BurnDebuff(value * num * layer, time);
            e.debuffContainer.AddAspect<BurnDebuff>(burnDebuff);
            burnDebuff.OnApply(e);
        }
        else
        {
            //rewrite old instance
            burnDebuff.value = value * num * layer;
            burnDebuff.times = time;
            burnDebuff.OnApply(e);
        }
        print("burn layer num : " + num * layer);
    }
}
