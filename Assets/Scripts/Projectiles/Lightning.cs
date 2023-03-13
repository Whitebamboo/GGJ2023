using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : projectile
{
    public Vector3 startPoint;
    private Vector3 sectionStartPoint;
    private List<Vector3> newDir;
    private Vector3 newTargetPos;
    private int offset_ind = 0;
    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="e"></param>
    public void InstantiateInit(LightningCreator lt, Enemy e)
    {
        startPoint = new Vector3(e.transform.position.x, 16, e.transform.position.z);
        attack = lt.num * lt.layer * lt.value;//damage;
        move_direction = (e.transform.position - startPoint).normalized;//set moce direction here, dont need to set on other place
       
        speed = 100;
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {


        if (canMove)
        {


            if (offset_ind == 0)
            {
                this.transform.position += newDir[offset_ind].normalized * speed * Time.deltaTime;
                if ((this.transform.position - sectionStartPoint).magnitude > 3)
                {
                    offset_ind++;
                    sectionStartPoint = this.transform.position;
                }
            }
            else if (offset_ind == 1)
            {
                this.transform.position += newDir[offset_ind].normalized * speed * Time.deltaTime;
                if ((this.transform.position - sectionStartPoint).magnitude > 4)
                {
                    offset_ind++;
                    //calculate a target point,which is the intersection of two line: a 
                    newTargetPos = VectorMathUtils.GetIntersection(startPoint, move_direction, transform.position, newDir[offset_ind]);
                }
            }
            else if (offset_ind == 2)
            {
                if((transform.position - newTargetPos).magnitude < .5f)
                {
                    offset_ind++;
                }
                else
                {
                    this.transform.position += newDir[offset_ind].normalized * speed * Time.deltaTime;
                }

            }
            else
            {
                this.transform.position += move_direction.normalized * speed * Time.deltaTime;
            }



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
        newDir = new List<Vector3>();
        float offset = Random.Range(-0.3f, 0.3f);
        newDir.Add(move_direction + new Vector3(offset, 0, 0));
        newDir.Add(move_direction + new Vector3(-offset, 0, 0));
        newDir.Add(move_direction + new Vector3(offset, 0, 0));
        offset_ind = 0;
        sectionStartPoint = startPoint;
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
