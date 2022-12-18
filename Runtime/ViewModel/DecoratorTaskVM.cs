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

namespace CZToolKit.BehaviorTree
{
    public abstract class DecoratorTaskVM : ContainerTaskVM
    {
        protected DecoratorTaskVM(Task model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(ContainerTaskVM)));
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(TaskVM)));
        }

        public TaskVM Child
        {
            get { return Ports[TaskVM.ChildrenPortName].Connections[0].ToNode as TaskVM; }
        }
    }
}