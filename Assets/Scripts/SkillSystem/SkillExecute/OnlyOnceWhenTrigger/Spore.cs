using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : SkillExecute
{
    public int spreadNum;//how many target to spread
    
    
    public Spore()
    {
        num = 1;
        spreadNum = 3;
    }


    public override void AfterOnHitExec(GameObject target)
    {
        base.AfterOnHitExec(target);
        Enemy e = target.GetComponent<Enemy>();
        VulnerableDebuff v = e.debuffContainer.GetAspect<VulnerableDebuff>();//find debuff
        if(v != null && v.times > 0)
        {
            //find neareast enemys
            Collider[] cs = Physics.OverlapSphere(target.transform.position, 10f);//only detect enemy
            List<Enemy> spreadList = new List<Enemy>();
            spreadList.Add(e);
            print("how many enemy in this place:" +cs.Length.ToString());
            if(cs.Length > 0)
            {
                foreach(Collider c in cs)
                {
                    Enemy near_enemy = c.GetComponentInParent<Enemy>();
                    if (spreadList.Contains(near_enemy))
                    {
                        continue;
                    }
                    else if (near_enemy)
                    {
                        VulnerableDebuff new_vulnerable = new VulnerableDebuff(v.value, v.times);
                        near_enemy.debuffContainer.AddAspect<VulnerableDebuff>(new_vulnerable);
                        new_vulnerable.OnApply(near_enemy);
                        spreadList.Add(near_enemy);
                        if (spreadList.Count > spreadNum + 1)
                        {
                            return;
                        }
                    }
                   
                }
            }
            
        }
        print("spread vulnerable debuff : " + spreadNum);
    }
}
