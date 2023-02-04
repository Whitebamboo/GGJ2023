using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : onHitEffect
{
    public float wood_damageIncrease = 5f;//need to used as 1+5%

    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);
        print("add damage increase buff to enemy");

        Enemy e = enemy.GetComponent<Enemy>();
        e.AddDebuff(ElementsType.Wood, wood_damageIncrease);
    }
}
