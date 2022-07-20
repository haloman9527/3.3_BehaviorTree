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
    [NodeMenuItem("Root", showInList = false)]
    [TaskIcon("BehaviorTree/Icons/Entry")]
    [NodeTitleColor(0, 0.7f, 0)]
    [NodeTooltip("入口节点，不可移动，不可删除，自动生成")]
    public class Entry : Task { }

    [ViewModel(typeof(Entry))]
    public class EntryVM : TaskVM
    {
        public EntryVM(BaseNode model) : base(model)
        {
            AddPort(new BasePortVM("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(Task)));
        }

        protected override TaskStatus OnUpdate()
        {
            foreach (var node in GetConnections("Children"))
            {
                return (node as TaskVM).Update();
            }
            return TaskStatus.Success;
        }
    }
}
