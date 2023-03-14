using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceConfig", menuName = "ScriptableObjects/ExperienceConfig")]
public class ExperienceData : ScriptableObject
{
    public List<ExperienceLevelData> experienceDatas;
    public List<SkillConfig> generalDropSkills;//when there are not setting in experience data;
}

/// <summary>
/// a data store level, experience to next, drop skill
/// </summary>
[System.Serializable]
public class ExperienceLevelData
{
    public int level;
    public int toNextLevelExp;
    public List<SkillConfig> dropSkills;
}
