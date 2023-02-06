
    using System;
    using Unity.VisualScripting;
    using UnityEngine;

    [Serializable]
    public class Debuff
    {
        public int configTimes = 4;
        public float configInterval = 1f;

        public ElementsType elementType;
        public float value;
        public int times;
        public float coolDown = 1f;

        public Debuff()
        {
            times = configTimes;
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

    public class DebuffCreator : CSingleton<DebuffCreator>
    {
        public Debuff Create(ElementsType elementsType)
        {
            switch (elementsType)
            {
                case ElementsType.Fire:
                    return new FireDebuff();
                case ElementsType.Water:
                    return new WaterDebuff();
                case ElementsType.Wood:
                    return new WoodDebuff();
                default:
                    return new Debuff();
            }
        }
    }

    public class FireDebuff : Debuff
    {
        public FireDebuff()
        {
            elementType = ElementsType.Fire;
        }

        public override void OnApply(Enemy enemy)
        {
            enemy.CreateEnemyDebuffEffect(ElementsType.Fire);
        }
        public override void OnRemove(Enemy enemy)
        {
            enemy.RemoveEnemyDebuffEffect(ElementsType.Fire);
        }
        public override void OnRepeat(Enemy enemy)
        {
            enemy.health -= value;
        }
    }

    public class WaterDebuff : Debuff
    {
        public WaterDebuff()
        {
            elementType = ElementsType.Water;
        }

        public override void OnApply(Enemy enemy)
        {
            enemy.speedDecreaseRate = value;
            enemy.CreateEnemyDebuffEffect(ElementsType.Water);
        }

        public override void OnRemove(Enemy enemy)
        {
            enemy.speedDecreaseRate = 0;
            enemy.RemoveEnemyDebuffEffect(ElementsType.Water);
        }
    }

    public class WoodDebuff : Debuff
    {
        public WoodDebuff()
        {
            elementType = ElementsType.Wood;
        }
        public override void OnApply(Enemy enemy)
        {
            enemy.damageIncreaseRate = value;
            enemy.CreateEnemyDebuffEffect(ElementsType.Wood);
        }

        public override void OnRemove(Enemy enemy)
        {
            enemy.damageIncreaseRate = 0;
            enemy.RemoveEnemyDebuffEffect(ElementsType.Wood);
        }
    }