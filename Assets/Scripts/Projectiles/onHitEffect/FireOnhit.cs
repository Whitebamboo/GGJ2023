using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOnhit : onHitEffect
{
    public float flame_damage = 1f;

    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);

        print("add flame buff to enemy");
        GameObject particle =  Instantiate(GetComponent<projectile>().onhit_particle_list[0],enemy.transform.position,Quaternion.identity);
        Destroy(particle, 1f);
        Enemy e = enemy.GetComponentInParent<Enemy>();
        e.AddDebuff(ElementsType.Fire, flame_damage);
    }


}
