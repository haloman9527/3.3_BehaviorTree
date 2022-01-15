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
    public abstract partial class Decorator : Task
    {
        public IEnumerable<Task> Children
        {
            get
            {
                foreach (var node in GetConnections("Children"))
                {
                    if (node is Task task)
                        yield return task;
                }
            }
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            AddPort(new BasePort("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
            AddPort(new BasePort("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(Task)));
        }
    }
}
