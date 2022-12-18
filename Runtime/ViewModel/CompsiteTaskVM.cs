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
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract class CompositeTaskVM : ContainerTaskVM
    {
        private List<TaskVM> children;

        protected List<TaskVM> Children
        {
            get { return children; }
        }

        protected CompositeTaskVM(Task model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(ContainerTaskVM)));
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(TaskVM)));
        }

        protected override void DoInitialized()
        {
            base.DoInitialized();
            Refresh();
            Ports["Children"].onSorted += Refresh;
        }

        private void Refresh()
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
                    if (node is TaskVM task)
                        yield return task;
                }
            }
        }
    }
}