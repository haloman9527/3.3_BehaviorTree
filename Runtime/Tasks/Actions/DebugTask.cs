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
using CZToolKit.Core.SharedVariable;
using CZToolKit.GraphProcessor;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Debug")]
    [NodeMenuItem("Action", "Debug")]
    public class DebugTask : ActionTask
    {
        public string text;
        public SharedGameObject go;

        public string Text
        {
            get { return GetPropertyValue<string>(nameof(Text)); }
            set { SetPropertyValue(nameof(Text), value); }
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            this[nameof(Text)] = new BindableProperty<string>(text, v => { text = v; });
        }

        protected override TaskStatus OnUpdate()
        {
            Debug.Log(Text);
            return TaskStatus.Success;
        }
    }
}
