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
    public abstract partial class ActionTask : Task
    {
        protected override void OnEnabled()
        {
            base.OnEnabled();
            AddPort(new BasePort("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
        }
    }
}