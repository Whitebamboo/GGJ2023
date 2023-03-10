using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyWords", menuName = "ScriptableObjects/KeyWords")]
public class KeyWordConfig : ScriptableObject
{
    public List<KeyWord> keyWords;//keywords have own ability
    public List<string> reservedWord;//verb, what should we do to this key words
    public List<string> ValueWord;//keyword's time or value, or shield's hp or bullet attack
}

[System.Serializable]
/// <summary>
/// single key word structure
/// </summary>
public struct KeyWord
{
    public string keyWord;
    public List<float> keyWordValue;

}
