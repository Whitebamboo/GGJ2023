using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackModule : MonoBehaviour
{
    public GameObject projectile_prefab;
    public Transform projectile_spawn_point;
    public int attacknode_number = 1;
    public float radius = 20;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ProcessTreeNodes(null);
        }
    }
    public void ProcessTreeNodes(List<TreeNode> nodes)
    {
        //first get how manys base nodes in it

        //find position
        List<Vector3> dir =  FindClosestEnemyPosition(attacknode_number);
        if(dir == null)
        {
            print("find attack direction wrong");
            return;
        }
        //create projectiles
        for(int i = 0; i < attacknode_number; i++)
        {
            GameObject go = Instantiate(projectile_prefab, projectile_spawn_point);
            projectile p = go.GetComponent<projectile>();
            //set go parameters
            //add onhit effect to go
            //set go move direction and start move
            print(projectile_spawn_point.position);
            p.StartMove(dir[i] - projectile_spawn_point.position);
            
        }
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
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 1f);
        List<RaycastHit> raycast_list = new List<RaycastHit>(raycastHits);
        print("hit list len " + raycast_list.Count);
        while(raycast_list.Count > 0 && num > 0)
        {
            if(raycast_list[0].transform.tag != "Enemy")
            {
                raycast_list.RemoveAt(0);
                continue;
            }
           
            enemy_position.Add(raycast_list[0].transform.position);
            raycast_list.RemoveAt(0);
            num--;
        }
        if(enemy_position.Count == 0)
        {
            //try get position from enemy list
            //if that doesn't work 
            return null;
        }
        else if((enemy_position.Count>0) && (num > 0))
        {
            print("did not find enough enemies, just repeat on same path");
            for(int i = 0; i < num; i++)
            {
                enemy_position.Add(enemy_position[0]);
            }
            return enemy_position;
        }

        return enemy_position;
        


    }
}
