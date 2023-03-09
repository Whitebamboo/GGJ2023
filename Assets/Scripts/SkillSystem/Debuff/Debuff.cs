
    using System;
    using Unity.VisualScripting;
    using UnityEngine;


[Serializable]
public class Debuff : Aspect
{
        
        public float configInterval = 1f;
        public float value;
        public int times;
        public float coolDown = 1f;

        public Debuff()
        {
            coolDown = configInterval;
        }

        public virtual void OnApply(Enemy enemy)
        {
            
        }

        public virtual void OnRepeat(Enemy enemy)
        {
            
        }

        public virtual void OnRemove(Enemy enemy)
        {

        }
}
