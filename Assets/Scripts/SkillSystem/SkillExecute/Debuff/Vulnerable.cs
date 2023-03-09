using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulnerable : DebuffExecute
{
    public Vulnerable()
    {
        num = 1;
        layer = 0;
        value = 10;
        time = 4;
    }

    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponentInParent<Enemy>();
        VulnerableDebuff vulnerableDebuff = e.debuffContainer.GetAspect<VulnerableDebuff>();
        if (vulnerableDebuff == null)
        {
            //create a new instance
            vulnerableDebuff = new VulnerableDebuff(num * layer * value * 0.01f, time);
            e.debuffContainer.AddAspect<VulnerableDebuff>(vulnerableDebuff);
            vulnerableDebuff.OnApply(e);
        }
        else
        {
            //rewrite old instance
            vulnerableDebuff.value = num * layer * value * 0.01f;
            vulnerableDebuff.times = time;
            vulnerableDebuff.OnApply(e);
        }
    }
}
