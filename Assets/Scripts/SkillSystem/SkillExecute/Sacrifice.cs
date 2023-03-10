using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : SkillExecute
{
    public int value = 50;

    public Sacrifice()
    {
        num = 1;
        value = 50;
    }


    public override void OnDeadExec(GameObject target)
    {
        base.OnDeadExec(target);
        Shield s = target.GetComponent<Shield>();
        if (s)
        {
            ShieldValue sv = s.ability.GetAspect<ShieldValue>();
            if(sv != null)
            {
                float healval = num * value * 0.01f * sv.health; 
                GameObject.FindObjectOfType<Tree>().GetHeal(healval);
            }
        }
    }
}
