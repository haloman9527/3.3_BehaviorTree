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
    [TaskIcon("BehaviorTree/Icons/Selector")]
    [NodeTitle("选择执行")]
    [NodeTooltip("依次执行，直到Success或Running，并返回该状态")]
    [NodeMenu("Composite", "Selector")]
    public class Selector : Composite { }

    [ViewModel(typeof(Selector))]
    public class SelectorVM : CompositeVM
    {
        int index;

        public SelectorVM(Selector model) : base(model) { }

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
                var tmpStatus = task.Update();
                if (tmpStatus == TaskResult.Success)
                {
                    return TaskResult.Success;
                }
                if (tmpStatus == TaskResult.Running)
                {
                    return TaskResult.Running;
                }
                index++;
            }
            return TaskResult.Failure;
        }
    }
}
