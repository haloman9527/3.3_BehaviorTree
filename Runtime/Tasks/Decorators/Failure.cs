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
    [NodeMenu("Decorator/Success")]
    public class Failure : Decorator { }

    [ViewModel(typeof(Failure))]
    public class FailureVM : DecoratorVM
    {
        public FailureVM(Failure model) : base(model) { }

        protected override TaskResult OnUpdate()
        {
            Children.Update();
            return TaskResult.Failure;
        }
    }
}
