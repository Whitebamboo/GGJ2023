using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class NetworkUI : MonoBehaviour
{
    public Transform LayerRoot;
    public GameObject LayerPrefab;
    public NodeDragUI itemNodePrefab;
    public Transform ItemRoot;
    public TreeLayerPositionData nodePositionData;
    public float topLayerPosition;
    public float layerPositionOffset;
    public Transform LineRoot;
    public Material lineMaterial;
    public LineRenderObj lineObj;
    public GameObject particle;

    public List<GameObject> Layers { get; private set; }
    public List<List<LineRenderObj>> lineRenderObjs = new List<List<LineRenderObj>>();

    private void Awake()
    {
        Layers = new List<GameObject>();
    }

    private void Start()
    {
        //add first two layers
        for (int i = 0; i < transform.childCount; i++)
        {
            Layers.Add(LayerRoot.GetChild(i).gameObject);
        }

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

    public void AddLineRender(int layerIndex)
    {
        if (layerIndex > lineRenderObjs.Count)
        {
            Debug.LogError("Layer add Incorrect");
            return;
        }

        LineRenderObj obj = Instantiate(lineObj, LineRoot);

        if (layerIndex == lineRenderObjs.Count)
        {
            //add new layer
            List<LineRenderObj> newLayer = new List<LineRenderObj>();
            newLayer.Add(obj);
            lineRenderObjs.Add(newLayer);
        }
        else
        {
            lineRenderObjs[layerIndex].Add(obj);
        }       
    }

    public void ProcessLineRender(List<List<TreeNode>> Layers)
    {
        for (int i = 0; i < Layers.Count - 1; i++)
        {
            for (int j = 0; j < Layers[i].Count; j++)
            {
                Transform startNode = nodePositionData.GetPosition(Layers[i].Count, j);
                Vector3 start = new Vector3(startNode.position.x, topLayerPosition - i * layerPositionOffset, startNode.position.z + 0.5f);
                TreeNode node = Layers[i][j];

                for (int k = 0; k < Layers[i + 1].Count; k++)
                {
                    LineRenderObj lineObj = lineRenderObjs[i][k + Layers[i + 1].Count * j];
                    Transform endNode = nodePositionData.GetPosition(Layers[i + 1].Count, k);
                    Vector3 end = new Vector3(endNode.position.x, topLayerPosition - (i + 1) * layerPositionOffset, endNode.position.z + 0.5f);
                    UpdateLineRender(lineObj, start, end, LineRoot, node.Edges[k].Weight);
                }
            }
        }
    }

    public void ProcessTreeNodeChain(TreeNodeChain chain)
    {
        List<TreeNode> nodeList = chain.treeNodeList;
        List<int> nodePosition = new List<int>();

        nodePosition.Add(0);

        for(int i = nodeList.Count - 1; i >= 1; i--)
        {
            TreeNode node = nodeList[i];
            TreeNode nextNode = nodeList[i - 1];
            int index = node.Children.IndexOf(nextNode);
            nodePosition.Add(index);
        }

        for(int j = nodePosition.Count - 1; j >= 0; j--)
        {
            int index = nodePosition[j];

            Vector3 position = nodePositionData.GetPosition(Layers[j].transform.childCount, index).position;
            Vector3 start = new Vector3(position.x, topLayerPosition - j * layerPositionOffset, position.z - 0.1f);
            GameObject obj = Instantiate(particle, start, Quaternion.identity);
            StartCoroutine(DestroyParticle(obj, 1.4f));
        }
    }

    IEnumerator DestroyParticle(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

    void UpdateLineRender(LineRenderObj obj, Vector3 start, Vector3 end, Transform parent, float weight = 1)
    {
        obj.UpdateLine(start, end, weight, parent, lineMaterial);
    }
}
