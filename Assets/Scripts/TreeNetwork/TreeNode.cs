using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public SkillConfig skillConfig { get; private set; }
    public List<TreeNode> Children { get; private set; }
    public List<Edge> Edges { get; private set; }

    public TreeNode(SkillConfig skillConfig)
    {
        this.skillConfig = skillConfig;

        Children = new List<TreeNode>();
        Edges = new List<Edge>();  
    }

    public void AddChildren(TreeNode node)
    {
        Children.Add(node);
        Edges.Add(new Edge());
    }
}
