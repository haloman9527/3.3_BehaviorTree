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
using CZToolKit.Core.ViewModel;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeMenu("Root", hidden = true)]
    [TaskIcon("BehaviorTree/Icons/Entry")]
    [NodeTitleColor(0, 0.7f, 0)]
    [NodeTooltip("入口节点，不可移动，不可删除，自动生成")]
    public class Entry : Task { }

    [ViewModel(typeof(Entry))]
    public class EntryVM : TaskVM
    {
        public EntryVM(Entry model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(Task)));
        }

        protected override TaskResult OnUpdate()
        {
            foreach (var node in GetConnections(TaskVM.ChildrenPortName))
            {
                return (node as TaskVM).Update();
            }
            return TaskResult.Success;
        }
    }
}
