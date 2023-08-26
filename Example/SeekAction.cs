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

using CZToolKit.VM;
using CZToolKit.GraphProcessor;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace CZToolKit.BehaviorTree
{
    [NodeTooltip("追逐敌人到一定距离后停下")]
    [NodeMenu("Seek")]
    public class SeekAction : ActionTask
    {
        public float stopDistance = 2;
        [Header("超时")] [Tooltip("超时将不再追击敌人")] public float timeout = 10;
    }

    [ViewModel(typeof(SeekAction))]
    public class SeekActionVM : ActionTaskVM, IUpdateTask
    {
        public SeekActionVM(SeekAction model) : base(model)
        {
            Title = "追逐";
        }

        [NonSerialized] GameObject target;
        [NonSerialized] NavMeshAgent navMeshAgent;

        private float startTime;

        protected override void DoStart()
        {
            base.DoStart();
            var t_model = Model as SeekAction;
            Owner.Blackboard.TryGet("Target", out target);
            startTime = Time.time;
            navMeshAgent.stoppingDistance = t_model.stopDistance;
            navMeshAgent.updateRotation = true;
            navMeshAgent.isStopped = false;
            Debug.Log("追逐");
        }

        public void Update()
        {
            var t_model = Model as SeekAction;
            var navMeshAgent = Owner.Blackboard.Get<NavMeshAgent>("NavMeshAgent");

            if (target == null || !target.activeSelf || Time.time - startTime > t_model.timeout)
            {
                Owner.Blackboard.Set<GameObject>("Target", null);
                navMeshAgent.isStopped = true;
                Debug.Log("追不上");
                Stopped(false);
            }

            if (Vector3.Distance(navMeshAgent.transform.position, target.transform.position) <= t_model.stopDistance)
            {
                Owner.Blackboard.Set<GameObject>("Target", null);
                navMeshAgent.isStopped = true;
                Stopped(true);
            }

            navMeshAgent.destination = target.transform.position;
        }

        protected override void DoStop()
        {
            Owner.Blackboard.Set<GameObject>("Target", null);
            navMeshAgent.isStopped = true;
        }
    }
}