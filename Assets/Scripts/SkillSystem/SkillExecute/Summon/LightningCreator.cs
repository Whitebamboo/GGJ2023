using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningCreator : SkillExecute
{
    public int layer;
    public float value;
   
    public LightningCreator()
    {
        num = 1;
        layer = 1;
        value = 20;

    }


    public override void onHitExec(GameObject target)
    {
        base.onHitExec(target);
        Enemy e = target.GetComponent<Enemy>();
        TreeAttackModule tam = GameObject.FindObjectOfType<TreeAttackModule>();
        if (e && tam.new_ability.GetAspect<Thunder>() != null)
        {
            tam.CreateLightning(this, e);
        }
    }
}
