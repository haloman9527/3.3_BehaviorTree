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
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using Jiange;
using Jiange.GraphProcessor;

namespace Jiange.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Sequence")]
    [NodeTitle("顺序执行")]
    [NodeTooltip("依次执行，直到某一行为失败，则返回失败，若所有行为都成功，则返回成功")]
    [NodeMenu("Composite/Sequence")]
    public class Sequence : Task
    {
    }

    [ViewModel(typeof(Sequence))]
    public class SequenceProcessor : CompositeTaskProcessor
    {
        private int currentIndex;

        public SequenceProcessor(Sequence model) : base(model)
        {
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
            {
                SelfStop(true);
            }
            else
            {
                currentIndex = 0;
                Children[currentIndex].Start();
            }
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            if (!result)
                SelfStop(false);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                SelfStop(true);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}