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

using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using Moyo.GraphProcessor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Moyo.BehaviorTree
{
    [CreateAssetMenu]
    public class BehaviorTreeAsset : ScriptableObject, IGraphAsset, IGraphAsset<BehaviorTree>
    {
        [HideInInspector] [SerializeField] byte[] serializedGraph;
        [HideInInspector] [SerializeField] List<UnityObject> graphUnityReferences = new List<UnityObject>();

        public Type GraphType => typeof(BehaviorTree);

        public UnityObject UnityAsset => this;

        public void SaveGraph(BaseGraph graph)
        {
            serializedGraph = SerializationUtility.SerializeValue(graph, DataFormat.JSON, out graphUnityReferences);
        }

        public BaseGraph DeserializeGraph()
        {
            return DeserializeTGraph();
        }

        public BehaviorTree DeserializeTGraph()
        {
            BehaviorTree graph = null;
            if (serializedGraph != null && serializedGraph.Length > 0)
                graph = SerializationUtility.DeserializeValue<BehaviorTree>(serializedGraph, DataFormat.JSON, graphUnityReferences);
            if (graph == null)
                graph = new BehaviorTree();
            return graph;
        }
    }
}