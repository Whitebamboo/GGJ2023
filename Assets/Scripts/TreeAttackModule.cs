using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackModule : MonoBehaviour
{
    //only for test
    public List<SkillConfig> config_list = new List<SkillConfig>();
    private List<TreeNode> treeNode_list = new List<TreeNode>();
    //
 
    public GameObject projectile_prefab;
    public GameObject shield_prefab;
    public Transform projectile_spawn_point;
    public int attacknode_number = 0;
    public int defendnode_number = 0;
    public float radius = 5;
    public ProjectileCreateInfo createInfo;
    public ElementTotalInfo elementCreateInfo;
    public int node_number_multiply = 1;//only for attack and shild base mode

   

    private void Start()
    {
        //only for test
        foreach(var c in config_list)
        {
            TreeNode t = new TreeNode(c);
            treeNode_list.Add(t);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ProcessTreeNodes(treeNode_list);
        }
    }
    /// <summary>
    /// Process tree nodes
    /// </summary>
    /// <param name="nodes"></param>
    public void ProcessTreeNodes(List<TreeNode> nodes)
    {
        //init some variable
        attacknode_number = 0;
        defendnode_number = 0;
        node_number_multiply = 1;
        //first get how manys base nodes in it
        createInfo = new ProjectileCreateInfo();
        elementCreateInfo = new ElementTotalInfo();
        foreach (TreeNode n in nodes)
        {
            if (n.skillConfig == null)
                continue;

            SkillConfig s = n.skillConfig;
            switch (s.skilltype)
            {
                case SkillType.Base:
                    CompileBaseNode(s);
                    break;
                case SkillType.Elements:
                    CompileElementsNode(s);
                    break;
                case SkillType.BattleCryBehavior:
                    CompileBattleCryBehavior(s);//TODO
                    break;
                case SkillType.OnHitBehavior:
                    CompileOnHitBehavior(s);
                    break;
                case SkillType.Attibutes:
                    CompileAttributes(s);
                    break;
            }
        }


        //final multiply for the attack node
        attacknode_number *= node_number_multiply;
        defendnode_number *= node_number_multiply;
        //when have a multply node for base node
        //
        //create attack projectile
        //find position
        List<Vector3> dir =  FindClosestEnemyPosition(attacknode_number);
        List<Vector3> defend_dir = FindClosestEnemyPosition(defendnode_number);
       
        //create attack projectiles
        if((dir != null) && (dir.Count > 0))
        {
            for (int i = 0; i < attacknode_number; i++)
            {
                GameObject go = Instantiate(projectile_prefab, projectile_spawn_point);
                projectile p = go.GetComponent<projectile>();
                //set go parameters
                p.SetProjectileParameters(createInfo);
                //add onhit effect to go
                AddOnhitComponentToProjectile(go);
                //set go move direction and start move
                //print(projectile_spawn_point.position);
                p.StartMove(dir[i] - projectile_spawn_point.position);

            }
        }
        
        if((defend_dir != null) && (defend_dir.Count > 0))
        {
            //create defend shield
            for (int i = 0; i < defendnode_number; i++)
            {
                GameObject go = Instantiate(shield_prefab, projectile_spawn_point);
                shield s = go.GetComponent<shield>();
                //set shild parameters
                s.SetShieldParameters(createInfo);
                //set on dead effect
                AddOnDeadComponentToShield(go);
                //start move
                s.StartMove(defend_dir[i] - projectile_spawn_point.position);
            }
        }
       
    }

    /// <summary>
    /// add when on dead
    /// </summary>
    /// <param name="shield"></param>
    private void AddOnDeadComponentToShield(GameObject shield)
    {
        if (createInfo.isExplode)
        {
            OnDeadExplode e = shield.AddComponent<OnDeadExplode>();
            e.explode_radius = createInfo.explode_radius;

        }
    }

    /// <summary>
    /// add a on hit component to projectile
    /// </summary>
    private void AddOnhitComponentToProjectile(GameObject projectile)
    {
        if (createInfo.isExplode)
        {
            Explode e =  projectile.AddComponent<Explode>();
            e.explode_radius = createInfo.explode_radius;
         
        }
        if (elementCreateInfo.isFire)
        {
            projectile.GetComponent<projectile>().AddElementParticle(ElementsType.Fire);
            Fire f =  projectile.AddComponent<Fire>();
            f.flame_damage = elementCreateInfo.fire_damage;
           
        }
        if (elementCreateInfo.isWater)
        {
            projectile.GetComponent<projectile>().AddElementParticle(ElementsType.Water);
        
            Water w = projectile.AddComponent<Water>();
            w.water_decelerate = elementCreateInfo.water_decelerate;
            
        }
        if (elementCreateInfo.isWood)
        {
            projectile.GetComponent<projectile>().AddElementParticle(ElementsType.Wood);
            Wood w = projectile.AddComponent<Wood>();
            w.wood_damageIncrease = elementCreateInfo.wood_damageIncrease;
        }
    }






    /// <summary>
    /// find a closest enemy
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private List<Vector3> FindClosestEnemyPosition(int num)
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


    #region Compile Node infomaion
    /// <summary>
    /// compile base Node
    /// </summary>
    /// <param name="s"></param>
    private void CompileBaseNode(SkillConfig s)
    {
        switch (s.baseType)
        {
            case BaseType.Attack:
                attacknode_number += 1;
                break;
            case BaseType.Shiled:
                defendnode_number += 1;
                break;
        }

        
    }

    /// <summary>
    /// compile elements node
    /// </summary>
    /// <param name="s"></param>
    private void CompileElementsNode(SkillConfig s)
    {
        createInfo.elements_list.Add(s.elementType);
        switch (s.elementType)
        {
            case ElementsType.Fire:
                elementCreateInfo.isFire = true;
                elementCreateInfo.fire_damage += s.elements_parameters[0].value;
                break;
            case ElementsType.Water:
                elementCreateInfo.isWater = true;
                elementCreateInfo.water_decelerate += s.elements_parameters[0].value;
                break;
            case ElementsType.Wood:
                elementCreateInfo.isWood = true;
                elementCreateInfo.wood_damageIncrease += s.elements_parameters[0].value;
                break;
        }
    
    }

    private void CompileBattleCryBehavior(SkillConfig s)
    {

    }

    private void CompileOnHitBehavior(SkillConfig s)
    {
        switch (s.onHitBehaviorType)
        {
            case OnHitBehaviorType.Explode:
                createInfo.isExplode = true;
                createInfo.explode_radius += s.onhit_parameters[0].value;//"explode radius"
                break;
            case OnHitBehaviorType.Penetrate:
                createInfo.isPenetrate = true;
                createInfo.penetrate += 1;
                break;
        }
    }

    private void CompileAttributes(SkillConfig s)
    {
        switch (s.attributetype)
        {
            case Attributetype.NodeNumber:
                //only for multiply
                node_number_multiply *= (int)s.attribute_information[0].value;//how many attack node
                break;
            case Attributetype.AttackOrDefend:
                //for add or multiply
                if (s.attribute_information[0].parameters_name == "AttackOrDefend_Add")
                {
                    createInfo.attack_add += s.attribute_information[0].value;
                }
                else if(s.attribute_information[0].parameters_name == "AttackOrDefend_Mul")
                {
                    createInfo.attack_multiply *= s.attribute_information[0].value;
                }
                break;
            
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    #endregion

    /// <summary>
    /// sort by vector distance
    /// </summary>
    /// <returns></returns>
    private int SortByVector(Enemy a,Enemy b)
    {
        float a_dis = (a.transform.position - transform.position).magnitude;

        float b_dis = (b.transform.position - transform.position).magnitude;
        return (int)(a_dis - b_dis);
    }
}


/// <summary>
/// the infomation needs to creat each project
/// </summary>
public class ProjectileCreateInfo
{
    public float attack_add = 0;
    public float attack_multiply = 1;
    public bool isPenetrate = false;
    public int penetrate = 0;
    public bool isExplode = false;
    public float explode_radius = 0;
    public float size = 1;
    public List<ElementsType> elements_list = new List<ElementsType>();
    
}

/// <summary>
/// a class restore all elements parameters
/// </summary>
public class ElementTotalInfo
{
    public bool isFire = false;
    public float fire_damage = 0f;
    public bool isWater = false;
    public float water_decelerate = 0f;
    public bool isWood = false;
    public float wood_damageIncrease = 0f;

}
