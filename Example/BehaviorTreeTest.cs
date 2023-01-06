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
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
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

    public Object GraphAsset
    {
        get { return behaviorTreeAsset; }
    }

    public BaseGraphVM Graph
    {
        get { return behaviorTree; }
    }

    private void Start()
    {
        behaviorTree = new BehaviorTreeVM(behaviorTreeAsset.DeserializeTGraph());
        behaviorTree.Blackboard.Set("Owner", this);
        behaviorTree.Start();
    }

    private void Update()
    {
        behaviorTree.Update();
    }
}