using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : onHitEffect
{
    public float water_decelerate = 10f;//need to used as 10%
    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);
        print("add decelerate buff to enemy");
        Enemy e = enemy.GetComponentInParent<Enemy>();
        e.AddDebuff(ElementsType.Water, water_decelerate);
    }
}
