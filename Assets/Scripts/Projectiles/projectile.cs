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
    public bool canDestroy = true;
    public float speed = 10;

    public Vector3 move_direction = Vector3.zero;
    private bool canMove = false; 

    //a dictionary to restore elements restraint relationship, first item is the element, second will be the elemnts make double damage
    private static Dictionary<ElementsType, ElementsType> element_restraint = new Dictionary<ElementsType, ElementsType>() {
        {ElementsType.Fire,  ElementsType.Water },
        {ElementsType.Water, ElementsType.Wood},
        {ElementsType.Wood, ElementsType.Fire } 
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            this.transform.position += move_direction.normalized * speed * Time.deltaTime;
        }
    }

    public void StartMove(Vector3 dir)
    {
        //rotate to move dir
        move_direction = dir;
        Debug.DrawLine(this.transform.position, this.transform.position + dir, Color.red,2f);
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
    /// calculate and make damage
    /// </summary>
    /// <param name="enemy"></param>
    private void MakeDamage(GameObject enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        float damage = (attack_base + attack_add) * attack_multiply;
        if ((elements_list.Count > 0) && (elements_list.Contains(element_restraint[e.GetElement()])))// have a restraint elements
        {
            damage *= 2;
        }
        e.TakeDamage(damage);
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
        if (canDestroy)
        {
            Dead();
        }
    }


    /// <summary>
    /// the bullet destroy it self in all situations
    /// </summary>
    private void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion
}
