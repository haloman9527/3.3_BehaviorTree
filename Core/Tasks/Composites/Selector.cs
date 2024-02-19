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

using CZToolKit;
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Selector")]
    [NodeTitle("选择执行")]
    [NodeTooltip("依次执行，直到某一行为成功，则返回成功，若所有行为都失败，则返回失败")]
    [NodeMenu("Composite/Selector")]
    public class Selector : Task
    {
    }

    [ViewModel(typeof(Selector))]
    public class SelectorProcessor : CompositeTaskProcessor
    {
        private int currentIndex;

        public SelectorProcessor(Selector model) : base(model)
        {
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
                SelfStop(true);
            else
            {
                currentIndex = 0;
                Children[currentIndex].Start();
            }
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            if (result)
                SelfStop(true);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                SelfStop(false);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}