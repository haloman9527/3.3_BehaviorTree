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
    [NodeMenu("Composite/Parallel")]
    public class Parallel : Task
    {
    }

    [ViewModel(typeof(Parallel))]
    public class ParallelVM : CompositeTaskVM
    {
        private int index;
        private int childrenCount = 0;
        private int succeededCount = 0;
        private int failedCount = 0;
        private bool successState;

        public ParallelVM(Parallel model) : base(model)
        {
        }

        protected override void DoStart()
        {
            succeededCount = 0;
            failedCount = 0;
            childrenCount = Children.Count;
            if (childrenCount == 0)
            {
                Stopped(true);
                return;
            }

            foreach (var child in Children)
            {
                child.Start();
            }
        }

        protected override void DoStop()
        {
            foreach (var child in Children)
            {
                child.Stop();
            }
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (result)
                succeededCount++;
            else
                failedCount++;
            successState = failedCount == 0;
            if (succeededCount + failedCount == childrenCount)
            {
                if (successState)
                    Stopped(true);
                else
                    Stopped(false);
            }
        }
    }
}