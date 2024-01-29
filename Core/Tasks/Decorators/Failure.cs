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
    [NodeTooltip("始终返回Failure")]
    [NodeMenu("Decorator/Failure")]
    public class Failure : Task { }

    [ViewModel(typeof(Failure))]
    public class FailureProcessor : DecoratorTaskProcessor
    {
        public FailureProcessor(Failure model) : base(model) { }

        protected override void DoStart()
        {
            Child.Start();
        }

        protected override void OnChildStopped(TaskProcessor child, bool succeeded)
        {
            SelfStop(false);
        }
    }
}
