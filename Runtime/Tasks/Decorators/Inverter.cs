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
    [NodeMenu("Decorator/Inverter")]
    public class Inverter : Decorator { }

    [ViewModel(typeof(Inverter))]
    public class InverterVM : DecoratorVM
    {
        public InverterVM(Success model) : base(model) { }

        protected override TaskResult OnUpdate()
        {
            switch (Children.Update())
            {
                case TaskResult.Failure:
                    return TaskResult.Success;
                case TaskResult.Success:
                    return TaskResult.Failure;
                default:
                    return TaskResult.Running;
            }
        }
    }
}
