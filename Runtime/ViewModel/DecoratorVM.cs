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
    public abstract partial class DecoratorVM : TaskVM
    {
        protected DecoratorVM(BaseNode model) : base(model)
        {
            AddPort(new BasePortVM("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
            AddPort(new BasePortVM("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(Task)));
        }

        public IEnumerable<TaskVM> Children
        {
            get
            {
                foreach (var node in GetConnections("Children"))
                {
                    if (node is TaskVM task)
                        yield return task;
                }
            }
        }

        protected override TaskStatus OnUpdate()
        {
            foreach (var connection in GetConnections("Children"))
            {
                if (connection is TaskVM task)
                    task.Update();
            }
            return base.OnUpdate();
        }
    }
}
