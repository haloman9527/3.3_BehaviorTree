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
 *  Blog: https://www.mindgear.net/
 *
 */
#endregion
using CZToolKit.VM;
using CZToolKit.GraphProcessor;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Debug")]
    [NodeMenu("Action/Log")]
    public class LogTask : Task
    {
        public string text;
    }
    
    [ViewModel(typeof(LogTask))]
    public class LogTaskVM : ActionTaskVM
    {

        public string Text
        {
            get { return GetPropertyValue<string>(nameof(Text)); }
            set { SetPropertyValue(nameof(Text), value); }
        }
        
        public LogTaskVM(LogTask model) : base(model)
        {
            this[nameof(Text)] = new BindableProperty<string>(() => model.text, v => { model.text = v; });
        }

        protected override void DoStart()
        {
            Debug.Log(Text);
            SelfStop(true);
        }

        protected override void DoStop()
        {
            SelfStop(false);
        }
    }
}
