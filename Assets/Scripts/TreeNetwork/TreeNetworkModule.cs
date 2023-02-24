using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNetworkModule
{
    public TreeNode root;

    public List<List<TreeNode>> Layers;

    public List<TreeNodeChain> chainList;

    public TreeNetworkModule()
    {
        root = new TreeNode(null);
    
        //Initialize Top Layer and first default layer
        Layers = new List<List<TreeNode>>();

        List<TreeNode> topRootLayer = new List<TreeNode>();
        topRootLayer.Add(root);
        Layers.Add(topRootLayer); 
    }

    public TreeNodeChain GetTreeNodeChain()
    {
        WeightedRandomGenerator<TreeNodeChain> generator = new WeightedRandomGenerator<TreeNodeChain>("Random");
        foreach(TreeNodeChain chain in chainList)
        {
            generator.AddEntry(chain, chain.GetWeight());
            Debug.Log("Weight: " + chain.GetWeight());
        }
        return generator.GetRandomEntry();
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
        GameManager.instance.networkUI.ProcessLineRender(GameManager.instance.tree.NetworkModule.Layers);

        chainList = BuildNodeChain(root);
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
            GameManager.instance.networkUI.AddLineRender(layer - 1);
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
            GameManager.instance.networkUI.AddLineRender(layer);
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

                //text += node.skillConfig.skilltype + " | ";
            }

            //Debug.Log(text);
        }
    }

    //List<TreeNodeChain> BuildNodeChain(List<TreeNodeChain> chainList, TreeNode node)
    //{
    //    if(node.Children.Count == 0)
    //    {
    //        TreeNodeChain chain = new TreeNodeChain();
    //        chain.treeNodeList.Add(node);
    //        chainList.Add(chain);
    //        return chainList;
    //    }

    //    for(int i = 0; i < node.Children.Count; i++)
    //    {
    //        TreeNode child = node.Children[i];
    //        BuildNodeChain(chainList, child);
    //    }

    //    foreach(TreeNodeChain chain in chainList)
    //    {
    //        chain.treeNodeList.Add(node);
    //    }

    //    return chainList;
    //}

    List<TreeNodeChain> BuildNodeChain(TreeNode node)
    {
        if (node.Children.Count == 0)
        {
            TreeNodeChain chain = new TreeNodeChain();
            chain.treeNodeList.Add(node);
            List<TreeNodeChain> baseList = new List<TreeNodeChain>();
            baseList.Add(chain);
            return baseList;
        }

        List<TreeNodeChain> returnList = new List<TreeNodeChain>();
        for (int i = 0; i < node.Children.Count; i++)
        {
            TreeNode child = node.Children[i];
            var list = BuildNodeChain(child);
            foreach (TreeNodeChain chain in list)
            {
                chain.treeNodeList.Add(node);
            }
            returnList.AddRange(list);
        }

        //foreach (TreeNodeChain chain in chainList)
        //{
        //    chain.treeNodeList.Add(node);
        //}

        return returnList;
    }
}
