using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : SkillExecute
{
    //counter blow
    public int value = 30;

    public override void OnGetHitExec(GameObject target)
    {
        base.OnGetHitExec(target);
    }
}
