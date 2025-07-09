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

using System.Collections.Generic;
using Atom.GraphProcessor;

namespace Atom.BehaviorTree
{
    public abstract class ContainerTaskProcessor : TaskProcessor
    {
        private List<TaskProcessor> children;

        protected List<TaskProcessor> Children
        {
            get { return children; }
        }

        protected ContainerTaskProcessor(Task model) : base(model)
        {
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            RefreshChildren(null);
            Ports[TaskProcessor.ChildrenPortName].OnConnected += RefreshChildren;
            Ports[TaskProcessor.ChildrenPortName].onDisconnected += RefreshChildren;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Ports[TaskProcessor.ChildrenPortName].OnConnected -= RefreshChildren;
            Ports[TaskProcessor.ChildrenPortName].onDisconnected -= RefreshChildren;
        }

        protected override void DoStop()
        {
            if (Children != null && Children.Count != 0)
            {
                foreach (var child in Children)
                {
                    child.Stop();
                }
            }
        }

        public void ChildStopped(TaskProcessor child, bool childSuccess)
        {
            this.OnChildStopped(child, childSuccess);
        }

        protected abstract void OnChildStopped(TaskProcessor child, bool childSuccess);

        protected virtual void RefreshChildren(BaseConnectionProcessor connection)
        {
            if (children == null)
                children = new List<TaskProcessor>(GetChildren());
            else
            {
                children.Clear();
                children.AddRange(GetChildren());
            }

            IEnumerable<TaskProcessor> GetChildren()
            {
                foreach (var node in GetPortConnections(TaskProcessor.ChildrenPortName))
                {
                    yield return (TaskProcessor)node;
                }
            }
        }
    }
}