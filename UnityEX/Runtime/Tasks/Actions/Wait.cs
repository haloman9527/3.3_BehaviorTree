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
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [NodeMenu("Action/Wait")]
    public class Wait : Task
    {
        public float interval;
    }

    [ViewModel(typeof(Wait))]
    public class WaitProcessor : ActionTaskProcessor, IUpdateTask
    {
        float startTime;

        public WaitProcessor(Wait model) : base(model)
        {
            this[nameof(Wait.interval)] = new BindableProperty<float>(() => model.interval, v => model.interval = v);
        }

        protected override void DoStart()
        {
            startTime = Time.time;
        }

        protected override void DoStop()
        {
            SelfStop(false);
        }

        public void Update()
        {
            var t_model = Model as Wait;
            if (Time.time - startTime > t_model.interval)
                SelfStop(true);
        }
    }
}