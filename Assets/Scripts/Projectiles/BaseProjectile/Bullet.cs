using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : projectile
{

    private void Update()
    {
        Move();
    }
    #region behavior

    public override void InstantiateInit(Ability _ability)
    {
        base.InstantiateInit(_ability);
    }

    public override void StartMove(Vector3 dir)
    {
        base.StartMove(dir);
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
        if(other.tag == "Enemy")
        {
            Enemy e = other.gameObject.GetComponentInParent<Enemy>();
           
            e.TakeBaseDamage(attack);//get a base damage
            ability.ExecSkill(TriggerTime.onHit, e.gameObject);//on hit effect to enemy
            ability.ExecSkill(TriggerTime.onDamage, e.gameObject);//calculate final damage
            e.EnemyAfterOndamageCalCulate();
            ability.ExecSkill(TriggerTime.After_onHit, e.gameObject);//do after on hit effect
            e.TakeFinalDamage();
            Dead();
        }
    }

    public void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
