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
    public float attackCoolDown = 0;
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
    public List<Debuff> debuffs = new List<Debuff>();
    public float damageIncreaseRate = 0;
    public float speedDecreaseRate = 0;

    public GameObject attack_target = null;

    [SerializeField] private float dropSkillChance = 0.3f;


    public List<GameObject> enemy_debuff_Effect_List = new List<GameObject>();
    public Dictionary<ElementsType, GameObject> enemy_had_effect = new Dictionary<ElementsType, GameObject>();


    public float baseDamage = 0;//each on hit has a base damage
    public float finalDamage = 0;//the final damage it take's base on a series of execute
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        GetComponentInChildren<HPBar>().InitialHP(health);
    }




    /// <summary>
    /// add a debuff to that enemy
    /// </summary>
    /// <param name="debuff"></param>
    public void AddDebuff(ElementsType elementType, float value)
    {
        int i = debuffs.FindIndex(debuff => debuff.elementType == elementType);
        if (i == -1)
        {
            var newDebuff = DebuffCreator.instance.Create(elementType);
            newDebuff.value = value;
            debuffs.Add(newDebuff);
            newDebuff.OnApply(this);
        }
        else
        {
            // renew debuff
            debuffs[i].OnRemove(this);
            debuffs[i].value += value;
            debuffs[i].OnApply(this);
            debuffs[i].times = debuffs[i].configTimes;
        }
    }

    /// <summary>
    /// function to get enemy element type
    /// </summary>
    /// <returns></returns>
    public ElementsType GetElement()
    {
        return element;
    }



    public void CreateEnemyDebuffEffect(ElementsType eType)
    {
        if (enemy_had_effect.ContainsKey(eType))
        {
            if (enemy_had_effect[eType] != null)
            {
                return;
            }
        }
        //create element effect
        GameObject go = null;
        switch (eType)
        {
            case ElementsType.Fire:
                go = Instantiate(enemy_debuff_Effect_List[0], this.transform);
                break;
            case ElementsType.Water:
                go = Instantiate(enemy_debuff_Effect_List[1], this.transform);
                break;
            case ElementsType.Wood:
                go = Instantiate(enemy_debuff_Effect_List[2], this.transform);
                break;

        }
        if (go)
        {
            if (enemy_had_effect.ContainsKey(eType))
            {
                enemy_had_effect[eType] = go;
            }
            else
            {
                enemy_had_effect.Add(eType, go);
            }
        }

    }

    public void RemoveEnemyDebuffEffect(ElementsType eType)
    {
        if (enemy_had_effect.ContainsKey(eType) && enemy_had_effect[eType])
        {
            GameObject go = enemy_had_effect[eType];
            Destroy(go);
            enemy_had_effect[eType] = null;
        }
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
    public void OnAttackTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") || (other.gameObject.CompareTag("Shield")))
        {
            isAttacking = true;
            rigidbody.velocity = new Vector3();
            attack_target = other.gameObject;
        }
    
    }

    public void OnAttackTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") || (other.gameObject.CompareTag("Shield")))
        {
            isAttacking = false;
        }
    }
    
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
}
