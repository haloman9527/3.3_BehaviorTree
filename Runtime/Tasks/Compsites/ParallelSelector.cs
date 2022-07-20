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
    [NodeMenuItem("Compsite", "并行选择")]
    [NodeTooltip("依次执行所有，若有Success或Running，则返回该状态")]
    public class ParallelSelector : Compsite { }

    [ViewModel(typeof(ParallelSelector))]
    public class ParallelSelectorVM : CompsiteVM
    {
        public ParallelSelectorVM(BaseNode model) : base(model) { }

        protected override TaskStatus OnUpdate()
        {
            var status = TaskStatus.Failure;
            foreach (var child in GetConnections("Children"))
            {
                var task = child as TaskVM;
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Success)
                {
                    status = TaskStatus.Success;
                }
                if (status != TaskStatus.Success && tmpStatus == TaskStatus.Running)
                {
                    status = TaskStatus.Running;
                }
            }
            return status;
        }
    }
}
