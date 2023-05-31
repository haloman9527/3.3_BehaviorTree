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
using CZToolKit.Common;
using CZToolKit.Common.Collection;
using CZToolKit.GraphProcessor;
using CZToolKit.GraphProcessor.Editors;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CZToolKit.BehaviorTree.Editors
{
    public class BehaviorTreeGraphView : BaseGraphView
    {
        public BehaviorTreeGraphView(BaseGraphVM graph, BaseGraphWindow window, CommandDispatcher commandDispatcher) : base(graph, window, commandDispatcher)
        {
        }

        protected override void OnCreated()
        {
            base.OnCreated();
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

        protected override void BuildNodeMenu(NodeMenuWindow nodeMenu)
        {
            foreach (var nodeType in GetNodeTypes())
            {
                if (nodeType.IsAbstract) 
                    continue;
                var nodeStaticInfo = GraphProcessorUtil.NodeStaticInfos[nodeType];
                if (nodeStaticInfo.hidden)
                    continue;
                
                var path = nodeStaticInfo.path;
                var menu = nodeStaticInfo.menu;
                nodeMenu.entries.Add(new NodeMenuWindow.NodeEntry(path, menu, nodeType));
            }
        }

        private IEnumerable<Type> GetNodeTypes()
        {
            foreach (var type in Util_TypeCache.GetTypesDerivedFrom<Task>())
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
