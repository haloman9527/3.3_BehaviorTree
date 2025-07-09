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
    public abstract class CompositeTaskProcessor : ContainerTaskProcessor
    {
        protected CompositeTaskProcessor(Task model) : base(model)
        {
            AddPort(new PortProcessor(TaskProcessor.ParentPortName, BasePort.Direction.Top, BasePort.Capacity.Single, typeof(ContainerTaskProcessor)));
            AddPort(new PortProcessor(TaskProcessor.ChildrenPortName, BasePort.Direction.Bottom, BasePort.Capacity.Multi, typeof(TaskProcessor)));
        }
    }
}