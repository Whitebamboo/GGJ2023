using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjcetilePool : Pool
{
   
         

    public override void PoolInitial(GameObject go)
    {
        base.PoolInitial(go);
        go.transform.localPosition = Vector3.zero;
        
       
    }


    public override void PoolDead(GameObject go)
    {
        base.PoolDead(go);
        go.SetActive(false);
    }
}
