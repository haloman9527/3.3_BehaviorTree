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
    [NodeTitle("并行顺序")]
    [NodeTooltip("同时执行所有行为，直到所有行为完成，若全部成功，则返回成功，否则返回失败")]
    [NodeMenu("Composite/Parallel Sequence")]
    public class ParallelSequence : Task
    {
    }

    [ViewModel(typeof(ParallelSequence))]
    public class ParallelSequenceVM : CompositeTaskVM
    {
        private int index;
        private int childrenCount = 0;
        private int runningCount = 0;
        private int succeededCount = 0;
        private int failedCount = 0;
        private bool successState;

        public ParallelSequenceVM(ParallelSequence model) : base(model)
        {
        }

        protected override void DoStart()
        {
            succeededCount = 0;
            failedCount = 0;
            runningCount = 0;
            childrenCount = Children.Count;
            if (childrenCount == 0)
            {
                SelfStop(true);
                return;
            }

            foreach (var child in Children)
            {
                runningCount++;
                child.Start();
            }
        }

        protected override void DoStop()
        {
            if (childrenCount != 0)
            {
                foreach (var child in Children)
                {
                    child.Stop();
                }
            }
            else
                SelfStop(false);
        }

        protected override void OnChildStopped(TaskVM child, bool childSuccess)
        {
            runningCount--;
            if (childSuccess)
                succeededCount++;
            else
                failedCount++;
            if (succeededCount + failedCount == childrenCount)
                SelfStop(failedCount == 0);
        }
    }
}