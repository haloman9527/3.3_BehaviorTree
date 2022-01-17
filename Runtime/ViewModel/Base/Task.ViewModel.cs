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
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Default")]
    public abstract partial class Task : BaseNode
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

        protected override void OnEnabled()
        {
            base.OnEnabled();
            status = TaskStatus.Running;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            agent = GraphOwner as BehaviorTreeAgent;
        }

        public TaskStatus Update()
        {
            if (!Started)
            {
                started = true;
                OnStart();
            }

            status = OnUpdate();

            if (Status == TaskStatus.Failure || Status == TaskStatus.Success)
            {
                OnStop();
                started = false;
            }
            onUpdate?.Invoke();
            return Status;
        }

        protected virtual void OnStart() { }

        protected virtual TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        protected virtual void OnStop() { }

        public virtual void OnDrawGizmos() { }
    }
}
