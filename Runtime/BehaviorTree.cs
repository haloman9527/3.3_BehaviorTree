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

using CZToolKit.Core.ViewModel;
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

        public IGraphOwner GraphOwner { get; private set; }


        public BehaviorTreeVM(BaseGraph model) : base(model)
        {
            var t_model = Model as BehaviorTree;
            if (t_model.entryID != 0 && !Nodes.ContainsKey(t_model.entryID))
                t_model.entryID = 0;
            if (t_model.entryID == 0)
                t_model.entryID = model.nodes.FirstOrDefault(pair => pair.Value is Entry).Key;
            if (t_model.entryID == 0)
                t_model.entryID = AddNode<Entry>(InternalVector2Int.zero).ID;
            entry = Nodes[t_model.entryID] as EntryVM;

        }

        public void Initialize(IGraphOwner graphOwner)
        {
            GraphOwner = graphOwner;

            foreach (var node in Nodes.Values)
            {
                if (node is TaskVM task)
                    task.Initialize();
            }
            OnNodeAdded += NodeAdded;
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

        public void RegisterUpdateTask(IUpdateTask updateTask)
        {
            updateTasks.AddLast(updateTask);
        }

        private void NodeAdded(BaseNodeVM node)
        {
            if (!(node is TaskVM task))
                return;
            if (GraphOwner != null)
                task.Initialize();
        }
    }
}