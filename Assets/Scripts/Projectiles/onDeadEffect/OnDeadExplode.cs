using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeadExplode : OnDeadEffect
{
    public float explode_radius = 1f;
    private bool only_once = true;
    public float radius = 1f;//base value

    public override void OnDeadBehavior(Enemy enemy)
    {
       
        if (only_once)
        {
            only_once = false;
            base.OnDeadBehavior(enemy);
            print("explode !!!!");

            shield s = GetComponent<shield>();
            //create sphere
            GameObject explode_express = Instantiate(s.explode_Object, this.transform.position, Quaternion.identity);
            explode_express.transform.localScale = Vector3.one * explode_radius;


            //get explode hit
            RaycastHit[] explode_targets = Physics.SphereCastAll(this.transform.position, radius * explode_radius, s.move_direction, 0.01f);
            foreach (RaycastHit target in explode_targets)
            {
                if (target.transform.tag == "Enemy")
                {
                    s.MakeExplodeDamage(target.transform.gameObject);
                }
            }
        }
    }
}
