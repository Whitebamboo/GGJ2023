using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : ElementExecute
{
    /// <summary>
    /// initial constructor
    /// </summary>
    public Water()
    {
        num = 1;
        RestraintElement = new List<ElementsType>();
        RestraintElement.Add(ElementsType.Fire);
    }
}
