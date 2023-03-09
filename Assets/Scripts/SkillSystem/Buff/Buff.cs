using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Aspect
{
    public float configInterval = 1f;
    public float value;
    public int times;
    public float coolDown = 1f;

    public Buff()
    {
        coolDown = configInterval;
    }

    public virtual void OnApply(TreeAttackModule tree)
    {

    }

    public virtual void OnCompileRepeat(TreeAttackModule tree)
    {
       
    }

    public virtual void OnRemove(TreeAttackModule tree)
    {

    }

    
}
