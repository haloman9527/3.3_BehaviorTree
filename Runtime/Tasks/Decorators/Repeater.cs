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

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Repeater")]
    [NodeMenu("Decorator/Repeater")]
    public class Repeater : Task
    {
        public int loopCount;
    }

    [ViewModel(typeof(Repeater))]
    public class RepeaterVM : DecoratorTaskVM, IUpdateTask
    {
        private Repeater tModel;
        private int counter;
        private bool childRunning;

        public RepeaterVM(Repeater model) : base(model)
        {
            this.tModel = model;
        }

        protected override void DoStart()
        {
            counter = 0;
            childRunning = false;
        }

        protected override void DoStop()
        {
            Child.Stop();
            if (Child.CurrentState != TaskState.Active)
                Stopped(false);
        }

        public void Update()
        {
            if (childRunning)
                return;

            if (tModel.loopCount < 0 || counter < tModel.loopCount)
            {
                childRunning = true;
                Child.Start();
            }
            else
                Stopped(true);
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (!result)
                Stopped(false);
            this.childRunning = false;
            counter++;
        }
    }
}