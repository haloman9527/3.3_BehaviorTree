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
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [NodeMenuItem("Action", "Wait")]
    public class Wait : ActionTask
    {
        public float interval;
    }

    [ViewModel(typeof(Wait))]
    public class WaitVM : ActionTaskVM
    {

        float startTime;

        public WaitVM(BaseNode model) : base(model)
        {
            var t_model = Model as Wait;
            this[nameof(Wait.interval)] = new BindableProperty<float>(() => t_model.interval, v => t_model.interval = v);
        }

        protected override void OnStart()
        {
            base.OnStart();
            startTime = Time.time;
        }

        protected override TaskStatus OnUpdate()
        {
            var t_model = Model as Wait;
            if (Time.time - startTime < t_model.interval)
            {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}
