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

using System;
using Atom.GraphProcessor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Atom.BehaviorTree
{
    [CreateAssetMenu]
    public class BehaviorTreeAsset : ScriptableObject, IGraphAsset
    {
        [FormerlySerializedAs("graph")] [SerializeField] private BehaviorTree data;

        public Type GraphType => typeof(BehaviorTree);

        public void SaveGraph(BaseGraph graph) => this.data = (BehaviorTree)graph;

        public BaseGraph LoadGraph() => data;
    }
}