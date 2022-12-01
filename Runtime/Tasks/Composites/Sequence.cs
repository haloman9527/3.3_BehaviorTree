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
    [TaskIcon("BehaviorTree/Icons/Sequence")]
    [NodeTitle("顺序执行")]
    [NodeTooltip("依次执行，遇Failure或Running中断，并返回该状态")]
    [NodeMenu("Composite", "Sequence")]
    public class Sequence : Compsite { }

    [ViewModel(typeof(Sequence))]
    public class SequenceVM : CompsiteVM
    {
        int index;

        public SequenceVM(BaseNode model) : base(model) { }

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Failure)
                {
                    return TaskStatus.Failure;
                }
                if (tmpStatus == TaskStatus.Running)
                {
                    return TaskStatus.Running;
                }
                index++;
            }
            return TaskStatus.Success;
        }
    }
}