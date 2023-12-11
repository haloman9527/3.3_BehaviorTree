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

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Repeater")]
    [NodeMenu("Decorator/Repeater")]
    public class Repeater : Task
    {
        public bool ignoreFaild;
        public int loopCount;
    }

    [ViewModel(typeof(Repeater))]
    public class RepeaterVM : DecoratorTaskVM, IUpdateTask
    {
        private Repeater model;
        private int counter;
        private bool childRunning;

        public RepeaterVM(Repeater model) : base(model)
        {
            this.model = model;
        }

        protected override void DoStart()
        {
            if (model.loopCount != 0)
            {
                counter = 0;
                childRunning = true;
                Child.Start();
            }
            else
                SelfStop(true);
        }

        protected override void DoStop()
        {
            if (Child.CurrentState == TaskState.Active)
                Child.Stop();
            else
                SelfStop(false);
        }

        public void Update()
        {
            if (childRunning)
                return;

            childRunning = true;
            Child.Start();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            this.childRunning = false;
            if (model.ignoreFaild ||　result)
            {
                if (model.loopCount > 0 && ++counter >= model.loopCount)
                    SelfStop(true);
            }
            else
                SelfStop(false);
        }
    }
}