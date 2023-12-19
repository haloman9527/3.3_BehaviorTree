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
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using CZToolKit.BehaviorTree;
using CZToolKit.GraphProcessor;
using UnityEngine;

public class BehaviorTreeTest : MonoBehaviour, IGraphAssetOwner
{
    public BehaviorTreeAsset behaviorTreeAsset;

    private BehaviorTreeVM behaviorTree;

    public IGraphAsset GraphAsset
    {
        get { return behaviorTreeAsset; }
    }

    public BaseGraphVM Graph
    {
        get
        {
            if (behaviorTree == null)
                behaviorTree = new BehaviorTreeVM(behaviorTreeAsset.DeserializeTGraph());
            return behaviorTree;
        }
    }

    public BehaviorTreeVM BehaviorTree
    {
        get
        {
            if (behaviorTree == null)
                behaviorTree = new BehaviorTreeVM(behaviorTreeAsset.DeserializeTGraph());
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