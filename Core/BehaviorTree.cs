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

using CZToolKit;
using CZToolKit.GraphProcessor;
using System.Collections.Generic;
using System.Linq;

namespace CZToolKit.BehaviorTree
{
    public class BehaviorTree : BaseGraph
    {
        public int entryID;
    }

    [ViewModel(typeof(BehaviorTree))]
    public class BehaviorTreeProcessor : BaseGraphProcessor
    {
        private EntryProcessor entry;
        private Queue<IUpdateTask> updateTasks;

        public TaskState RootState
        {
            get { return entry.CurrentState; }
        }

        public BehaviorTreeProcessor(BehaviorTree model) : base(model)
        {
            if (model.entryID != 0 && !Nodes.ContainsKey(model.entryID))
                model.entryID = 0;
            if (model.entryID == 0)
                model.entryID = model.nodes.FirstOrDefault(pair => pair.Value is Entry).Key;
            if (model.entryID == 0)
                model.entryID = AddNode<Entry>(InternalVector2Int.zero).ID;
            entry = Nodes[model.entryID] as EntryProcessor;
            updateTasks = new Queue<IUpdateTask>(16);
        }

        public void Start()
        {
            entry.Start();
        }

        public void Update()
        {
            var counter = updateTasks.Count;
            while (counter-- > 0)
            {
                var task = updateTasks.Dequeue();
                if (task.CurrentState == TaskState.Active)
                {
                    task.Update();
                    updateTasks.Enqueue(task);
                }
            }
        }

        public void Stop()
        {
            entry.Stop();
        }

        internal void RegisterUpdateTask(IUpdateTask updateTask)
        {
            updateTasks.Enqueue(updateTask);
        }
    }
}