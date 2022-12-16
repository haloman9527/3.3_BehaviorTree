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
    [NodeTitle("并行选择")]
    [NodeTooltip("依次执行所有，若有Success或Running，则返回该状态")]
    [NodeMenu("Composite/Parallel Selector")]
    public class ParallelSelector : Composite { }

    [ViewModel(typeof(ParallelSelector))]
    public class ParallelSelectorVM : CompositeVM
    {
        public ParallelSelectorVM(ParallelSelector model) : base(model) { }

        protected override TaskResult OnUpdate()
        {
            var status = TaskResult.Failure;
            foreach (var child in GetConnections("Children"))
            {
                var task = child as TaskVM;
                var tmpStatus = task.Update();
                if (tmpStatus == TaskResult.Success)
                {
                    status = TaskResult.Success;
                }
                if (status != TaskResult.Success && tmpStatus == TaskResult.Running)
                {
                    status = TaskResult.Running;
                }
            }
            return status;
        }
    }
}
