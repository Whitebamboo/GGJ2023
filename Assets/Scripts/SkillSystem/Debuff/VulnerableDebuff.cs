using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableDebuff : Debuff
{
    public VulnerableDebuff(float _value, int _times)
    {
        value = _value;
        times = _times;
    }

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);
        enemy.damageIncreaseRate = value;

    }

    public override void OnRemove(Enemy enemy)
    {
        base.OnRemove(enemy);
        enemy.damageIncreaseRate = 0;
    }
}
