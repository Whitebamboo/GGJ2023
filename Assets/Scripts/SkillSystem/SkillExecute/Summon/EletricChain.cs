using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricChain : SkillExecute
{

    public int layer;//equal to thunder elements
    public int baseSpreadNum;
    public int value;
    public EletricChain()
    {
        num = 1;
        layer = 1;
        baseSpreadNum = 3;
        value = 10;
    }

    public override void AfterOnHitExec(GameObject target)
    {
        base.AfterOnHitExec(target);
        Enemy e = target.GetComponent<Enemy>();
        ParalysisDebuff p = e.debuffContainer.GetAspect<ParalysisDebuff>();
        if(p != null && p.times > 0)
        {
            //find neareast enemys
            Collider[] cs = Physics.OverlapSphere(target.transform.position, 10f);//only detect enemy
            List<Enemy> spreadList = new List<Enemy>();
            spreadList.Add(e);
            print("how many enemy in this place:" + cs.Length.ToString());
            if (cs.Length > 0)
            {
                foreach (Collider c in cs)
                {
                    Enemy near_enemy = c.GetComponentInParent<Enemy>();
                    if (spreadList.Contains(near_enemy))
                    {
                        continue;
                    }
                    else if (near_enemy)
                    {
                        //create a projectile target to the object
                        GameObject.FindObjectOfType<TreeAttackModule>().CreateEletricChain(this,e.transform.position, near_enemy);
                        spreadList.Add(near_enemy);
                    }

                }
            }
        }
    }
}
