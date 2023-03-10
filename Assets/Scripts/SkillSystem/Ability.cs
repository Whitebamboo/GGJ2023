using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Container
{
    public int Bullet = 0;
    public int Shield = 0;
    #region behavior

    public void ExecSkill(TriggerTime trigger, GameObject target)
    {
        switch (trigger)
        {
            case TriggerTime.onHit:
                foreach(SkillExecute s in Aspects())
                {
                    s.onHitExec(target);
                }
                break;
            case TriggerTime.onDamage:
                foreach (SkillExecute s in Aspects())
                {
                    s.onDamageExec(target);
                }
                break;
            case TriggerTime.onCreate:
                foreach (SkillExecute s in Aspects())
                {
                    s.onCreateExec(target);
                }
                break;
            case TriggerTime.onCompile:
                foreach (SkillExecute s in Aspects())
                {
                    s.OnCompileExec(target);
                }
                break;
            case TriggerTime.After_onHit:
                foreach (SkillExecute s in Aspects())
                {
                    s.AfterOnHitExec(target);
                }
                break;
            case TriggerTime.onDead:
                foreach (SkillExecute s in Aspects())
                {
                    s.OnDeadExec(target);
                }
                break;
                //TODO



        }
    }

    public void ExecSkill(TriggerTime trigger, GameObject target, float damage)
    {
        switch (trigger)
        {
            case TriggerTime.onGetHit:
                foreach (SkillExecute s in Aspects())
                {
                    s.OnGetHitExec(target,damage);
                }
                break;
        }
    }
    #endregion
}
