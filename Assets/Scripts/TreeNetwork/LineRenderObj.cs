using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineRenderObj : MonoBehaviour
{
    public LineRenderer line;

    public void UpdateLine(Vector3 start, Vector3 end, float weight, Transform parent = null, Material material = null)
    {
        line.transform.parent = parent;
        line.positionCount = 2;
        float emissionIntensity = Mathf.Lerp(-3f, 3f, weight / GameManager.instance.maxWeight);
        line.material = new Material(material);
        line.material.SetColor("_EmissionColor", new Color(2 / 255f, 64 / 255f, 74 / 255f) * emissionIntensity);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        line.SetPositions(new Vector3[] { start, end });
    }
}
