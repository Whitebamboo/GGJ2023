using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballCreator : SkillExecute
{
    public int layer;
    public int sizeMul;//such as 120 equal to 120%
    public int speedMul;//such as 80 equal to 80%
    public int time;

    public FireballCreator()
    {
        num = 1;
        layer = 1;
        sizeMul = 100;
        speedMul = 100;
        time = 5;
    }
    public override void onCreateExec(GameObject target)
    {
        base.onCreateExec(target);
        print("speed "+speedMul);
        print("size " + sizeMul);
        projectile p = target.GetComponent<projectile>();
        TreeAttackModule tam = GameObject.FindObjectOfType<TreeAttackModule>();
        if(p && p.ability.GetAspect<Fire>() != null)
        {
            tam.CreateFireball(p);
        }
        
    }
}
