using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackModule : MonoBehaviour
{

    public List<SkillConfig> ns = new List<SkillConfig>();
    private List<TreeNode> ts = new List<TreeNode>();

    private int bullet_num = 0;
    private int shield_num = 0;
    private void Start()
    {
        //for test
        foreach(var i in ns)
        {
            TreeNode t = new TreeNode(i);
            ts.Add(t);
        }
   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ProcessTreeNodes(ts);
        }
    }


    /// <summary>
    /// Process tree nodes
    /// </summary>
    /// <param name="nodes"></param>
    public void ProcessTreeNodes(List<TreeNode> nodes)
    {
        //TODO Initial
        bullet_num = 0;
        shield_num = 0;
        // order the skill base on its compile order
        List<SkillConfig> turn_skill = new List<SkillConfig>();
        foreach(var n in nodes)
        {
            if (n.skillConfig)
            {
                turn_skill.Add(n.skillConfig);
            }
        }
        turn_skill.Sort(sortBySkillOrder);
        //compile skill
        Ability new_ability = new Ability();
        foreach(var skill in turn_skill)
        {
            SkillCompiler.instance.Compile(skill.SkillCode, this, new_ability);
        }
        //TODO Lauch 
        print("bullet num" + bullet_num);
        print("shield num" + shield_num);
    }

    private int sortBySkillOrder(SkillConfig a, SkillConfig b)
    {
        return (int)(a.compile_Order - b.compile_Order);
    }


    #region Target Finding Methods maybe add more later

    /// <summary>
    /// find a closest enemy
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public List<Vector3> FindClosestEnemyPosition(int num)
    {
        List<Vector3> enemy_position = new List<Vector3>();
        if(num == 0)
        {
            return null;
        }
        List<Enemy> enemy_targets_list = GameManager.instance.enemyManager.enemies;
        enemy_targets_list.Sort(SortByVector);
        if(enemy_targets_list.Count > 0)
        {
            if(enemy_targets_list.Count >= num)
            {
                for(int i = 0; i < num; i++)
                {
                    enemy_position.Add(enemy_targets_list[i].transform.position);
                }
            }
            else
            {
                for(int i = 0; i< enemy_targets_list.Count; i++)
                {
                    enemy_position.Add(enemy_targets_list[i].transform.position);
                    num--;
                }
                for(int i = 0; i < num; i++)
                {
                    enemy_position.Add(enemy_position[0]);
                }
            }
        }
        return enemy_position;
        


    }
    /// <summary>
    /// sort by vector distance
    /// </summary>
    /// <returns></returns>
    private int SortByVector(Enemy a, Enemy b)
    {
        float a_dis = (a.transform.position - transform.position).magnitude;

        float b_dis = (b.transform.position - transform.position).magnitude;
        return (int)(a_dis - b_dis);
    }


    #endregion


    #region Utils
    /// <summary>
    /// calculate bullet
    /// </summary>
    public int Bullet
    {
        get => bullet_num;
        set => bullet_num = value;
    }

    public int Shield
    {
        get => shield_num;
        set => shield_num = value;
    }
    #endregion
}

