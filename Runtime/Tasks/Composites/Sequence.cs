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

using CZToolKit.Common.ViewModel;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Sequence")]
    [NodeTitle("顺序执行")]
    [NodeTooltip("依次执行，直到某一行为失败，则返回失败，若所有行为都成功，则返回成功")]
    [NodeMenu("Composite/Sequence")]
    public class Sequence : Task
    {
    }

    [ViewModel(typeof(Sequence))]
    public class SequenceVM : CompositeTaskVM
    {
        private int currentIndex;

        public SequenceVM(Sequence model) : base(model)
        {
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
            {
                Stopped(true);
                return;
            }

            currentIndex = 0;
            Children[currentIndex].Start();
        }

        protected override void DoStop()
        {
            Children[currentIndex].Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (!result)
                Stopped(false);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                Stopped(true);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}