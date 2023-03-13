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
    public GameObject shield_prefab;
    public Dictionary<long, Ability> abilityCombination = new Dictionary<long, Ability>();
    private List<Vector3> lauch_dir;
    private int bullet_num = 0;
    private int shield_num = 0;

    //ability
    public Ability new_ability;

    //Buff value
    public BuffContainer buffContainer = new BuffContainer();
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
        BuffCountDown();
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
            print("new ability combination" + com_num);
        }

        //all on compile event
        new_ability.ExecSkill(TriggerTime.onCompile, this.gameObject);

        //after ability compile 
        //compile all buff
        BuffCompileRepeat();
        //end compile buff
        bullet_num = new_ability.Bullet;
        shield_num = new_ability.Shield;

        //TODO Lauch 
        lauch_dir = FindClosestEnemyPosition(Bullet);
        if(lauch_dir != null && lauch_dir.Count > 0)
        {
            for (int i = 0; i < bullet_num; i++)
            {
                GameObject bullet_obj = Instantiate(bullet_prefab, spawnPoint);
                Bullet b = bullet_obj.GetComponent<Bullet>();
                b.InstantiateInit(new_ability);
                b.StartMove(lauch_dir[i] - spawnPoint.position);

                //print(b.gameObject);
                new_ability.ExecSkill(TriggerTime.onCreate, b.gameObject);
            }
        }
        lauch_dir = FindClosestEnemyPosition(Shield);
        if (lauch_dir != null && lauch_dir.Count > 0)
        {
            for (int i = 0; i < shield_num; i++)
            {
                GameObject shield_obj = Instantiate(shield_prefab, spawnPoint);
                Shield s = shield_obj.GetComponent<Shield>();
                s.InstantiateInit(new_ability);
                s.StartMove(lauch_dir[i] - spawnPoint.position);
                new_ability.ExecSkill(TriggerTime.onCreate, s.gameObject);
            }
        }
        
     



        //Debug
        //print("water num : " + new_ability.GetAspect<Water>().num);
        //new_ability.GetAspect<Decelerate>().onHitExec(null);
        ////print("layer num of decelerate: " + );
        //print("bullet num: " + bullet_num);
        //print("shield num: " + shield_num);
    }

    /// <summary>
    /// when compile some buff will do something
    /// </summary>
    private void BuffCompileRepeat()
    {
        foreach(Buff b in buffContainer.Aspects())
        {
            if(b.times > 0)
            {
                b.OnCompileRepeat(this);
                //after repeat
                if (b.times <= 0)
                {
                    b.OnRemove(this);
                }
            }
           
        }
    }

    /// <summary>
    /// work in update each time interval , minus times
    /// </summary>
    public void BuffCountDown()
    {
        foreach (Buff b in buffContainer.Aspects())
        {
            if(b.times > 0)
            {
                b.coolDown -= Time.deltaTime;
                if (b.coolDown <= 0)
                {
                    b.times -= 1;
                    if (b.times <= 0)
                    {
                        b.OnRemove(this);
                    }
                    b.coolDown += b.configInterval;
                }
            }
         

        }
    }


    /// <summary>
    /// get a specific prime number to sign the ability
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
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

    public GameObject fireball_prefab;
    public void CreateFireball(projectile p)
    {
        GameObject go = Instantiate(fireball_prefab, spawnPoint.position, Quaternion.identity);
        go.GetComponent<Fireball>().InstantiateInit(p);
        go.GetComponent<Fireball>().StartMove(p.move_direction);
    }


    public GameObject lightning_prefab;
    public void CreateLightning(LightningCreator lt, Enemy e)
    {
        GameObject go = Instantiate(lightning_prefab, e.transform.position, Quaternion.identity);
        go.GetComponent<Lightning>().InstantiateInit(lt, e);
        go.GetComponent<Lightning>().StartMove(Vector3.zero);//todo maybe change this later;
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

