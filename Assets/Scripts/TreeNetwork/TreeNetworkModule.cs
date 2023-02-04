using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNetworkModule
{
    public TreeNode root;

    public List<List<TreeNode>> Layers;

    public TreeNetworkModule()
    {
        root = new TreeNode(null);
    
        //Initialize Top Layer and first default layer
        Layers = new List<List<TreeNode>>();

        List<TreeNode> topRootLayer = new List<TreeNode>();
        topRootLayer.Add(root);
        Layers.Add(topRootLayer); 

        TreeNode bulletNode = new TreeNode(null);
        AddNodeToLayer(1, bulletNode);
    }

    //Layer starts at 0
    public void AddNodeToLayer(int layer, TreeNode newNode)
    {
        if(layer > Layers.Count)
        {
            Debug.LogError("Layer add Incorrect");
            return;
        }

        if(layer == Layers.Count)
        {
            //add new layer
            List<TreeNode> newLayer = new List<TreeNode>();
            newLayer.Add(newNode);
            Layers.Add(newLayer);
        }
        else
        {
            Layers[layer].Add(newNode);
        }

        ConnectNodeToParent(layer, newNode);
        ConnectNodeToChildren(layer, newNode);
    }

    void ConnectNodeToParent(int layer, TreeNode newNode)
    {
        if(layer - 1 < 0)
        {
            Debug.LogError("Index incorrect");
            return;
        }

        foreach(TreeNode node in Layers[layer - 1])
        {
            node.AddChildren(newNode);
        }
    }

    void ConnectNodeToChildren(int layer, TreeNode newNode)
    {
        if (layer + 1 >= Layers.Count)
        {
            Debug.Log("No Child Layer: " + layer);
            return;
        }

        foreach (TreeNode node in Layers[layer + 1])
        {
            newNode.AddChildren(node);
        }
    }
}
