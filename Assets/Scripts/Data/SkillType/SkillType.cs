using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// what type of skill,effect first compiling in tree attack module
/// </summary>
public enum SkillType
{
    Base,//bullet or shiled
    Attribute,//calculate when compile
    OnHit,//calculate when onhit
    BeHit,//when be hit
    OnCreate,//when instantiate, create
    OnDead,//When dead will be calculate
}

/// <summary>
/// element type
/// </summary>
public enum ElementsType
{
    Fire,
    Water,
    Wood,
    Thunder,
    Ice,
}