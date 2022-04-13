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
using System.Linq;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    public partial class BehaviorTree : BaseGraph
    {
        [SerializeField] string entryGUID;
    }

    public partial class BehaviorTree
    {
        [NonSerialized] private TaskStatus status = TaskStatus.Running;
        [NonSerialized] private Entry entry;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            if (!string.IsNullOrEmpty(entryGUID) && !Nodes.ContainsKey(entryGUID))
                entryGUID = string.Empty;
            if (string.IsNullOrEmpty(entryGUID))
                entryGUID = Nodes.Values.FirstOrDefault(node => node is Entry)?.GUID;
            if (string.IsNullOrEmpty(entryGUID))
                entryGUID = AddNode<Entry>(Vector2.zero).GUID;
            entry = Nodes[entryGUID] as Entry;
        }

        public TaskStatus Update()
        {
            if (entry.Status == TaskStatus.Running)
                status = entry.Update();
            return status;
        }

        public void OnDrawGizmos()
        {
            foreach (var node in Nodes.Values)
            {
                if (node is Task task)
                    task.OnDrawGizmos();
            }
        }
    }
}