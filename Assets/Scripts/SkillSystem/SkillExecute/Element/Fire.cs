using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : ElementExecute
{
    public Fire()
    {
        num = 1;
        RestraintElement = new List<ElementsType>();
        RestraintElement.Add(ElementsType.Wood);
    }
}
