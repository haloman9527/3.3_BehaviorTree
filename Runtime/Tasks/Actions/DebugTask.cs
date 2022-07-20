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
    [TaskIcon("BehaviorTree/Icons/Debug")]
    [NodeMenuItem("Action", "Debug")]
    public class DebugTask : ActionTask
    {
        public string text;
    }

    [ViewModel(typeof(DebugTask))]
    public class DebugTaskVM : ActionTaskVM
    {

        public string Text
        {
            get { return GetPropertyValue<string>(nameof(Text)); }
            set { SetPropertyValue(nameof(Text), value); }
        }
        public DebugTaskVM(BaseNode model) : base(model)
        {
            var t_model = Model as DebugTask;
            this[nameof(Text)] = new BindableProperty<string>(() => t_model.text, v => { t_model.text = v; });
        }

        protected override TaskStatus OnUpdate()
        {
            Debug.Log(Text);
            return TaskStatus.Success;
        }
    }
}
