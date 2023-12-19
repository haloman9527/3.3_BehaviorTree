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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using CZToolKit.VM;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeTitle("并行选择")]
    [NodeTooltip("同时执行所有行为，直到所有行为完成，若全部失败，则返回失败，否则返回成功")]
    [NodeMenu("Composite/Parallel Selector")]
    public class ParallelSelector : Task
    {
    }

    [ViewModel(typeof(ParallelSelector))]
    public class ParallelSelectorVM : CompositeTaskVM
    {
        private int succeededCount = 0;
        private int failedCount = 0;

        public ParallelSelectorVM(ParallelSelector model) : base(model)
        {
        }

        protected override void DoStart()
        {
            succeededCount = 0;
            failedCount = 0;
            if (Children.Count == 0)
            {
                SelfStop(true);
                return;
            }

            foreach (var child in Children)
            {
                child.Start();
            }
        }

        protected override void DoStop()
        {
            if (Children.Count != 0)
            {
                foreach (var child in Children)
                {
                    child.Stop();
                }
            }
            else
                SelfStop(false);
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (result)
                succeededCount++;
            else
                failedCount++;
            if (succeededCount + failedCount == Children.Count)
                SelfStop(succeededCount != 0);
        }
    }
}