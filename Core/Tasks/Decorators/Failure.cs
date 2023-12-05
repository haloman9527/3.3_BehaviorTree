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
    [NodeTooltip("始终返回Failure")]
    [NodeMenu("Decorator/Failure")]
    public class Failure : Task { }

    [ViewModel(typeof(Failure))]
    public class FailureVM : DecoratorTaskVM
    {
        public FailureVM(Failure model) : base(model) { }

        protected override void DoStart()
        {
            Child.Start();
        }

        protected override void DoStop()
        {
            Child.Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool succeeded)
        {
            Stopped(false);
        }
    }
}
