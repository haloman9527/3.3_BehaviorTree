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
        #region Define

        public enum State
        {
            InActive,
            Active,
        }

        #endregion
        
        #region Keyword
        
        public const string ParentPortName = "Parent";
        public const string ChildrenPortName = "Children";
        
        #endregion

        #region Fields

        [NonSerialized] private BehaviorTreeAgent agent;
        [NonSerialized] private State currentState;
        [NonSerialized] private TaskResult status = TaskResult.Running;
        public event Action onUpdate;

        #endregion

        #region Properties

        protected BehaviorTreeAgent Agent
        {
            get { return agent; }
        }

        public State CurrentState
        {
            get { return currentState; }
        }

        public TaskResult Status
        {
            get { return status; }
        }

        #endregion

        protected TaskVM(Task model) : base(model)
        {
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            currentState = State.InActive;
            status = TaskResult.Running;
        }

        public void Initialize()
        {
            agent = (Owner as BehaviorTreeVM).GraphOwner as BehaviorTreeAgent;
            OnInitialized();
        }


        private void Start()
        {
            currentState = State.Active;
            OnStart();
        }

        public TaskResult Update()
        {
            if (currentState == State.InActive)
                Start();

            status = OnUpdate();

            if (Status == TaskResult.Failure || Status == TaskResult.Success)
                End();

            onUpdate?.Invoke();
            return Status;
        }

        private void End()
        {
            if (currentState != State.Active)
                return;
            if (Ports.ContainsKey("Children"))
            {
                foreach (var connection in GetConnections("Children"))
                {
                    if (connection is TaskVM task)
                        task.End();
                }
            }

            OnEnd();
            currentState = State.InActive;
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual TaskResult OnUpdate()
        {
            return TaskResult.Success;
        }

        protected virtual void OnEnd()
        {
        }

        public virtual void OnDrawGizmos()
        {
        }
    }
}