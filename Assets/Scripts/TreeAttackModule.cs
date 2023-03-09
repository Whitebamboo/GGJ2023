using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackModule : MonoBehaviour
{
    //for test 
    public List<SkillConfig> ns = new List<SkillConfig>();
    private List<TreeNode> ts = new List<TreeNode>();


    //real variable
    public Transform spawnPoint;
    public GameObject bullet_prefab;
    public Dictionary<long, Ability> abilityCombination = new Dictionary<long, Ability>();
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
        long com_num = GetAbilityCombinationNumber(turn_skill);
        Ability new_ability;
        if (abilityCombination.ContainsKey(com_num))
        {
            new_ability = abilityCombination[com_num];
            //print("existing ability combination" + com_num);
        }
        else
        {
            turn_skill.Sort(sortBySkillOrder);//from small to big

            //compile skill
            new_ability = new Ability();
            foreach (var skill in turn_skill)
            {
                SkillCompiler.instance.Compile(skill, new_ability);
            }

            abilityCombination.Add(com_num, new_ability);
            //print("new ability combination" + com_num);
        }
        bullet_num = new_ability.Bullet;
        shield_num = new_ability.Shield;
        //TODO Lauch 
        List<Vector3> lauch_dir = FindClosestEnemyPosition(Bullet);
        for (int i = 0; i < bullet_num; i++)
        {
            GameObject bullet_obj = Instantiate(bullet_prefab, spawnPoint);
            Bullet b = bullet_obj.GetComponent<Bullet>();
            b.InstantiateInit(new_ability);
            b.StartMove(lauch_dir[i] - spawnPoint.position);
        }



        //Debug
        //print("water num : " + new_ability.GetAspect<Water>().num);
        //new_ability.GetAspect<Decelerate>().onHitExec(null);
        ////print("layer num of decelerate: " + );
        //print("bullet num: " + bullet_num);
        //print("shield num: " + shield_num);
    }

    private long GetAbilityCombinationNumber(List<SkillConfig> skills)
    {
        long result = 1;
        foreach(var a in skills)
        {
            result *= a.PrimeId;
        }
        return result;
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


    #region summon game object
    public GameObject puppet_prefab;

    public void CreatePuppet(Enemy e,int inherit_mul)
    {
        GameObject go =  Instantiate(puppet_prefab, e.transform.position, Quaternion.identity);
        go.GetComponent<PuppetDrone>().InstantiateInit(e, inherit_mul);
        e.health = 0;
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

