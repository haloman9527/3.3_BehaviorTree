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
    [TaskIcon("BehaviorTree/Icons/Parallel")]
    [NodeTitle("并行执行")]
    [NodeTooltip("依次执行所有，若全部Success，则返回Success，否则按照Task的状态返回(Running > Failure)")]
    [NodeMenu("Composite", "Parallel")]
    public class Parallel : Composite { }

    [ViewModel(typeof(Parallel))]
    public class ParallelVM : CompositeVM
    {
        int index;

        public ParallelVM(Parallel model) : base(model) { }

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
        }

        protected override TaskResult OnUpdate()
        {
            var status = TaskResult.Success;
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var tmpStatus = task.Update();
                if (tmpStatus == TaskResult.Running)
                {
                    status = TaskResult.Running;
                }
                if (tmpStatus != TaskResult.Running)
                {
                    status = tmpStatus;
                    index++;
                }
            }
            return status;
        }
    }
}
