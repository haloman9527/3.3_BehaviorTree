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

using Jiange.GraphProcessor;

namespace Jiange.BehaviorTree
{
    public abstract class ActionTaskProcessor : TaskProcessor
    {
        protected ActionTaskProcessor(Task model) : base(model)
        {
            AddPort(new BasePortProcessor(TaskProcessor.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Left, BasePort.Capacity.Single, typeof(ContainerTaskProcessor)));
        }
    }
}