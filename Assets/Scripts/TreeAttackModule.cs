using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackModule : MonoBehaviour
{

    private void Start()
    {
   
   
    }

    private void Update()
    {

    }

    /// <summary>
    /// Process tree nodes
    /// </summary>
    /// <param name="nodes"></param>
    public void ProcessTreeNodes(List<TreeNode> nodes)
    {
        List<SkillConfig> turn_skill = new List<SkillConfig>();
        foreach(var n in nodes)
        {
            if (n.skillConfig)
            {
                turn_skill.Add(n.skillConfig);
            }
        }
        turn_skill.Sort(sortBySkillOrder);

        print(turn_skill[0].First_Compile_Order);
    }

    private int sortBySkillOrder(SkillConfig a, SkillConfig b)
    {
        return (int)(a.First_Compile_Order - b.First_Compile_Order);
    }

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




}

