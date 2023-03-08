using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillConfig : ScriptableObject
{
    public Sprite skillImage;
    public string name;
    public int id;
    public string description;
    public SkillType skilltype = SkillType.Base;
    public ReqProjectileType reqProjectileType = ReqProjectileType.Bullet;
    public TriggerTime triggerType = TriggerTime.onHit;
    public string SkillCode;
    public int compile_Order;//0,1,2,3,4
   


}
