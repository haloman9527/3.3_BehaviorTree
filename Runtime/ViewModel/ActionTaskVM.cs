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
    public abstract partial class ActionTaskVM : TaskVM
    {
        protected ActionTaskVM(BaseNode model) : base(model)
        {
            AddPort(new BasePortVM("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
        }
    }
}