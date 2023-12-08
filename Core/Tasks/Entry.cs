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
using CZToolKit.VM;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeMenu("Entry", hidden = true)]
    [TaskIcon("BehaviorTree/Icons/Entry")]
    [NodeTitleColor(0, 0.7f, 0)]
    [NodeTooltip("入口节点，不可移动，不可删除，自动生成")]
    public class Entry : Task { }

    [ViewModel(typeof(Entry))]
    public class EntryVM : ContainerTaskVM
    {
        public EntryVM(Entry model) : base(model)
        {
            AddPort(new BasePortVM(TaskVM.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Single, typeof(TaskVM)));
        }

        private TaskVM Child
        {
            get { return Ports[TaskVM.ChildrenPortName].Connections[0].ToNode as TaskVM; }
        }

        protected override void DoStart()
        {
            Child.Start();
        }

        protected override void DoStop()
        {
            Child.Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            SelfStop(result);
        }
    }
}
