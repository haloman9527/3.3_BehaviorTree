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
    [NodeTitle("并行顺序")]
    [NodeTooltip("同时执行所有行为，直到所有行为完成，若全部成功，则返回成功，否则返回失败")]
    [NodeMenu("Composite/Parallel Sequence")]
    public class ParallelSequence : Task
    {
    }

    [ViewModel(typeof(ParallelSequence))]
    public class ParallelSequenceProcessor : CompositeTaskProcessor
    {
        private int successedCount = 0;
        private int failedCount = 0;

        public ParallelSequenceProcessor(ParallelSequence model) : base(model)
        {
        }

        protected override void DoStart()
        {
            successedCount = 0;
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

        protected override void OnChildStopped(TaskProcessor child, bool childSuccess)
        {
            if (childSuccess)
                successedCount++;
            else
                failedCount++;
            if (successedCount + failedCount >= Children.Count)
                SelfStop(failedCount == 0);
        }
    }
}