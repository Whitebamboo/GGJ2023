using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// what type of skill,effect first compiling in tree attack module
/// </summary>
public enum SkillType
{
    Base,//bullet or shiled
    Skill,//skill node
    Hextech,//hextech
}

/// <summary>
/// element type
/// 
/// </summary>
public enum ElementsType
{
    Fire,
    Water,
    Wood,
    Thunder,
    Ice,
}


/// <summary>
/// requirement restrict
/// </summary>
public enum ReqProjectileType
{
    None,
    Bullet,
    Shield,
    Both,
}


public enum TriggerTime
{
    onHit,
    onDamage,
    After_onHit,
    onCreate,
    onCompile,
    onGetHit,
}