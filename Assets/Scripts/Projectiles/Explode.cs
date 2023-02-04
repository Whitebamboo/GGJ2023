using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : onHitEffect
{
    public float explode_radius = 1f;

    public override void OnHitBehavior(GameObject enemy)
    {
        base.OnHitBehavior(enemy);
        print("explode !!!!");//TODO


    }
}
