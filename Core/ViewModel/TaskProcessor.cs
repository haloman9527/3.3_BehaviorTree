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

using System;
using Jiange.GraphProcessor;

namespace Jiange.BehaviorTree
{
    public abstract class TaskProcessor : BaseNodeProcessor
    {
        #region Keyword

        public const string ParentPortName = "Parent";
        public const string ChildrenPortName = "Children";

        #endregion

        #region Fields

        private ContainerTaskProcessor parent;
        private TaskState currentState;

        #endregion

        #region Properties

        public event Action OnStart;
        public event Action<bool> OnStop;

        public TaskState CurrentState
        {
            get { return currentState; }
        }

        public ContainerTaskProcessor Parent
        {
            get { return parent; }
        }

        #endregion

        protected TaskProcessor(Task model) : base(model)
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
                (Owner as BehaviorTreeProcessor).RegisterUpdateTask(updateTask);
        }

        public void Stop()
        {
            DoStop();
            currentState = TaskState.InActive;
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
                parent = parentPort.Connections[0].FromNode as ContainerTaskProcessor;
            else
                parent = null;
        }
    }
}