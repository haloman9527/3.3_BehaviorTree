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
    [NodeMenu("Composite/Selector")]
    public class Selector : Task { }

    [ViewModel(typeof(Selector))]
    public class SelectorVM : CompositeTaskVM
    {
        private int currentIndex;

        public SelectorVM(Selector model) : base(model) { }

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
            if (result)
                Stopped(true);
            else if (++currentIndex < Children.Count)
                Restart();
            else
                Stopped(false);
        }

        private void Restart()
        {
            Children[currentIndex].Start();
        }
    }
}
