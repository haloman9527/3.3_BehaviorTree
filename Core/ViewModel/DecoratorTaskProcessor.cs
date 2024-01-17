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
    public abstract class DecoratorTaskProcessor : ContainerTaskProcessor
    {
        protected TaskProcessor Child
        {
            get { return Children.Count == 0 ? null : Children[0]; }
        }

        protected DecoratorTaskProcessor(Task model) : base(model)
        {
            AddPort(new BasePortProcessor(TaskProcessor.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(ContainerTaskProcessor)));
            AddPort(new BasePortProcessor(TaskProcessor.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(TaskProcessor)));
        }
    }
}