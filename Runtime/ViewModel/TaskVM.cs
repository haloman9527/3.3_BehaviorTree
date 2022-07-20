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
        #region Fields
        [NonSerialized] private BehaviorTreeAgent agent;
        [NonSerialized] private bool started;
        [NonSerialized] private TaskStatus status = TaskStatus.Running;
        public event Action onUpdate;
        #endregion

        #region Properties
        protected BehaviorTreeAgent Agent { get { return agent; } }
        public bool Started { get { return started; } }
        public TaskStatus Status { get { return status; } }
        #endregion

        protected TaskVM(BaseNode model) : base(model) { }

        protected override void OnEnabled()
        {
            base.OnEnabled();
            status = TaskStatus.Running;
        }

        public void Initialize()
        {
            agent = (Owner as BehaviorTreeVM).GraphOwner as BehaviorTreeAgent;
            OnInitialized();
        }


        private void Start()
        {
            started = true;
            OnStart();
        }

        public TaskStatus Update()
        {
            if (!Started)
                Start();

            status = OnUpdate();

            if (Status == TaskStatus.Failure || Status == TaskStatus.Success)
                End();

            onUpdate?.Invoke();
            return Status;
        }

        private void End()
        {
            if (Ports.ContainsKey("Children"))
            {
                foreach (var connection in GetConnections("Children"))
                {
                    if (connection is TaskVM task && task.started)
                        task.End();
                }
            }
            OnEnd();
            started = false;
        }

        protected virtual void OnInitialized() { }

        protected virtual void OnStart() { }

        protected virtual TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        protected virtual void OnEnd() { }

        public virtual void OnDrawGizmos() { }
    }
}
