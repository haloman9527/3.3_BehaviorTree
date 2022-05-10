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

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Parallel")]
    [NodeMenuItem("Compsite", "并行执行")]
    [NodeTooltip("依次执行所有，若全部Success，则返回Success，否则按照Task的状态返回(Running > Failure)")]
    public class Parallel : Compsite
    {
        int index;

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
        }

        protected override TaskStatus OnUpdate()
        {
            var status = TaskStatus.Success;
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Running)
                {
                    status = TaskStatus.Running;
                }
                if (tmpStatus != TaskStatus.Running)
                {
                    status = tmpStatus;
                    index++;
                }
            }
            return status;
        }
    }
}
