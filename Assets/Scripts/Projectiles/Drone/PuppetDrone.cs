using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a drone create from an enemy
/// </summary>
public class PuppetDrone : DroneBase
{
    public float attack_range = 1;
    public LayerMask layerMask;
    private Enemy attack_target;
    public void InstantiateInit(Enemy e, int inherit_mul)
    {
        //Inherit art
        GameObject mesh = Instantiate(e.model, this.transform);
        mesh.transform.tag = "Drone";
        mesh.layer = LayerMask.NameToLayer("Shield");
        health = e.health * (float)inherit_mul;
        attack = e.attack * (float)inherit_mul;
        attack_interval = e.attack_interval;
        atktimer = 0;

        move_direction = e.transform.position - FindObjectOfType<TreeAttackModule>().spawnPoint.position;
        //rotate to that side
        mesh.transform.localScale = new Vector3(mesh.transform.localScale.x, mesh.transform.localScale.y, -mesh.transform.localScale.z);

        //Initial UI
        GetComponentInChildren<HPBar>().InitialHP(health);

        canAttack = true;
        canGetDamge = true;
    }

    private void Update()
    {
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
        if(canAttack == false)
        {
            return;
        }
        if(atktimer <= 0)
        {
           
            FindAttackTarget();
            
            if (attack_target != null)
            {


                attack_target.TakeDamage(attack, DmgType.EnemyNormal);
            }
           
            atktimer = attack_interval;
        }
        else
        {
            atktimer -= Time.deltaTime;
        }
    }

    private void FindAttackTarget()
    {
        
        Collider[] cs = Physics.OverlapSphere(this.transform.position, attack_range, layerMask);
        if(cs.Length > 0)
        {
            
            for(int i = 0; i < cs.Length; i++)
            {
                if(cs[i].transform.tag == "Enemy")
                {
                    attack_target = cs[i].GetComponentInParent<Enemy>();
                    if (attack_target)
                    {
                        //print("find exits target: " + attack_target.transform.name);
                        return;
                    }
                }
            }
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawSphere(this.transform.position, attack_range);
    //}

    public override void TakeDamage(float damage, DmgType dmgType)
    {
        base.TakeDamage(damage, dmgType);
        health -= damage;
        GetComponentInChildren<HPBar>().UpdateHP(health);
        CheckDead();
    }
    private void CheckDead()
    {
        if(health <= 0)
        {
            Dead();
        }
    }


}
