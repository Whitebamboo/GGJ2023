using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : ElementExecute
{
    public Thunder()
    {
        num = 1;
        RestraintElement = new List<ElementsType>();
        RestraintElement.Add(ElementsType.Fire);
        RestraintElement.Add(ElementsType.Water);
    }

}
