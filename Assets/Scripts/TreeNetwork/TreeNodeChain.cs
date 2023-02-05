using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreeNodeChain
{
    public List<TreeNode> treeNodeList;
    public List<Edge> edges;

    public TreeNodeChain()
    {
        treeNodeList = new List<TreeNode>();
        edges = new List<Edge>();
    }

    public float GetWeight()
    {
        if(edges.Count == 0)
        {
            buildEdges();
        }

        float total = 0;
        foreach(Edge e in edges)
        {
            total += e.Weight;
        }

        return total;
    }

    void buildEdges()
    {
        for(int i = treeNodeList.Count - 1; i >= 1; i--)
        {
            int index = treeNodeList[i].Children.IndexOf(treeNodeList[i - 1]);
            edges.Add(treeNodeList[i].Edges[index]);
        }
    }


}
