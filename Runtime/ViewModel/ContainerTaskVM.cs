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

using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract class ContainerTaskVM : TaskVM
    {
        private List<TaskVM> children;

        protected List<TaskVM> Children
        {
            get { return children; }
        }

        protected ContainerTaskVM(Task model) : base(model)
        {
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            RefreshChildren();
            Ports[TaskVM.ChildrenPortName].onConnectionChanged += RefreshChildren;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Ports[TaskVM.ChildrenPortName].onConnectionChanged -= RefreshChildren;
        }

        public void ChildStopped(TaskVM child, bool result)
        {
            if (CurrentState != TaskState.Active)
            {
                // Stopped(result);
                return;
            }
            this.OnChildStopped(child, result);
        }

        protected abstract void OnChildStopped(TaskVM child, bool result);
        
        protected virtual void RefreshChildren()
        {
            if (children == null)
                children = new List<TaskVM>(GetChildren());
            else
            {
                children.Clear();
                children.AddRange(GetChildren());
            }

            IEnumerable<TaskVM> GetChildren()
            {
                foreach (var node in GetConnections(TaskVM.ChildrenPortName))
                {
                    yield return (TaskVM)node;
                }
            }
        }
    }
}