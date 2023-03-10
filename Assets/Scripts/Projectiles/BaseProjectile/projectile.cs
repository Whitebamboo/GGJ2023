using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    //base attributes
    public float attack;//atk
    public float critic;//critic ratio
    public Ability ability;//ability when create
    public bool isPenetrate = false;
    public int penetrate_times = 0;
    public float speed = 10;
    public Vector3 move_direction = Vector3.zero;
    public bool canMove = false;
    public TreeAttackModule treeAttackModule;

  
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    #region behavior
    public virtual void InstantiateInit(Ability _ability)
    {
        //when this create
        ability = _ability;
        treeAttackModule = FindObjectOfType<TreeAttackModule>();

    }

    public virtual void Move()
    {
        if (canMove)
        {
            this.transform.position += move_direction.normalized * speed * Time.deltaTime;
        }
        if (transform.localPosition.magnitude > 1000)//outside screen make it dead
        {
            Dead();
        }
    }
    /// <summary>
    /// start move
    /// </summary>
    /// <param name="dir"></param>
    public virtual void StartMove(Vector3 dir)
    {
        //print("StartMove");
        //rotate to move dir
        dir = dir.normalized;
        move_direction = dir;
        canMove = true;
    
        
    }


    #endregion










    /// <summary>
    /// the bullet destroy it self in all situations
    /// </summary>
    private void Dead()
    {
        Destroy(this.gameObject);
    }

}
