using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : ElementExecute
{

    public Wood()
    {
        num = 1;
        RestraintElement = new List<ElementsType>();
        RestraintElement.Add(ElementsType.Water);
    }
}
