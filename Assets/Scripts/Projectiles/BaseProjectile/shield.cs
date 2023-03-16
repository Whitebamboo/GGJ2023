using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Shield : projectile
{
    //Base_attribute
    public float health = 15;//real health


    public float exist_time = 5f;//only exist 5 seconds
    private float disappear_timer = 0f;//timer count its disappear
    public float push_distance = 10f;//only leave 10 m and stop there to be attack by enemy
    public bool start_get_damge = false;//the shield start to get damage and make onhit



    private void Start()
    {

    }

    public void Update()
    {
        if (start_get_damge)
        {
            if (disappear_timer > 0)
            {
                disappear_timer -= Time.deltaTime;
            }
            else
            {
                Dead();
            }
        }


    }

    public override void InstantiateInit(Ability _ability)
    {
        base.InstantiateInit(_ability);
        start_get_damge = false;
        disappear_timer = 3;//not start to do the real count down
        attack = health;
    }



    public override void StartMove(Vector3 dir)
    {
        base.StartMove(dir);
        //rotate
        print("shield start move");
        transform.LookAt(transform.position + move_direction);
        //transform.rotation = Quaternion.FromToRotation(transform.right, dir);
        StartCoroutine(PushEnemyMove(move_direction));
        //prepare to destroy it self by time
    }




    /// <summary>
    /// after finished push,can take damege
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    IEnumerator PushEnemyMove(Vector3 dir)
    {
        float push_time = push_distance / speed;

        Tween t = transform.DOMove(this.transform.position + dir * push_distance, push_time);
        yield return t.WaitForCompletion();
        disappear_timer = exist_time;
        start_get_damge = true;
    }

    /// <summary>
    /// shield get damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage, Enemy enemy)
    {
        //print("exit health :" + health);
        ElementsType enemy_type = enemy.element;
        if (start_get_damge)
        {
            ability.ExecSkill(TriggerTime.onGetHit, enemy.gameObject, damage);
            health -= Mathf.Floor(damage);
            CheckDead(enemy);//self dead
        }

    }




    #region dead behavior


    /// <summary>
    /// check if the health lower than 0
    /// </summary>
    private void CheckDead(Enemy enemy)
    {
        if (health <= 0)
        {
            //ondead effect TODO
            ability.ExecSkill(TriggerTime.onDead, enemy.gameObject);//some ondead effect focus on enemy

            Dead();

        }
    }


    /// <summary>
    /// all dead behavior call this
    /// </summary>
    private void Dead()
    {
        ability.ExecSkill(TriggerTime.onDead, this.gameObject);//some ondead effect focus on it self
        Destroy(this.gameObject);
    }
    #endregion
}
