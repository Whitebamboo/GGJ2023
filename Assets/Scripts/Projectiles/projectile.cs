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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region hit enemy behavior
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {

        }
    }

    #endregion
}
