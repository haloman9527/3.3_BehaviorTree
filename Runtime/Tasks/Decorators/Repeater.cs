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

using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [NodeMenuItem("Decorator", "Repeater")]
    public class Repeater : Decorator { }

    [ViewModel(typeof(Repeater))]
    public class RepeaterVM : DecoratorVM
    {
        public RepeaterVM(BaseNode model) : base(model) { }

        protected override TaskStatus OnUpdate()
        {
            foreach (var task in Children)
            {
                task.Update();
            }
            return TaskStatus.Running;
        }
    }
}
