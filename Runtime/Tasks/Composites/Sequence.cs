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
    [TaskIcon("BehaviorTree/Icons/Sequence")]
    [NodeTitle("顺序执行")]
    [NodeTooltip("顺序执行，遇Failure停止，并返回Failure，否则返回Success")]
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
            else if (++currentIndex < Children.Count)
                Restart();
            else
                Stopped(true);
        }

        private void Restart()
        {
            Children[currentIndex].Start();
        }
    }
}