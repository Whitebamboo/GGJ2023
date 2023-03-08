using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOnhit : onHitEffect
{
    public float water_decelerate = 10f;//need to used as 10%
    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);
        print("add decelerate buff to enemy");
        GameObject particle = Instantiate(GetComponent<projectile>().onhit_particle_list[1], enemy.transform.position, Quaternion.identity);
        Destroy(particle, 1f);
        Enemy e = enemy.GetComponentInParent<Enemy>();
        e.AddDebuff(ElementsType.Water, water_decelerate);
    }
}
