using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceSystems : CSingletonMono<ExperienceSystems>
{
    private int experience;
    private int treelevel;
    public ExperienceData experienceData;//store the structure of experience data connect to level
    public List<SkillConfig> currentLevelSkills;
    public void Init()
    {
        experience = 0;
        treelevel = 0;
    }

    



    public void AddExp(float exp)
    {
        AddExp((int)exp);
        
    }

    public void AddExp(int exp)
    {
        experience += exp;
        if (experience >= experienceData.experienceDatas[treelevel].toNextLevelExp)
        {
            experience -= experienceData.experienceDatas[treelevel].toNextLevelExp;
            DropSkills();
            treelevel++;
            if(treelevel >= experienceData.experienceDatas.Count)
            {
                treelevel = experienceData.experienceDatas.Count - 1;
            }
            print("update to level : " + treelevel);

        }
    }


    /// <summary>
    /// print Drop skill, the level before add
    /// </summary>
    public void DropSkills()
    {
        if(experienceData.experienceDatas[treelevel].dropSkills == null || experienceData.experienceDatas[treelevel].dropSkills.Count == 0)
        {
            currentLevelSkills = experienceData.generalDropSkills;
            GameManager.instance.ProcessDropItemList(currentLevelSkills);
    
        }
        else
        {
            currentLevelSkills = experienceData.experienceDatas[treelevel].dropSkills;
            GameManager.instance.ProcessDropItemList(currentLevelSkills);
           
        }
        
    }
}
