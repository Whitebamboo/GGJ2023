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
/// what type of elements
/// </summary>
public enum ElementsType
{
    Fire,
    Water,
    Wood,
}


public enum BattleCryBehaviorType
{
    Buff,
    Summon,
}


public enum OnHitBehaviorType
{
   Penetrate,
   Explode,
}