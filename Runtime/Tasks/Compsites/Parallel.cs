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
    [NodeTooltip("依次执行所有，若全部Success，则返回Success，否则按照Task的状态返回(Failure > Running)")]
    public class Parallel : Compsite
    {
        protected override TaskStatus OnUpdate()
        {
            var status = TaskStatus.Success;
            foreach (var child in GetConnections("Children"))
            {
                var task = child as Task;
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Failure)
                {
                    status = TaskStatus.Failure;
                }
                if (tmpStatus == TaskStatus.Running && status != TaskStatus.Failure)
                {
                    status = TaskStatus.Running;
                }
            }
            return status;
        }
    }
}
