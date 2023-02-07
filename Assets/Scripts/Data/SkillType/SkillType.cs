using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// what type of skill
/// </summary>
public enum SkillType
{
    Base,//bullet or shiled
    Elements,//fire, water, wood
    BattleCryBehavior,//when projectile create
    OnHitBehavior,//when projectile hit monster
    Attibutes,//number,attack,hp,size
}


/// <summary>
/// base node
/// </summary>
public enum BaseType
{
    Attack,
    Shiled,
}

/// <summary>
/// what type of elements
/// </summary>
public enum ElementsType
{
    Fire,
    Water,
    Wood,
}

/// <summary>
/// battle cry
/// </summary>
public enum BattleCryBehaviorType
{
    Buff,
    Summon,
}

/// <summary>
/// on hit
/// </summary>
public enum OnHitBehaviorType
{
   Penetrate,
   Explode,
}

/// <summary>
/// attributes
/// </summary>
public enum Attributetype
{
    NodeNumber,
    AttackOrDefend,
    Size,
}

[System.Serializable]
/// <summary>
/// a structure for Float parameters,include parameter name and value 
/// </summary>
public struct FloatParameters
{
    public string parameters_name;
    public float value;
}
[System.Serializable]
/// <summary>
/// a structure for string parameters
/// </summary>
public struct StringParameters
{
    public string parameters_name;
    public string value;
}