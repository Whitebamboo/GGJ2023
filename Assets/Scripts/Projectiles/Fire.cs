using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : onHitEffect
{
    public float flame_damage = 1f;

    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);

        print("add flame buff to enemy");

        Enemy e = enemy.GetComponent<Enemy>();
        e.AddDebuff(ElementsType.Fire, flame_damage);
    }


}
