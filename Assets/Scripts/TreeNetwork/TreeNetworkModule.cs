using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        PrintTree();
        List<TreeNodeChain> chainList = new List<TreeNodeChain>();
        BuildNodeChain(chainList, root);
        Debug.Log(chainList.Count);
        Debug.Log(chainList[0].GetWeight());
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

    void PrintTree()
    {
        foreach(List<TreeNode> treeNodes in Layers)
        {
            string text = "";

            foreach(TreeNode node in treeNodes)
            {
                if(node.skillConfig == null)
                {
                    text += "RootNode";
                    continue;
                }

                text += node.skillConfig.skilltype + " | ";
            }

            Debug.Log(text);
        }
    }

    List<TreeNodeChain> BuildNodeChain(List<TreeNodeChain> chainList, TreeNode node)
    {
        if(node.Children.Count == 0)
        {
            TreeNodeChain chain = new TreeNodeChain();
            chain.treeNodeList.Add(node);
            chainList.Add(chain);
            return chainList;
        }

        for(int i = 0; i < node.Children.Count; i++)
        {
            TreeNode child = node.Children[i];
            BuildNodeChain(chainList, child);
        }

        //foreach(TreeNode child in node.Children)
        //{
        //    BuildNodeChain(chainList, child);
        //}

        foreach(TreeNodeChain chain in chainList)
        {
            chain.treeNodeList.Add(node);
        }

        return chainList;
    }
}
