using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : ElementExecute
{


    public int growthLayer;

    public Wood()
    {
        num = 1;
        growthLayer = 1;
        RestraintElement = new List<ElementsType>();
        RestraintElement.Add(ElementsType.Water);
    }
    public override void onDamageExec(GameObject target)
    {
     
        Enemy e = target.GetComponent<Enemy>();
        if (RestraintElement.Contains(e.GetElement()))
        {
            e.finalDamage *= Mathf.Pow(baseAttributeRestraintMultiplier, num * growthLayer);

        }
        //print("wood effect : " + num * growthLayer);
    }
}
