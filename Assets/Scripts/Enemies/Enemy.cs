using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public float health = 10;//HP
    public float attack = 3;//attack
    public float attack_interval = 1f;//attack each second
    public ElementsType element = ElementsType.Fire;
    public float speed = 0.5f;
    public float attackCoolDown = 0;//timer
    [SerializeField]
    public GameObject slashEffect;
    [SerializeField]
    public GameObject text;
    [SerializeField]
    public GameObject model;
    public List<SkillConfig> droppedSkills = new List<SkillConfig>();
    private Coroutine hideTextCoroutine;

    private Rigidbody rigidbody;
    private Coroutine attackCoroutine;
    public bool isAttacking = false;

    //debuff
    public DebuffContainer debuffContainer = new DebuffContainer();
    public float damageIncreaseRate = 0;//(0,1)
    public float speedDecreaseRate = 0;//(0,1)
    public float paralysisRate = 0;//(0,1) probablity that enemy may do nothing(no moving or atk, each second) TODO

    //debuff end

    public GameObject attack_target = null;

    [SerializeField] private float dropSkillChance = 0.3f;


    public float baseDamage = 0;//each on hit has a base damage
    public float finalDamage = 0;//the final damage it take's base on a series of execute
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        GetComponentInChildren<HPBar>().InitialHP(health);
    }



    /// <summary>
    /// function to get enemy element type
    /// </summary>
    /// <returns></returns>
    public ElementsType GetElement()
    {
        return element;
    }

    

    #region on Hit

    //new getting hit's logic will becom
    // 1. get base damage
    // 2. execute all onhit trigger
    // 3. execute all onDamage trigger
    // 4. execute all after On hit trigger
    public void TakeBaseDamage(float damage)
    {
        baseDamage = damage;
        finalDamage = damage;
    }

    /// <summary>
    /// a calulation made after bullet ondamage finished,include vulnerable or other
    /// </summary>
    public void EnemyAfterOndamageCalCulate()
    {
        finalDamage *= (1 + damageIncreaseRate);//vulnerable 
    }

    /// <summary>
    /// after all calculation do this damage
    /// </summary>
    public void TakeFinalDamage()
    {
       
        if(finalDamage < baseDamage)
        {
            TakeDamage(finalDamage, DmgType.EnemyWeak);
        }
        else if(finalDamage == baseDamage)
        {
            TakeDamage(finalDamage, DmgType.EnemyNormal);
        }
        else if((finalDamage < baseDamage * 1.5) && (finalDamage > baseDamage))
        {
            TakeDamage(finalDamage, DmgType.EnemyRestraint);
        }
        else if (finalDamage > 1.5)
        {
            TakeDamage(finalDamage, DmgType.EnemyCritical);
        }
    }


    /// <summary>
    /// function to actually make damage to enemy
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage, DmgType damage_type)
    {
        var dmg = damage ;
        health -= dmg;
        GameManager.instance.dmgTextManager.AddDmgText(dmg, damage_type, transform.position);
        GetComponentInChildren<HPBar>().UpdateHP(health);
        //print("on hit:" + health);//call UI utils function
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(0.5f);
        text.SetActive(false);
    }

    #endregion

    #region attack
    public void OnAttackTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") || (other.gameObject.CompareTag("Shield")) || (other.gameObject.CompareTag("Drone")))
        {
            isAttacking = true;
            rigidbody.velocity = new Vector3();
            attack_target = other.gameObject;
        }
    
    }

    public void OnAttackTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") || (other.gameObject.CompareTag("Shield")) || (other.gameObject.CompareTag("Drone")))
        {
            isAttacking = false;
        }
    }
    #endregion

    #region system
    public void DropSkills()
    {
        GameManager.instance.ProcessDropItemList(droppedSkills, dropSkillChance);
    }

    public void AddRank(int rank)
    {
        health += rank * 10;
        attack += rank * 2;
        attack_interval -= rank * 0.2f;
        attack_interval = Math.Clamp(attack_interval, 0.3f, Single.MaxValue);
        speed += rank * 0.1f;
        transform.localScale += transform.localScale * (rank *  0.2f);
    }
    #endregion
}
