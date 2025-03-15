#region 注 释

/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using Atom.BehaviorTree;
using Atom.GraphProcessor;
using UnityEngine;

public class BehaviorTreeTest : MonoBehaviour, IGraphAssetOwner
{
    public BehaviorTreeAsset behaviorTreeAsset;

    private BehaviorTreeProcessor behaviorTree;

    public IGraphAsset GraphAsset
    {
        get { return behaviorTreeAsset; }
    }

    public BaseGraphProcessor Graph
    {
        get
        {
            if (behaviorTree == null)
                behaviorTree = new BehaviorTreeProcessor(behaviorTreeAsset.LoadGraph() as BehaviorTree);
            return behaviorTree;
        }
    }

    public BehaviorTreeProcessor BehaviorTree
    {
        get
        {
            if (behaviorTree == null)
                behaviorTree = new BehaviorTreeProcessor(behaviorTreeAsset.LoadGraph() as BehaviorTree);
            return behaviorTree;
        }
    }

    private void Start()
    {
        BehaviorTree.Blackboard.Set("Owner", this);
        BehaviorTree.Start();
    }

    private void Update()
    {
        behaviorTree.Update();
    }
}