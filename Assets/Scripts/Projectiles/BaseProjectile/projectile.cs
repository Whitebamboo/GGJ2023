using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    
    //base attributes
    public float attack_base = 5;
    public float attack_add = 0;
    public float attack_multiply = 1;
    public List<ElementsType> elements_list = new List<ElementsType>();
    public float size = 1f;
    public bool isPenetrate = false;
    public int penetrate_times = 0; 
    public float speed = 10;
    public Vector3 move_direction = Vector3.zero;
    private bool canMove = false;
    public GameObject explode_Object;
    //a dictionary to restore elements restraint relationship, first item is the element, second will be the elemnts make double damage
    private static Dictionary<ElementsType, ElementsType> element_restraint = new Dictionary<ElementsType, ElementsType>() {
        {ElementsType.Fire,  ElementsType.Water },
        {ElementsType.Water, ElementsType.Wood},
        {ElementsType.Wood, ElementsType.Fire } 
    };

    public List<GameObject> element_particle_list = new List<GameObject>();
    public Transform element_particle_parent;
    public List<GameObject> onhit_particle_list = new List<GameObject>();

    private ProjcetilePool projcetilePool;
    private void Start()
    {
        if(projcetilePool == null)
            projcetilePool = FindObjectOfType<ProjcetilePool>();
     
    }

    private void OnEnable()
    {
        foreach (var element in element_particle_list)
        {
            element.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            this.transform.position += move_direction.normalized * speed * Time.deltaTime;
        }
        if(transform.localPosition.magnitude > 1000)
        {
            Dead();
        }
    }

    public void AddElementParticle(ElementsType e_type)
    {
        switch (e_type)
        {
            case ElementsType.Fire:
                element_particle_list[0].SetActive(true);
                break;
            case ElementsType.Water:
                element_particle_list[1].SetActive(true);
                break;
            case ElementsType.Wood:
                element_particle_list[2].SetActive(true);
                break;
        }
    }

    /// <summary>
    /// create projectile and 
    /// </summary>
    /// <param name="p_info"></param>
    public void SetProjectileParameters(ProjectileCreateInfo p_info)
    {
        attack_add = p_info.attack_add;
        attack_multiply = p_info.attack_multiply;
        isPenetrate = p_info.isPenetrate;
        penetrate_times = p_info.penetrate;
        elements_list = p_info.elements_list;
        size = p_info.size;
        //change size fuct TODO

    }

    /// <summary>
    /// start move
    /// </summary>
    /// <param name="dir"></param>
    public void StartMove(Vector3 dir)
    {
        //rotate to move dir
        move_direction = dir;
        //Debug.DrawLine(this.transform.position, this.transform.position + dir, Color.red,2f);
        transform.rotation = Quaternion.FromToRotation(transform.right, move_direction);
        canMove = true;
        EventBus.Broadcast(EventTypes.ProjectileBattleCry);
    }

    #region hit enemy behavior
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            //1.make damage
            MakeDamage(other.gameObject);
            //2.call all on hit effect
            CallAllOnHit(other.gameObject);
            //3.destroy itself
            CallDeadOnHit();
        }
    }

    /// <summary>
    /// explosion will call this
    /// </summary>
    /// <param name="enemy"></param>
    public void MakeExplodeDamage(GameObject enemy)
    {
        if (enemy)
        {
            MakeDamage(enemy);
            CallAllOnHit(enemy);
        }
        
    }

    /// <summary>
    /// calculate and make damage
    /// </summary>
    /// <param name="enemy"></param>
    private void MakeDamage(GameObject enemy)
    {
        Enemy e = enemy.GetComponentInParent<Enemy>();
        DmgType damge_type = DmgType.EnemyNormal;
        float damage = (attack_base + attack_add) * attack_multiply;
        if ((elements_list.Count > 0) && (elements_list.Contains(element_restraint[e.GetElement()])))// have a restraint elements
        {
            print("find retraint");
            damage *= 2;
        }
        else if ((elements_list.Count > 0))
        {
            foreach(ElementsType element_type in elements_list)
            {
                if(e.element == element_restraint[element_type])
                {
                    damage /= 2;
                    break;
                }
            }
        }
        if(damage > (attack_base + attack_add))
        {
            if(damage>((attack_base + attack_add) * attack_multiply))
            {
                damge_type = DmgType.EnemyRestraint;
            }
            else
            {
                damge_type = DmgType.EnemyCritical;

            }
            
        }
        else if(damage < (attack_base + attack_add))
        {
            damge_type = DmgType.EnemyWeak;
        }
        e.TakeDamage(damage, damge_type);
    }

    /// <summary>
    /// call the onhit effect attach to this object
    /// </summary>
    /// <param name="enemy"></param>
    private void CallAllOnHit(GameObject enemy)
    {
        List<onHitEffect> onhit_list = new List<onHitEffect>(GetComponents<onHitEffect>());
        if (onhit_list.Count == 0)
        { 
            return; 
        }
        foreach(onHitEffect onhitComponent in onhit_list)
        {
            onhitComponent.OnHitBehavior(enemy);//an onhit behavior will be override later
        }
    }


    /// <summary>
    /// only call this on hit
    /// </summary>
    private void CallDeadOnHit()
    {
        if(isPenetrate && penetrate_times > 0)
        {
            penetrate_times -= 1;
            return;
        }
        Dead();
    }


    /// <summary>
    /// the bullet destroy it self in all situations
    /// </summary>
    private void Dead()
    {
        //Destroy(this.gameObject);
        projcetilePool.PoolDead(this.gameObject);
       
    }
    #endregion
}
