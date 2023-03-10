using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecute : Aspect
{
    public int num = 0;


    /// <summary>
    /// on each bullet or shield create trigger
    /// </summary>
    /// <param name="target"></param>
    public virtual void onCreateExec(GameObject target)
    {

    }

    /// <summary>
    /// on Hit trigger this
    /// </summary>
    /// <param name="target"></param>
    public virtual void onHitExec(GameObject target)
    {

    }

    /// <summary>
    /// when real did damage
    /// </summary>
    /// <param name="target"></param>
    public virtual void onDamageExec(GameObject target)
    {

    }

    /// <summary>
    /// after on hit
    /// </summary>
    /// <param name="target"></param>
    public virtual void AfterOnHitExec(GameObject target)
    {

    }

    /// <summary>
    /// a function will be call after all compile finished
    /// </summary>
    /// <param name="target"></param>
    public virtual void OnCompileExec(GameObject target)
    {
        
    }

    /// <summary>
    /// when enemies hit u 
    /// </summary>
    /// <param name="target"></param>
    public virtual void OnGetHitExec(GameObject target,float damage)
    {

    }

    /// <summary>
    /// when this object dead
    /// </summary>
    /// <param name="target"></param>
    public virtual void OnDeadExec(GameObject target)
    {

    }

}
