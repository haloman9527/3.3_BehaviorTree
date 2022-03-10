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
        float startTime;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            this["Interval"] = new BindableProperty<float>(() => interval, v => interval = v);
        }

        protected override void OnStart()
        {
            base.OnStart();
            startTime = Time.time;
        }

        protected override TaskStatus OnUpdate()
        {
            if (Time.time - startTime < interval)
            {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}
