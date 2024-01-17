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
 *  Blog: https://www.mindgear.net/
 *
 */

#endregion

using CZToolKit;
using CZToolKit.ECS;
using CZToolKit.GraphProcessor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CZToolKit.BehaviorTree
{
    [NodeMenu("Wander")]
    [NodeTooltip("在指定区域内徘徊，直到看到敌人")]
    public class WanderAction : ActionTask
    {
        [Header("巡逻范围")] public string center;
        public float range = 10;
        [Header("视野范围")] public float radius = 5;
        [Range(0, 360)] [Header("视野角度")] public float sector = 90;
        public LayerMask layer;
    }

    [ViewModel(typeof(WanderAction))]
    public class WanderActionProcessor : ActionTaskProcessor, IUpdateTask
    {
        public WanderActionProcessor(WanderAction model) : base(model)
        {
            Title = "徘徊";
        }

        Vector3 targetPos;
        float stayTime;

        protected override void DoStart()
        {
            var navMeshAgent = Owner.Blackboard.Get<NavMeshAgent>("NavMeshAgent");
            var t_model = Model as WanderAction;

            targetPos = Random.insideUnitSphere * t_model.range + Owner.Blackboard.Get<GameObject>(t_model.center).transform.position;
            targetPos.y = 0;
            stayTime = Random.Range(2, 5);
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.isStopped = false;
            Debug.Log("徘徊");
        }

        public void Update()
        {
            var navMeshAgent = Owner.Blackboard.Get<NavMeshAgent>("NavMeshAgent");
            var world = Owner.Blackboard.Get<World>("World");
            var entity = Owner.Blackboard.Get<Entity>("Entity");
            var t_model = Model as WanderAction;
            if (Vector3.Distance(targetPos, navMeshAgent.transform.position) <= 2)
            {
                stayTime -= Time.deltaTime;
                if (stayTime <= 0)
                {
                    targetPos = Random.insideUnitSphere * t_model.range + Owner.Blackboard.Get<GameObject>(t_model.center).transform.position;
                    targetPos.y = 0;
                    stayTime = Random.Range(2, 5);
                }
            }
            
            navMeshAgent.SetDestination(targetPos);

            Collider[] colliders = Physics.OverlapSphere(navMeshAgent.transform.position, t_model.radius, t_model.layer);
            if (colliders.Length > 0)
            {
                foreach (var item in colliders)
                {
                    if (Vector3.Angle(navMeshAgent.transform.forward, item.transform.position - navMeshAgent.transform.position) <= t_model.sector / 2)
                    {
                        Owner.Blackboard.Set("Target", item.gameObject);
                        SelfStop(true);
                    }
                }
            }
        }
    }
}