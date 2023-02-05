using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeadEffect : MonoBehaviour
{
   /// <summary>
   /// rewrite this
   /// </summary>
   /// <param name="enemy"></param>
    public virtual void OnDeadBehavior(Enemy enemy)
    {
        if(enemy == null)
        {
            return;
        }
    }
}
