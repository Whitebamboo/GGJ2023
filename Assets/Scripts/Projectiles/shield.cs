using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class shield : MonoBehaviour
{
    //Base_attribute
    public float health_base = 6;
    private float health = 6;//real health
    public float health_add = 0;
    public float health_multiply = 1;

    public float exist_time = 5f;//only exist 5 seconds
    public float move_distance = 10f;//only leave 10 m and stop there to be attack by enemy
    public List<ElementsType> elements_list = new List<ElementsType>();//a list that do not get damage
    public float size = 1f;
    public bool isPenetrate = false;
    public int penetrate_times = 0;//will respawn from dead for each time
    public bool start_get_damge = false;
    //a dictionary to restore elements restraint relationship, first item is the element, second will be the elemnts make 1/2 damage
    private static Dictionary<ElementsType, ElementsType> element_restraint_reverse = new Dictionary<ElementsType, ElementsType>() {
        {ElementsType.Fire,  ElementsType.Water },
        {ElementsType.Water, ElementsType.Wood},
        {ElementsType.Wood, ElementsType.Fire }
    };

    public void Update()
    {
        if(exist_time > 0)
        {
            exist_time -= Time.deltaTime;
        }
        else
        {
            Dead();
        }
        
    }
    public void StartMove(Vector3 dir)
    {
        //rotate
        print("shield start move");
        dir = dir.normalized;
        //Quaternion.ve
        //transform.rotation = Quaternion.FromToRotation(transform.right, dir);
        StartCoroutine(PushEnemyMove(dir));
        //prepare to destroy it self by time
    }


    /// <summary>
    /// after finished push,can take damege
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    IEnumerator PushEnemyMove(Vector3 dir)
    {
        Tween t = transform.DOMove(this.transform.position + dir * move_distance, 1.5f);
        yield return t.WaitForCompletion();
        start_get_damge = true;
    }
    /// <summary>
    /// shield get damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage,ElementsType enemy_type)
    {
        if (start_get_damge)
        {
            if (elements_list.Contains(element_restraint_reverse[enemy_type]))
            {
                damage *= 1 / 2;//damage lower
            }
            health -= Mathf.Floor(damage);
            CheckDead();
        }
       
    }

    /// <summary>
    /// check if the health lower than 0
    /// </summary>
    private void CheckDead()
    {
        if(health <= 0)
        {
            if(isPenetrate && (penetrate_times > 0))
            {
                health = (health_base + health_add) * health_multiply;
                penetrate_times -= 1;
            }
            else
            {
                Dead();
            }
        }
    }
    public void SetShieldParameters(ProjectileCreateInfo info)
    {
        health_add = info.attack_add;
        health_multiply = info.attack_multiply;
        isPenetrate = info.isPenetrate;
        penetrate_times = info.penetrate;
        elements_list = info.elements_list;
        size = info.size;

        //calculate real health
        health = (health_base + health_add) * health_multiply;
    }

    /// <summary>
    /// all dead behavior call this
    /// </summary>
    private void Dead()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// shield effect happen on dead
    /// </summary>
    private void CallOnDeadEffect()
    {

    }
}
