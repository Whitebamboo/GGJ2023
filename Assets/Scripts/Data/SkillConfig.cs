using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillConfig : ScriptableObject
{
    public Sprite skillImage;
    public string Description;
    public SkillType skilltype = SkillType.Base;
    public string Node_Effect;
    public int First_Compile_Order;
   


}
