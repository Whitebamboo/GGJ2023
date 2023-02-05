using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public GameObject go;
    public List<GameObject> alive_pool = new List<GameObject>();
    public List<GameObject> dead_pool = new List<GameObject>(); 
    
    /// <summary>
    /// use transform
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public GameObject PoolCreate(GameObject obj, Transform t)
    {
        if (dead_pool.Count > 0)
        {
            go = dead_pool[0];
            dead_pool.RemoveAt(0);
            alive_pool.Add(go);

            go.SetActive(true);
            go.transform.SetParent(t);
            PoolInitial(go);
            return go;
        }
        go = Instantiate(obj, t);
        alive_pool.Add(go);
        PoolInitial(go);
        return go;
    }


    /// <summary>
    /// use vector
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public GameObject PoolCreate(GameObject obj, Vector3 v)
    {
        if (dead_pool.Count > 0)
        {
            go = dead_pool[0];
            dead_pool.RemoveAt(0);
            alive_pool.Add(go);

            go.SetActive(true);
           
            PoolInitial(go);
            return go;
        }
        go = Instantiate(obj, v,Quaternion.identity);
        alive_pool.Add(go);
        PoolInitial(go);
        return go;
    }


    public virtual void PoolInitial(GameObject go)
    {

    }

    public void PoolDead(GameObject go)
    {
        if (alive_pool.Contains(go))
        {
            alive_pool.Remove(go);
        }
        dead_pool.Add(go);
        go.SetActive(false);
    }
}
