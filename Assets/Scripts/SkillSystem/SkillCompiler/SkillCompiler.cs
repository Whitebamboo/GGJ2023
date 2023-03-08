using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
[System.Serializable]
public class SkillCompiler : CSingletonMono<SkillCompiler>
{
  
    /// <summary>
    /// compile a specific skill
    /// </summary>
    /// <param name="Skill_Code"></param>
    /// <param name="ability"></param>
    public virtual void Compile(string skill_code, TreeAttackModule tree_attack, Ability ability)
    {
        string[] skill_sentences = skill_code.Split(";");
        for(int i = 0; i < skill_sentences.Length; i++)
        {
            string[] words = skill_sentences[i].Split(" ");
            switch (words.Length)
            {
                case 1:
                    CompileSingleWords(words, tree_attack, ability);
                    break;
            }
        }
    }

    #region when the code sentences only a words

    /// <summary>
    /// if this code sentence only one words
    /// </summary>
    /// <param name="words"></param>
    private void CompileSingleWords(string[] words, TreeAttackModule tree_attack, Ability ability)
    {
        
        if(words[0] == "Bullet")
        {
            tree_attack.Bullet += 1;
        }
        else if(words[0] == "Shield")
        {
            tree_attack.Shield += 1;
        }
        else 
        {
            CompileKeyWord(words[0], ability);
        }
    }

    /// <summary>
    /// use reflection to compile the key words include adding num or else
    /// </summary>
    /// <param name="keyword"></param>
    private void CompileKeyWord(string keyword, Ability ability)
    {
        object obj = ability.GetAspect<Aspect>(keyword);
        Type t = Type.GetType(keyword);//get a reflection of this type
        if(t == null)
        {
            print("wrong key words:" + keyword);
            return;
        }
        if (obj != null)
        {
            //ability already have this execute instance 
            FieldInfo num_fi = t.GetField("num");//get its number parameter
            if (num_fi != null)
            {
                num_fi.SetValue(obj, (int)num_fi.GetValue(obj) + 1); //its number ++
            }
            else
            {
                print("wrong parameter in this ability:" + keyword + "did not have num");
            }   
        }
        else if (obj == null)
        {
            //ability did not have this execute instance
            obj = Activator.CreateInstance(t);
            ability.AddAspect<Aspect>((Aspect)obj, key: keyword); 
        }
        //test
        Aspect a = ability.GetAspect<Aspect>(keyword);
        FieldInfo n_fi = t.GetField("num");
        print(keyword + "'s number" + n_fi.GetValue(a).ToString());

    }
    #endregion
}

