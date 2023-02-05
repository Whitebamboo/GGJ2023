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
    private Coroutine hideTextCoroutine;

    private Rigidbody rigidbody;
    private Coroutine attackCoroutine;
    public bool isAttacking = false;
    public List<Debuff> debuffs = new List<Debuff>();
    public float damageIncreaseRate = 0;
    public float speedDecreaseRate = 0;

    public GameObject attack_target = null;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    /// <summary>
    /// function to make damage to enemy
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage, bool isCritical)
    {
        var dmg = damage * (1 + damageIncreaseRate / 100);

        health -= dmg;
        GameManager.instance.dmgTextManager.AddDmgText(dmg, isCritical ? DmgType.EnemyCritical : DmgType.EnemyNormal, transform.position);
        print("on hit:" + health);//call UI utils function
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(0.5f);
        text.SetActive(false);
    }

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
    
}
