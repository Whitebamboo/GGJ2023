using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    public float attack = 3; 
    public float speed = 10;
    private Vector3 targe_dir;
    private bool canMove = false;
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public virtual void StartMove(Vector3 dir)
    {
        targe_dir = dir;
        transform.LookAt(transform.position + dir);
        canMove = true;
    }

    public virtual void Move()
    {
        if (canMove)
        {
            this.transform.position += targe_dir.normalized * speed * Time.deltaTime;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            if (other.tag == "Enemy")
            {

                other.transform.GetComponentInParent<Enemy>().TakeDamage(attack, DmgType.EnemyNormal);
                Dead();
          
            
            }
        }
    }

    public virtual void Dead()
    {
        Destroy(this.gameObject);
    }
         
}
