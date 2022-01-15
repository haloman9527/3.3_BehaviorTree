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
        [SerializeField] string _entryGUID;
    }

    public partial class BehaviorTree
    {
        [NonSerialized] private TaskStatus status = TaskStatus.Running;

        public Entry Entry
        {
            get
            {
                BaseNode entry = null;
                if (string.IsNullOrEmpty(_entryGUID))
                {
                    entry = Nodes.Values.FirstOrDefault(item => item is Entry);
                    if (entry == null)
                        entry = AddNode<Entry>(Vector2.zero);
                    _entryGUID = entry.GUID;
                }
                if (entry == null)
                {
                    if (!Nodes.TryGetValue(_entryGUID, out entry) || !(entry is Entry))
                    {
                        entry = Nodes.Values.FirstOrDefault(item => item is Entry);
                    }
                    if (entry == null)
                    {
                        entry = NewNode<Entry>(Vector2.zero);
                        AddNode(entry);
                        _entryGUID = entry.GUID;
                    }
                }
                return entry as Entry;
            }
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            if (Entry == null) { }
        }

        public TaskStatus Update()
        {
            if (Entry.Status == TaskStatus.Running)
                status = Entry.Update();
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