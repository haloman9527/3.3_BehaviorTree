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
        public event Action<Task> OnTaskExecute;

        private void Start()
        {
            T_Graph.Initialize(this);
        }

        private void Update()
        {
            T_Graph.Update();
        }

        private void OnDrawGizmos()
        {
            if (T_Graph != null)
                T_Graph.OnDrawGizmos();
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