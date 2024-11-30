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

using Moyo;
using Moyo.GraphProcessor;
using UnityEngine;

namespace Moyo.BehaviorTree
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

        public float Interval
        {
            get => (Model as Wait).interval;
            set => SetFieldValue(ref (Model as Wait).interval, value, nameof(Wait.interval));
        }

        public WaitProcessor(Wait model) : base(model)
        {
            
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