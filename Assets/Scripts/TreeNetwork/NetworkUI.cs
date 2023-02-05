using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public Transform LayerRoot;
    public GameObject LayerPrefab;
    public NodeDragUI itemNodePrefab;
    public Transform ItemRoot;

    public List<GameObject> Layers { get; private set; }

    private void Start()
    {
        Layers = new List<GameObject>();
        AddLayer();
    }

    public void AddLayer()
    {
        GameObject obj = Instantiate(LayerPrefab, LayerRoot);
        obj.GetComponent<NetworkLayerUI>().layerIndex = LayerRoot.childCount - 1;
        obj.GetComponent<NetworkLayerUI>().canAdd = true;
        Layers.Add(obj);
    }

    public void AddNewItem(SkillConfig config)
    {
        NodeDragUI node = Instantiate(itemNodePrefab, ItemRoot);
        node.Init(config);
    }
}
