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
using CZToolKit.GraphProcessor.Editors;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace CZToolKit.BehaviorTree.Editors
{
    public class BehaviorTreeGraphView : BaseGraphView
    {
        protected override void OnInitialized()
        {
            schedule.Execute(UpdateState).Every(100);
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
                if (type.IsAbstract)
                    continue;
                yield return type;
            }
        }

        protected override bool IsCompatible(BasePortView fromPortView, BasePortView toPortView, NodeAdapter nodeAdapter)
        {
            if (fromPortView.node == toPortView.node)
                return false;
            if (fromPortView.ViewModel.Owner == toPortView.ViewModel.Owner)
                return false;
            return base.IsCompatible(fromPortView, toPortView, nodeAdapter);
        }
    }
}
