using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLayerPositionData : MonoBehaviour
{
    public GameObject[] positionData;

    public Transform GetPosition(int childCount, int childPosition)
    {
        return positionData[childCount - 1].transform.GetChild(childPosition);
    }
}
