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
    [NodeTooltip("始终返回相反的结果")]
    [NodeMenu("Decorator/Inverter")]
    public class Inverter : Task { }

    [ViewModel(typeof(Inverter))]
    public class InverterVM : DecoratorTaskVM
    {
        public InverterVM(Inverter model) : base(model) { }

        protected override void DoStart()
        {
            Child.Start();
        }

        protected override void DoStop()
        {
            Child.Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            Stopped(!result);
        }
    }
}
