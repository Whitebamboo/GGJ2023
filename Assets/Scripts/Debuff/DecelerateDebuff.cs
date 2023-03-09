using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecelerateDebuff : Debuff
{
    public DecelerateDebuff(float _value, int _times)
    {
        value = _value;
        times = _times;
    }

    public override void OnApply(Enemy enemy)
    {
        base.OnApply(enemy);
        enemy.speedDecreaseRate = value;
        
    }

    public override void OnRemove(Enemy enemy)
    {
        base.OnRemove(enemy);
        enemy.speedDecreaseRate = 0;
    }
}
