using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onHitEffect : MonoBehaviour
{
   public virtual void OnHitBehavior(GameObject enemy)
    {
        if(enemy == null)
        {
            return;
        }
       
        //print("hit enemy");
    }

    /// <summary>
    /// this Component will dead when pool make it dead
    /// </summary>
    public virtual void PoolDead()
    {
        Destroy(this);
    }
}
