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

        public event Action OnStart;
        public event Action<bool> OnStop;

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
            if (Ports.TryGetValue(ParentPortName, out var parentPort))
            {
                RefreshParent();
                parentPort.onConnectionChanged += RefreshParent;
            }
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            if (Ports.TryGetValue(ParentPortName, out var parentPort))
            {
                parent = null;
                parentPort.onConnectionChanged -= RefreshParent;
            }
        }

        public void Start()
        {
            currentState = TaskState.Active;
            OnStart?.Invoke();
            DoStart();
            if (CurrentState == TaskState.Active && this is IUpdateTask updateTask)
                (Owner as BehaviorTreeVM).RegisterUpdateTask(updateTask);
        }

        public void Stop()
        {
            currentState = TaskState.StopRequested;
            DoStop();
        }

        protected void SelfStop(bool success)
        {
            currentState = TaskState.InActive;
            OnStop?.Invoke(success);
            Parent?.ChildStopped(this, success);
        }

        protected virtual void DoStart()
        {
        }

        protected virtual void DoStop()
        {
        }

        private void RefreshParent()
        {
            if (Ports.TryGetValue(ParentPortName, out var parentPort) && parentPort.Connections.Count > 0)
                parent = parentPort.Connections[0].FromNode as ContainerTaskVM;
            else
                parent = null;
        }
    }
}