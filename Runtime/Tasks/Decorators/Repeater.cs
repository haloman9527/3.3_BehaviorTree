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
    [NodeMenuItem("Decorator", "Repeater")]
    [TaskIcon("BehaviorTree/Icons/Repeater")]
    public class Repeater : Decorator
    {
        public int count;
    }

    [ViewModel(typeof(Repeater))]
    public class RepeaterVM : DecoratorVM
    {
        private int counter;

        public RepeaterVM(BaseNode model) : base(model) { }

        protected override void OnStart()
        {
            base.OnStart();
            counter = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            if (counter >= ((Repeater)Model).count)
                return TaskStatus.Success;
            counter++;
            foreach (var task in Children)
            {
                task.Update();
            }
            return TaskStatus.Running;
        }
    }
}
