using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet : SkillExecute
{
    public int puppetThreshold = 100;
    public override void AfterOnHitExec(GameObject target)
    {
        base.AfterOnHitExec(target);
        Debug.Log("make this puppet");
    }
}
