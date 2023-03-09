using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillConfig : ScriptableObject
{
    public Sprite skillImage;
    public string name;
    public int id;
    public int PrimeId;//use to create an un repeat combination, if each skill is a prime we can use their multiple value to find a unique combination
    public string unlock_Req;//a descirption to define when this node unlock
    public string description;
    public SkillType skilltype = SkillType.Base;
    public ReqProjectileType reqProjectileType = ReqProjectileType.Bullet;
    public TriggerTime triggerType = TriggerTime.onHit;
    public string SkillCode;
    public int compile_Order;//0,1,2,3,4
   


}
