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

using CZToolKit.Common.ViewModel;
using CZToolKit.GraphProcessor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    public class BehaviorTree : BaseGraph
    {
        [HideInInspector] public int entryID;
    }

    [ViewModel(typeof(BehaviorTree))]
    public class BehaviorTreeVM : BaseGraphVM
    {
        private EntryVM entry;
        private LinkedList<IUpdateTask> updateTasks = new LinkedList<IUpdateTask>();

        public BehaviorTreeVM(BehaviorTree model) : base(model)
        {
            if (model.entryID != 0 && !Nodes.ContainsKey(model.entryID))
                model.entryID = 0;
            if (model.entryID == 0)
                model.entryID = model.nodes.FirstOrDefault(pair => pair.Value is Entry).Key;
            if (model.entryID == 0)
                model.entryID = AddNode<Entry>(InternalVector2Int.zero).ID;
            entry = Nodes[model.entryID] as EntryVM;
        }

        public void Start()
        {
            entry.Start();
        }

        public void Update()
        {
            var t = updateTasks.First;
            while (t != null)
            {
                if (t.Value.CurrentState != TaskState.Active)
                {
                    updateTasks.Remove(t);
                }
                else
                {
                    t.Value.Update();
                }
                
                t = t.Next;
            }
        }

        public void Stop()
        {
            entry.Stop();
        }

        internal void RegisterUpdateTask(IUpdateTask updateTask)
        {
            updateTasks.AddLast(updateTask);
        }
    }
}