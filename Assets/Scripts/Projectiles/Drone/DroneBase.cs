using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DroneBase : MonoBehaviour
{
    public float health = 6;
    public float attack = 3;
    public float attack_interval = 1f;
    public bool canAttack = false;
    public bool canGetDamge = false;
    public bool canMove = false;
    public float atktimer = 0;
    public float speed = 10;
    public Vector3 move_direction;

 

    #region Move

    /// <summary>
    /// moving behavior
    /// </summary>
    public virtual void Move()
    {
        
    }

    public virtual void StartMove(Vector3 dir)
    {
        canMove = true;
    }

    #endregion

    #region behavior
    /// <summary>
    /// attack
    /// </summary>
    public virtual void Attack()
    {
   
    }

    /// <summary>
    /// be hit
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="dmgType"></param>
    public virtual void TakeDamage(float damage, DmgType dmgType)
    {
      
    }


    /// <summary>
    /// dead
    /// </summary>
    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion

}
