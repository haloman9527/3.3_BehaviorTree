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
    [NodeMenu("Composite/Sequence")]
    public class Sequence : Composite { }

    [ViewModel(typeof(Sequence))]
    public class SequenceVM : CompositeVM
    {
        int index;

        public SequenceVM(Sequence model) : base(model) { }

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
        }

        protected override TaskResult OnUpdate()
        {
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var result = task.Update();
                if (result == TaskResult.Failure)
                {
                    return TaskResult.Failure;
                }
                if (result == TaskResult.Running)
                {
                    return TaskResult.Running;
                }
                index++;
            }
            return TaskResult.Success;
        }
    }
}