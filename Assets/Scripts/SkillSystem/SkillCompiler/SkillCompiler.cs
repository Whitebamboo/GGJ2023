using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
[System.Serializable]
public class SkillCompiler : CSingletonMono<SkillCompiler>
{
    private Ability current_ability;
  
    /// <summary>
    /// compile a specific skill
    /// </summary>
    /// <param name="Skill_Code"></param>
    /// <param name="ability"></param>
    public virtual void Compile(SkillConfig skill, Ability ability)
    {
        string skill_code = skill.SkillCode;
        current_ability = ability;
        //pre constraint
        if((skill.reqProjectileType == ReqProjectileType.Bullet) && (ability.Bullet <= 0))
        {
            return;
        }
        else if ((skill.reqProjectileType == ReqProjectileType.Shield) && (ability.Shield <= 0))
        {
            return;
        }
        else if ((skill.reqProjectileType == ReqProjectileType.Both) && ((ability.Shield <= 0) && (ability.Bullet <= 0)))
        {
            return;
        }



        //start parsing string

        //first check if there are : if branches
        if (skill_code.Contains(":"))
        {
            string[] branches = skill_code.Split(":"); //may be a == b : c == d : a += b; a += c;
            string ExecuteLinearCode = branches[branches.Length - 1];//a += b; a += c;
            int j = 0;//branch index need all to be true 
            while (j != branches.Length - 1)
            {
                if (CompileBranch(branches[j]))
                {
                    j++;
                    if (j == branches.Length - 1)
                    {
                        print("satisfice compile : " + ExecuteLinearCode);
                        CompileExecuteCode(ExecuteLinearCode);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            CompileExecuteCode(skill_code);
        }


       
        //test 

    }

    /// <summary>
    /// compile the code that execute
    /// </summary>
    /// <param name="skill_code"></param>
    public void CompileExecuteCode(string skill_code)
    {
        string[] skill_sentences = skill_code.Split(";");
        for (int i = 0; i < skill_sentences.Length; i++)
        {
            string code_sentence = skill_sentences[i].Trim();
            string[] words = code_sentence.Split(" ");
            switch (words.Length)
            {
                case 1:
                    CompileSingleWords(words);
                    break;
                case 3:
                    CompileThreeWords(words);
                    break;
            }


        }
    }

    #region branches if else if


    /// <summary>
    /// if branches compile
    /// </summary>
    /// <param name="branchString"></param>
    /// <returns></returns>
    private bool CompileBranch(string branchString)
    {

        branchString = branchString.Trim();
        string[] words = branchString.Split(" ");
        switch (words.Length)
        {
            case 1:
                break;
            case 3:
                return CompileBranchesWithThreeWords(words);
               
        }
        return false;
    }


    /// <summary>
    /// branch support == >= <= > < 
    /// </summary>
    /// <param name="words"></param>
    /// <returns></returns>
    private bool CompileBranchesWithThreeWords(string[] words)
    {
        string[] left_variables = words[0].Split(".");
        string[] right_variables = words[2].Split(".");
        string left_field_value = "";
        if(left_variables.Length == 2)
        {
            left_field_value = GetValue(left_variables[0], left_variables[1]).ToString();
            if (left_field_value == "0")
            {
                //dont have this skill in ability
                return false;
            }
        }
        else if(left_variables.Length == 1)
        {
            left_field_value = words[0];
        }
        else
        {
            print("wrong variable format in Branches" + words[0]);
        }
        string right_field_value = "";
        if (right_variables.Length == 2)
        {
            right_field_value = GetValue(right_variables[0], right_variables[1]).ToString();
            if (right_field_value == "0")
            {
                //dont have this skill in ability
                return false;
            }
        }
        else if (right_variables.Length == 1)
        {
            right_field_value = words[2];
        }
        else
        {
            print("wrong variable format in Branches" + words[2]);
        }
        //compare
        int left_val = 0;
        int right_val = 0;
        bool isLeftInt = int.TryParse(left_field_value, out left_val);
        bool isRightInt = int.TryParse(right_field_value, out right_val);
        if (isLeftInt && isRightInt)
        {
            switch (words[1])
            {
                case "==":
                    return left_val == right_val;
                case ">=":
                    return left_val >= right_val;
                case "<=":
                    return left_val <= right_val;
                case "<":
                    return left_val < right_val;
                case ">":
                    return left_val > right_val;
            }
        }
        else
        {
            print("only Support int value compare now");
        }

        return false;

    }


    #endregion

    #region when the code sentences only a words

    /// <summary>
    /// if this code sentence only one words
    /// </summary>
    /// <param name="words"></param>
    private void CompileSingleWords(string[] words)
    {
        if(words[0] == "")
        {
            return;
        }
        else if(words[0] == "Bullet")
        {
            current_ability.Bullet += 1;
        }
        else if(words[0] == "Shield")
        {
            current_ability.Shield += 1;
        }
        else 
        {
            CompileKeyWord(words[0]);
        }
    }

    /// <summary>
    /// use reflection to compile the key words include adding num or else
    /// </summary>
    /// <param name="keyword"></param>
    private void CompileKeyWord(string keyword)
    {
        object obj = current_ability.GetAspect<Aspect>(keyword);
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
               
                int new_value = (int)num_fi.GetValue(obj) + 1;
                num_fi.SetValue(obj, new_value); //its number ++
                //print(keyword + ":" +new_value);
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
            current_ability.AddAspect<Aspect>((Aspect)obj, key: keyword); 
        }
        //test
        //Aspect a = current_ability.GetAspect<Aspect>(keyword);
        //FieldInfo n_fi = t.GetField("num");
        //print(keyword + "'s number" + n_fi.GetValue(a).ToString());

    }
    #endregion


    #region compile three words
    /// <summary>
    /// three words compiler will like a = b; a+=b; a-=b; a*=b;
    /// </summary>
    /// <param name="words"></param>
    /// <param name="treeAttack"></param>
    /// <param name="ability"></param>
    private int CompileThreeWords(string[] words)
    {
        int result = 0;
        if(words[1] == "=")
        {
            
            Assign(words[0], words[2]);//assign value to another   
        }
        else if(words[1] == "+=" || words[1] == "-=" || words[1] == "*=" || words[1] == "/=")
        {
            CalculateAssign(words[0], words[1], words[2]);
        }
        return result;
    }


    /// <summary>
    /// for the signal "+=" "-=" "*=" "/="
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private void CalculateAssign(string left, string middle, string right)
    {
        string[] left_variables = left.Split(".");
        string left_field_value = "";
        if (left_variables.Length != 2)
        {
            print("assign fail: wrong attribute: " + left);
        }
        else
        {
            left_field_value = GetValue(left_variables[0], left_variables[1]).ToString();
        }
        //again require the left is a variable in an existing instance
        string[] right_variables = right.Split(".");
        string field_value = "";
        if (right_variables.Length == 2)
        {
            field_value = GetValue(right_variables[0], right_variables[1]).ToString();
           
        }
        else if (right_variables.Length == 1)
        {
            field_value = right_variables[0];
        }
        int left_val = 0;
        int right_val = 0;
        bool isInt = int.TryParse(field_value, out right_val);
        if (!isInt)
        {
            print("only Support int value assign right now");
        }
        else
        {
            isInt = int.TryParse(left_field_value, out left_val);
            if (!isInt)
            {
                print("only Support int value assign right now");
            }
            else
            {
                int result = 0;
                if(middle == "+=")
                {
                    result = left_val + right_val;
                }
                else if (middle == "-=")
                {
                    result = left_val - right_val;
                }
                else if (middle == "*=")
                {
                    result = left_val * right_val;
                }
                else if (middle == "/=")
                {
                    result = left_val / right_val;
                }

                SetValue(left_variables[0], left_variables[1], result);
            }
        }
    }


    

    /// <summary>
    /// play with the =
    /// </summary>
    /// <returns></returns>
    private void Assign(string left,string right)
    {
        string[] left_variables = left.Split(".");
        string[] right_variables = right.Split(".");
        if (left_variables.Length != 2)
        {
            print("assign fail: wrong attribute: " + left);
        }
        //get right value
        string field_value = "";
        if (right_variables.Length == 2)
        {
            field_value = GetValue(right_variables[0], right_variables[1]).ToString();
           
        }
        else if(right_variables.Length == 1)
        {
            field_value = right_variables[0];
        }

       
        //give that value to left
        int right_value = 0;
        bool isInt = int.TryParse(field_value, out right_value);
        if (isInt)
        {
            SetValue(left_variables[0], left_variables[1], right_value);
        }
        else
        {
            print("only Support int value assign right now");
        }
      

    }
    #endregion

    /// <summary>
    /// get a existing instance's value
    /// </summary>
    /// <param name="classtype"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    private object GetValue(string classtype, string parameter)
    {
        Aspect a = current_ability.GetAspect<Aspect>(classtype);
        if (a == null)
        {
            print("dont have this class execute instance: " + classtype);
            return 0;
        }
        Type t = Type.GetType(classtype);
        FieldInfo n_fi = t.GetField(parameter);
        return n_fi.GetValue(a);
    }

    /// <summary>
    /// set value to int
    /// </summary>
    /// <param name="classtype"></param>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    private void SetValue(string classtype, string parameter, int value)
    {
        Aspect a = current_ability.GetAspect<Aspect>(classtype);
        if(a == null)
        {
            print("dont have this execute instance" + classtype);
            return;
        }
        Type t = Type.GetType(classtype);
        FieldInfo n_fi = t.GetField(parameter);
        n_fi.SetValue(a, value);

    }
}

