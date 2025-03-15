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

using Atom.GraphProcessor;
using Atom;
using UnityEngine;

namespace Atom.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Debug")]
    [NodeMenu("Action/Log")]
    public class LogTask : Task
    {
        public string text;
    }

    [ViewModel(typeof(LogTask))]
    public class LogTaskProcessor : ActionTaskProcessor
    {
        public string Text
        {
            get => this.ModelAs<LogTask>().text;
            set => SetFieldValue(ref this.ModelAs<LogTask>().text, value, nameof(LogTask.text));
        }

        public LogTaskProcessor(LogTask model) : base(model)
        {
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