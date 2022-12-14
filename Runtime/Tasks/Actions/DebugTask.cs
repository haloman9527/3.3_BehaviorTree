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
    [NodeMenu("Action", "Debug")]
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
        public DebugTaskVM(DebugTask model) : base(model)
        {
            this[nameof(Text)] = new BindableProperty<string>(() => model.text, v => { model.text = v; });
        }

        protected override TaskResult OnUpdate()
        {
            Debug.Log(Text);
            return TaskResult.Success;
        }
    }
}
