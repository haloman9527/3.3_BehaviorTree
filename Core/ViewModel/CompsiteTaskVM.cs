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
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using CZToolKit.GraphProcessor;
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract class CompositeTaskVM : ContainerTaskVM
    {

        protected CompositeTaskVM(Task model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ParentPortName, BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(ContainerTaskVM)));
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(TaskVM)));
        }
    }
}