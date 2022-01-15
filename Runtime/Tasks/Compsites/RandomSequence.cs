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
using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/RandomSequence")]
    [NodeMenuItem("Compsite", "随机顺序执行")]
    [NodeTooltip("以随机顺序执行行为，遇Failuer或Running中断，并返回该状态")]
    public class RandomSequence : Compsite
    {
        int index;

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
            for (int i = tasks.Count; i > 0; i--)
            {
                var index = Random.Range(0, i);
                tasks.Add(tasks[index]);
                tasks.RemoveAt(index);
            }
        }

        protected override TaskStatus OnUpdate()
        {
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i] as Task;
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Failure)
                {
                    return TaskStatus.Failure;
                }
                if (tmpStatus == TaskStatus.Running)
                {
                    return TaskStatus.Running;
                }
                index++;
            }
            return TaskStatus.Success;
        }
    }
}
