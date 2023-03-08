using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodOnhit : onHitEffect
{
    public float wood_damageIncrease = 5f;//need to used as 1+5%

    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);
        print("add damage increase buff to enemy");
        GameObject particle = Instantiate(GetComponent<projectile>().onhit_particle_list[2], enemy.transform.position, Quaternion.identity);
        Destroy(particle, 1f);
        Enemy e = enemy.GetComponentInParent<Enemy>();
        e.AddDebuff(ElementsType.Wood, wood_damageIncrease);
    }
}
