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

using CZToolKit;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeTitle("并行选择")]
    [NodeTooltip("同时开始所有行为，直到所有行为完成，如果有一个行为返回Success，则返回Success，否则返回Failure")]
    [NodeMenu("Composite/Parallel Selector")]
    public class ParallelSelector : Task
    {
    }

    [ViewModel(typeof(ParallelSelector))]
    public class ParallelSelectorProcessor : CompositeTaskProcessor
    {
        private int failedCount = 0;

        public ParallelSelectorProcessor(ParallelSelector model) : base(model)
        {
        }

        protected override void DoStart()
        {
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
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            if (result)
            {
                SelfStop(true);
            }
            else
            {
                failedCount++;
                if (failedCount == Children.Count)
                    SelfStop(false);
            }
        }
    }
}