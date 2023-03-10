using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffEffectManager : MonoBehaviour
{
    public List<DebuffEffects> debuffEffects = new List<DebuffEffects>();
    private Dictionary<DebuffType, GameObject> prepare_DebuffEffects = new Dictionary<DebuffType, GameObject>();
    private Dictionary<DebuffType, GameObject> exsit_debuffeffects = new Dictionary<DebuffType, GameObject>();

    private void Awake()
    {
        foreach( var d in debuffEffects)
        {
            prepare_DebuffEffects.Add(d.debuffType, d.effects_prefab);
        }
    }


    /// <summary>
    /// when debuff on apply
    /// </summary>
    public void CreateEffect(DebuffType dt)
    {
        if (exsit_debuffeffects.ContainsKey(dt))
        {
            exsit_debuffeffects[dt].SetActive(true);
        }
        else
        {
            GameObject go = Instantiate(prepare_DebuffEffects[dt],this.transform);
            exsit_debuffeffects.Add(dt, go);
        }
    }

    public void RemoveEffect(DebuffType dt)
    {
        if (exsit_debuffeffects.ContainsKey(dt))
        {
            exsit_debuffeffects[dt].SetActive(false);
        }
    }

}

[System.Serializable]
public struct DebuffEffects
{
    public DebuffType debuffType;
    public GameObject effects_prefab;
}