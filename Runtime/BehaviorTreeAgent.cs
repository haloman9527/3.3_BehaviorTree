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
using CZToolKit.GraphProcessor;
using System;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    public class BehaviorTreeAgent : GraphAssetOwner<BehaviorTreeAsset, BehaviorTreeVM>
    {
        private void Start()
        {
            T_Graph.Initialize(this);
            T_Graph.Start();
        }

        private void Update()
        {
            T_Graph.Update();
        }

#if UNITY_EDITOR
        [ContextMenu("Edit")]
        public void Edit()
        {
            UnityEditor.AssetDatabase.OpenAsset(this);
        }

        public override void SaveGraph(BaseGraph graph)
        {
            if (T_GraphAsset == null)
                return;
            T_GraphAsset.SaveGraph(graph);
        }

        public override BaseGraph DeserializeGraph()
        {
            if (T_GraphAsset == null)
                return null;
            return T_GraphAsset.DeserializeGraph();
        }
#endif
    }
}