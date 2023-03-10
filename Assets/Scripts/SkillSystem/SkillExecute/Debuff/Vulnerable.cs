using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulnerable : DebuffExecute
{
    public int growthLayer;
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
        float final_value = num * layer * value * 0.01f * growthLayer;
        if (vulnerableDebuff == null)
        {
            //create a new instance
            vulnerableDebuff = new VulnerableDebuff(final_value, time);
            e.debuffContainer.AddAspect<VulnerableDebuff>(vulnerableDebuff);
            vulnerableDebuff.OnApply(e);
        }
        else
        {
            //rewrite old instance
            vulnerableDebuff.value = final_value;
            vulnerableDebuff.times = time;
            vulnerableDebuff.OnApply(e);
        }
        //print("vulnerable layer num : " + final_value);
    }
}
