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

using Moyo;
using Moyo.GraphProcessor;

namespace Moyo.BehaviorTree
{
    [NodeMenu("Entry", hidden = true)]
    [TaskIcon("BehaviorTree/Icons/Entry")]
    [NodeTitleColor(0, 0.7f, 0)]
    [NodeTooltip("入口节点，不可移动，不可删除，自动生成")]
    public class Entry : Task
    {
    }

    [ViewModel(typeof(Entry))]
    public class EntryProcessor : ContainerTaskProcessor
    {
        public EntryProcessor(Entry model) : base(model)
        {
            AddPort(new BasePortProcessor(TaskProcessor.ChildrenPortName, BasePort.Orientation.Vertical, BasePort.Direction.Right, BasePort.Capacity.Single, typeof(TaskProcessor)));
        }

        public TaskProcessor GetFirstChild()
        {
            var port = Ports[TaskProcessor.ChildrenPortName];
            if (port.Connections.Count == 0)
                return null;
            return port.Connections[0].ToNode as TaskProcessor;
        }

        protected override void DoStart()
        {
            var child = GetFirstChild();
            if (child == null)
            {
                SelfStop(true);
            }
            else
            {
                child.Start();
            }
        }

        protected override void DoStop()
        {
            var child = GetFirstChild();
            child?.Stop();
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            SelfStop(result);
        }
    }
}