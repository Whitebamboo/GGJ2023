using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementExecute : SkillExecute
{
    public int num;
    public float baseAttributeRestraintMultiplier = 1.3f;
    public List<ElementsType> RestraintElement;
    public override void onDamageExec(GameObject target)
    {
        base.onDamageExec(target);
        Enemy e = target.GetComponent<Enemy>();
        if (RestraintElement.Contains(e.GetElement()))
        {
            e.finalDamage *= Mathf.Pow(baseAttributeRestraintMultiplier, num);

        }
        //Debug.Log("do element execute on damage"+ num);
    }
}
