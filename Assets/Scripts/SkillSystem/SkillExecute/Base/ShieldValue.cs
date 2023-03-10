using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldValue : BaseValueExec
{
    public float health;
    public int times;
    public float push_distance;
    
    //construct
    public ShieldValue()
    {
        health = 15;
        penetrate_times = 0;
        speed = 3;
        times = 5;
        push_distance = 10;
    }


    //on create

    public override void onCreateExec(GameObject target)
    {
        base.onCreateExec(target);
        Shield s = target.GetComponent<Shield>();
        if(s != null)
        {
            s.health = health;
            s.penetrate_times = penetrate_times;
            s.speed = speed;
            s.exist_time = times;
            s.push_distance = push_distance;
            s.attack = health;
        }
        //print("exec shield value");
    }
}
