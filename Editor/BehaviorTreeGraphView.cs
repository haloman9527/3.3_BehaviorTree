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
using CZToolKit.Core;
using CZToolKit.GraphProcessor;
using CZToolKit.GraphProcessor.Editors;
using System;
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree.Editors
{
    public class BehaviorTreeGraphView : BaseGraphView
    {
        public BehaviorTreeGraphView(BaseGraph graph, BaseGraphWindow window, CommandDispatcher commandDispacter) : base(graph, window, commandDispacter)
        {
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            schedule.Execute( UpdateState).Every(100);
        }

        private void UpdateState()
        {
            foreach (var node in NodeViews.Values)
            {
                if (node is TaskNodeView taskView)
                {
                    taskView.UpdateState();
                }
            }
        }

        protected override IEnumerable<Type> GetNodeTypes()
        {
            foreach (var type in Util_Reflection.GetChildTypes<Task>())
            {
                if (!type.IsAbstract)
                    yield return type;
            }
        }
    }
}
