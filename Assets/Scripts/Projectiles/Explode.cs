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
        if (only_once)
        {
            only_once = false;
            base.OnHitBehavior(enemy);
            print("explode !!!!");//TODO

            projectile p = GetComponent<projectile>();
            //create sphere
            GameObject explode_express = Instantiate(p.explode_Object, this.transform.position, Quaternion.identity);
            explode_express.transform.localScale = Vector3.one * explode_radius;


            //get explode hit
            RaycastHit[] explode_targets = Physics.SphereCastAll(this.transform.position, radius * explode_radius, p.move_direction, 0.01f);
            foreach (RaycastHit target in explode_targets)
            {
                if (target.transform.tag == "Enemy")
                {
                    p.MakeExplodeDamage(target.transform.gameObject);
                }
            }
        }
    

    }
    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius * explode_radius);
    }
}
