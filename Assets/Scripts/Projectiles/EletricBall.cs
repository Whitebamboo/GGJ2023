using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricBall : projectile
{
    private Enemy targetEnemy;
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// another function
    /// </summary>
    /// <param name="ec"></param>
    public void InstantiateInit(EletricChain ec, Enemy e)
    {
        targetEnemy = e;
        attack = (float)ec.num * ec.layer * ec.value;
        speed = 20;
        move_direction = (e.transform.position - transform.position).normalized;
    }
    public override void StartMove(Vector3 dir)
    {
        canMove = true;
        transform.rotation = Quaternion.FromToRotation(transform.right, move_direction);

        //trigger onCreate 
        //ability.ExecSkill(TriggerTime.onCreate, treeAttackModule.gameObject);//dont put it here maybe ,hard to manage
    }

    /// <summary>
    /// when hit enemy
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            
            Enemy e = other.gameObject.GetComponentInParent<Enemy>();
            if(e && e == targetEnemy)
            {
                e.TakeDamage(attack, DmgType.EnemyRestraint);
                Dead();
             
            }
         
           
        }
    }
}
