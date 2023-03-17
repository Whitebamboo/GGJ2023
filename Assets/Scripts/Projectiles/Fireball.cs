using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Bullet
{

    private float size = 1;//default size, dont change this value
    public int life_time = 5;
    public float LifeTimeInterval = 1f;
    private float lifetime_timer = 0;
    public float attack_range = 0.5f;//relative to the default size
    public const float default_speed = 1;
    /// <summary>
    /// multi methods
    /// </summary>
    /// <param name="b"></param>
    public void InstantiateInit(projectile p)
    {
        print("create fire ball");
        FireballCreator fbc = p.ability.GetAspect<FireballCreator>();
        if(fbc != null)
        {
            int count = fbc.num * fbc.layer;
            attack = p.attack * count * 0.3f;//30% * X * base_atk
            size = size * fbc.sizeMul / 100;
            transform.localScale = transform.localScale * size;
            attack_range = attack_range * fbc.sizeMul / 100;
            speed = default_speed * fbc.speedMul / 100;
            life_time = fbc.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        lifetime_timer -= Time.deltaTime;
        if(lifetime_timer <= 0)
        {
            Attack();
            life_time -= 1;
            lifetime_timer = LifeTimeInterval;
            if(life_time <= 0)
            {
                Dead();
            }
        }
        
    }


    private void Attack()
    {
        Collider[] cs = Physics.OverlapSphere(this.transform.position, attack_range);
        List<Enemy> attackedEnemies = new List<Enemy>();
        if(cs.Length > 0)
        {
            foreach(var c in cs)
            {
                Enemy e = c.GetComponentInParent<Enemy>();
                if(e && !attackedEnemies.Contains(e))
                {
                    e.TakeDamage(attack, DmgType.EnemyNormal);
                }
            }
        }

        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //do nothing
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.transform.position, attack_range);
    }
}
