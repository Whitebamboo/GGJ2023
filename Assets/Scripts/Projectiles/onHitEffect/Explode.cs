using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : onHitEffect
{
    public float explode_radius = 1f;
    private bool only_once = true;
    public float radius = 1f;//base value
    public override void OnHitBehavior(GameObject enemy)
    {


    }

}
