using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeadEffect : MonoBehaviour
{
   /// <summary>
   /// rewrite this
   /// </summary>
   /// <param name="enemy"></param>
    public virtual void OnDeadBehavior(GameObject enemy)
    {
        if(enemy == null)
        {
            return;
        }
    }
}
