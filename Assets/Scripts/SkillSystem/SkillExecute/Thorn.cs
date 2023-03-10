using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : SkillExecute
{
    //counter blow
    public int value = 30;

    public Thorn()
    {
        num = 1;
        value = 30;
    }
    /// <summary>
    /// when get hit need to calculate the attack damage first
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    public override void OnGetHitExec(GameObject target,float damage)
    {

        base.OnGetHitExec(target,damage);
        Enemy e = target.GetComponent<Enemy>();
        if(e != null)
        {
            float counterAtk = damage * value * num * 0.01f;
            counterAtk = Mathf.Round(Mathf.Max(counterAtk, 1));
            e.TakeDamage(counterAtk, damage_type: DmgType.EnemyNormal);  
        }
        
    }
}
