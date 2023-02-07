using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DroneBase : MonoBehaviour
{
    public float health = 6;
    public float attack_interval = 1f;
    public bool startAttack = false;
    public bool start_get_damge = false;
    private float timer = 0;
    public GameObject bullet;
    public float move_distance = 7;
    public Vector3 move_direction;
    private void Start()
    {
        
    }


    private void Update()
    {
        //attack
        Attack();
    }


    public void StartMove(Vector3 dir)
    {
        //rotate
        print("Drone start move");

        dir = dir.normalized;
        move_direction = dir;
        //Quaternion.ve

        //transform.LookAt(transform.position + dir);
        //transform.rotation = Quaternion.FromToRotation(transform.right, dir);
        StartCoroutine(FlyToAir(dir));
        //prepare to destroy it self by time
    }

    IEnumerator FlyToAir(Vector3 dir)
    {
        Tween t = transform.DOMove(this.transform.position + dir * move_distance, 1.5f);
        yield return t.WaitForCompletion();
        startAttack = true;
        start_get_damge = true;
    }

    /// <summary>
    /// attack
    /// </summary>
    private void Attack()
    {
        if (timer <= 0)
        {
            timer = attack_interval;
            CreatBullet();
        }
        timer -= Time.deltaTime;
    }
    public virtual void CreatBullet()
    {
        print("create bullet");
        //find a target
        List<Vector3> target_list =  FindObjectOfType<TreeAttackModule>().FindClosestEnemyPosition(1);
        GameObject go = Instantiate(bullet, this.transform);
        if((target_list != null) && (target_list.Count > 0)) 
        {
            go.GetComponent<DroneBullet>().StartMove(target_list[0] - transform.position);
        }
        
    }

    public void TakeDamage(float damage,DmgType dmgType)
    {
        if (start_get_damge)
        {
            health -= damage;
            CheckDead();
        }
    }

    private void CheckDead()
    {
        if (health <= 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }

}
