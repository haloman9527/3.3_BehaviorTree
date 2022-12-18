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

using CZToolKit.GraphProcessor;
using System;

namespace CZToolKit.BehaviorTree
{
    public abstract class TaskVM : BaseNodeVM
    {
        #region Keyword

        public const string ParentPortName = "Parent";
        public const string ChildrenPortName = "Children";

        #endregion

        #region Fields

        private ContainerTaskVM parent;
        private TaskState currentState;

        #endregion

        #region Properties

        public TaskState CurrentState
        {
            get { return currentState; }
        }

        public ContainerTaskVM Parent
        {
            get { return parent; }
        }

        #endregion

        protected TaskVM(Task model) : base(model)
        {
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            currentState = TaskState.InActive;
        }

        public void Initialize()
        {
            if (Ports.TryGetValue(ParentPortName, out var parentPort) && parentPort.Connections.Count > 0)
                parent = parentPort.Connections[0].FromNode as ContainerTaskVM;
            DoInitialized();
        }

        public void Start()
        {
            currentState = TaskState.Active;
            DoStart();
            if (CurrentState == TaskState.Active && this is IUpdateTask updateTask)
                (Owner as BehaviorTreeVM).RegisterUpdateTask(updateTask);
        }

        public void Stop()
        {
            if (currentState == TaskState.Active)
            {
                currentState = TaskState.StopRequested;
                DoStop();
            }
        }

        protected void Stopped(bool success)
        {
            currentState = TaskState.InActive;
            OnStopped();
            if (this.Parent != null)
                this.Parent.ChildStopped(this, success);
        }

        protected virtual void DoInitialized()
        {
        }

        protected virtual void DoStart()
        {
        }

        protected virtual void DoStop()
        {
        }

        protected virtual void OnStopped()
        {
        }
    }
}