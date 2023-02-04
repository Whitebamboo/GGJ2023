using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillConfig : ScriptableObject
{
    public Sprite skillImage;
    public SkillType skilltype = SkillType.Base;
    //the infomation that we can input
    //elements_parameters
    public ElementsType elementType = ElementsType.Fire; 
    public List<FloatParameters> elements_parameters = new List<FloatParameters>();
    //BattleCry_parameters
    public BattleCryBehaviorType battleCryBehaviorType = BattleCryBehaviorType.Buff;
    //onhit behavior
    public OnHitBehaviorType onHitBehaviorType = OnHitBehaviorType.Explode;
    //Attribute behavior
    public Attributetype attributetype = Attributetype.AttackOrDefend;
    public List<FloatParameters> attribute_infomation = new List<FloatParameters>();
    
    
}
