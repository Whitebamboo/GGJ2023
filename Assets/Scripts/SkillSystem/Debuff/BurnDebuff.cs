using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDebuff : Debuff
{
    public BurnDebuff(float _value, int _times)
    {
        value = _value;
        times = _times;
    }

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);
     
        if(value > 0)
        {
            enemy.GetComponentInChildren<DebuffEffectManager>().CreateEffect(DebuffType.Burn);
        }
        
    }

    public override void OnRepeat(Enemy enemy)
    {
        base.OnRepeat(enemy);
        if (enemy && value > 0)
        {
            enemy.TakeDamage(value, DmgType.EnemyNormal);
        }
        
    }

    public override void OnRemove(Enemy enemy)
    {
        base.OnRemove(enemy);
     
        enemy.GetComponentInChildren<DebuffEffectManager>().RemoveEffect(DebuffType.Burn);
    }
}
