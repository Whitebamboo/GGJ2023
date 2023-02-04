using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] private Enemy parent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        parent.OnAttackTriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        parent.OnAttackTriggerExit(other);
    }
}
