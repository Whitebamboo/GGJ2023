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
 
        }
    }



    /// <summary>
    /// calculate and make damage
    /// </summary>
    /// <param name="enemy"></param>
    private void MakeDamage(GameObject enemy)
    {
        Enemy e = enemy.GetComponentInParent<Enemy>();
   
        //e.TakeDamage(damage, damge_type);
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
