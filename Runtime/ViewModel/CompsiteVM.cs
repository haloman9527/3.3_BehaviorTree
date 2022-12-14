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
    public abstract class CompositeVM : TaskVM
    {
        protected List<TaskVM> tasks;

        protected CompositeVM(Composite model) : base(model)
        {
            base.OnEnabled();
            AddPort(new BasePortVM(TaskVM.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(Task)));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Ports["Children"].onSorted += Refresh;
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (tasks == null)
                tasks = new List<TaskVM>(GetChildren());
        }

        private  IEnumerable<TaskVM> GetChildren()
        {
            foreach (var node in GetConnections(TaskVM.ChildrenPortName))
            {
                if (node is TaskVM task)
                    yield return task;
            }
        }

        private void Refresh()
        {
            if (tasks != null)
            {
                tasks.Clear();
                tasks.AddRange(GetChildren());
            }
        }
    }
}