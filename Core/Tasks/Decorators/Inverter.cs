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
    [NodeTooltip("始终返回相反的结果")]
    [NodeMenu("Decorator/Inverter")]
    public class Inverter : Task { }

    [ViewModel(typeof(Inverter))]
    public class InverterProcessor : DecoratorTaskProcessor
    {
        public InverterProcessor(Inverter model) : base(model) { }

        protected override void DoStart()
        {
            Child.Start();
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            SelfStop(!result);
        }
    }
}
