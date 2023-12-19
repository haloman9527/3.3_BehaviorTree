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
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    public abstract class DecoratorTaskVM : ContainerTaskVM
    {
        protected TaskVM Child
        {
            get { return Children.Count == 0 ? null : Children[0]; }
        }

        protected DecoratorTaskVM(Task model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(ContainerTaskVM)));
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(TaskVM)));
        }
    }
}