using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : projectile
{
    public Vector3 startPoint;

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="e"></param>
    public void InstantiateInit(LightningCreator lt, Enemy e)
    {
        startPoint = new Vector3(e.transform.position.x, 20, e.transform.position.z);
        attack = lt.num * lt.layer * lt.value;//damage;
        move_direction = (e.transform.position - startPoint).normalized;//set moce direction here, dont need to set on other place
        //move_direction = new Vector3(0, -1, 0);
        speed = 100;
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        float dis = (this.transform.position - startPoint).magnitude;
        if(dis > 4)
        {
            float x = Random.Range(0f, 1f);
            float change = Random.Range(-0.1f, 0.1f);
            if (x > 0.8f)
            {
                move_direction += new Vector3(change, 0, 0);
                print("change dir");
            }
            startPoint = this.transform.position;
        }

        
        if (canMove)
        {
            this.transform.position += move_direction.normalized * speed * Time.deltaTime;
        }
        if (transform.localPosition.magnitude > 1000)//outside screen make it dead
        {
            Dead();
        }

    }


    public override void StartMove(Vector3 dir)
    {
        canMove = true;//start the animation showing a lightning dash down and calculate the damage
        this.transform.position = startPoint;
        transform.rotation = Quaternion.FromToRotation(transform.right, move_direction);
        //LightningDamage();//start make damage;
    }


    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Ground")
        {
            Dead();
        }
        else if(other.tag == "Enemy")
        {
            Enemy e = other.transform.GetComponentInParent<Enemy>();
            if (e)
            {

                e.TakeDamage(attack, DmgType.EnemyCritical);
            }
        }
    }

    /// <summary>
    /// a damage in a line
    /// </summary>
    private void LightningDamage()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(startPoint, move_direction, 100.0F);//a ray from top to down
        List<Enemy> alreadyDamageEnemy = new List<Enemy>();
        for (int i = 0; i < hits.Length; i++)
        {
            Enemy e = hits[i].transform.GetComponentInParent<Enemy>();
            if (e)
            {
                alreadyDamageEnemy.Add(e);
                e.TakeDamage(attack, DmgType.EnemyCritical);
            }
        }
    }
}
