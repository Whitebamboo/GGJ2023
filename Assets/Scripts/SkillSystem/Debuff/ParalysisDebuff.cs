using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysisDebuff : Debuff
{
    public ParalysisDebuff(float _value, int _times)
    {
        value = _value;
        times = _times;
    }

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);
        enemy.paralysisRate = value;
        if (value > 0)
        {
            enemy.GetComponentInChildren<DebuffEffectManager>().CreateEffect(DebuffType.Paralysis);
        }
        print("create paralysis : " + value);

    }

    public override void OnRemove(Enemy enemy)
    {
        base.OnRemove(enemy);
        enemy.paralysisRate = 0;
        enemy.GetComponentInChildren<DebuffEffectManager>().RemoveEffect(DebuffType.Paralysis);
    }
}
