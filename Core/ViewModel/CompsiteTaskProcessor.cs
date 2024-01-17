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
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract class CompositeTaskProcessor : ContainerTaskProcessor
    {

        protected CompositeTaskProcessor(Task model) : base(model)
        {
            AddPort(new BasePortProcessor(TaskProcessor.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Left, BasePort.Capacity.Single, typeof(ContainerTaskProcessor)));
            AddPort(new BasePortProcessor(TaskProcessor.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Right, BasePort.Capacity.Multi, typeof(TaskProcessor)));
        }
    }
}