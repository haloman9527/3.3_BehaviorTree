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
    public partial class Entry : Task { }

    [TaskIcon("BehaviorTree/Icons/Entry")]
    [NodeTitleColor(0, 0.7f, 0)]
    [NodeMenuItem("入口", showInList = false)]
    [NodeTooltip("入口节点，不可移动，不可删除，自动生成")]
    public partial class Entry : Task
    {
        protected override void OnEnabled()
        {
            base.OnEnabled();
            AddPort(new BasePort("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(Task)));
        }

        protected override TaskStatus OnUpdate()
        {
            foreach (var node in GetConnections("Children"))
            {
                return (node as Task).Update();
            }
            return TaskStatus.Success;
        }
    }
}
