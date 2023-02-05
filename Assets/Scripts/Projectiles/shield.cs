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
    public Vector3 move_direction;
    public float exist_time = 5f;//only exist 5 seconds
    private float disappear_timer = 0f;
    public float move_distance = 10f;//only leave 10 m and stop there to be attack by enemy
    public List<ElementsType> elements_list = new List<ElementsType>();//a list that do not get damage
    public float size = 1f;
    public bool isPenetrate = false;
    public int penetrate_times = 0;//will respawn from dead for each time
    public bool start_get_damge = false;
    public GameObject explode_Object;//need attach
    //a dictionary to restore elements restraint relationship, first item is the element, second will be the elemnts make 1/2 damage
    private static Dictionary<ElementsType, ElementsType> element_restraint_reverse = new Dictionary<ElementsType, ElementsType>() {
        {ElementsType.Fire,  ElementsType.Water },
        {ElementsType.Water, ElementsType.Wood},
        {ElementsType.Wood, ElementsType.Fire }
    };

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
    public void StartMove(Vector3 dir)
    {
        //rotate
        print("shield start move");
        
        dir = dir.normalized;
        move_direction = dir;
        //Quaternion.ve
        //transform.rotation = Quaternion.FromToRotation(transform.right, dir);
        StartCoroutine(PushEnemyMove(dir));
        //prepare to destroy it self by time
    }


    /// <summary>
    /// initial behavor
    /// </summary>
    /// <param name="info"></param>
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
    /// after finished push,can take damege
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    IEnumerator PushEnemyMove(Vector3 dir)
    {
        Tween t = transform.DOMove(this.transform.position + dir * move_distance, 1.5f);
        yield return t.WaitForCompletion();
        disappear_timer = exist_time;
        start_get_damge = true;
    }

    /// <summary>
    /// shield get damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage,Enemy enemy)
    {
        ElementsType enemy_type = enemy.element;
        if (start_get_damge)
        {
            if (elements_list.Contains(element_restraint_reverse[enemy_type]))
            {
                damage *= 1 / 2;//damage lower
            }
            health -= Mathf.Floor(damage);
            CheckDead(enemy);
        }
       
    }




    #region dead behavior
    /// <summary>
    /// only used in explode
    /// </summary>
    /// <param name="target"></param>
    public void MakeExplodeDamage(GameObject target)
    {
        Enemy e = target.GetComponent<Enemy>();
        //make damage
        float damage = ((health_base + health_add) * health_multiply);
        e.TakeDamage(damage / 2);//make an damge of 1/2 health
    }



    /// <summary>
    /// check if the health lower than 0
    /// </summary>
    private void CheckDead(Enemy enemy)
    {
        if(health <= 0)
        {

            //ondead effect
            CallOnDeadEffect(enemy);
            if (isPenetrate && (penetrate_times > 0))
            {
                health = (health_base + health_add) * health_multiply;
                penetrate_times -= 1;
                disappear_timer = exist_time;
                //print("shield respawn");
            }
            else
            {
                Dead();
            }
        }
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
    private void CallOnDeadEffect(Enemy enemy)
    {
        List<OnDeadEffect> ondead_list = new List<OnDeadEffect>(GetComponents<OnDeadEffect>());
        if (ondead_list.Count == 0)
        {
            return;
        }
        foreach (OnDeadEffect ondeadComponent in ondead_list)
        {
            ondeadComponent.OnDeadBehavior(enemy);//an onhit behavior will be override later
        }
    }
    #endregion
}
