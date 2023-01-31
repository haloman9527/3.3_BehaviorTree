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
using CZToolKit.GraphProcessor.Editors;
using System;
using System.Collections.Generic;
using CZToolKit.GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CZToolKit.BehaviorTree.Editors
{
    public class BehaviorTreeGraphView : BaseGraphView
    {
        public BehaviorTreeGraphView(BaseGraphVM graph, BaseGraphWindow window, CommandDispatcher commandDispatcher) : base(graph, window, commandDispatcher)
        {
        }
        
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

        protected override void NodeCreationRequest(NodeCreationContext c)
        {
            var multiLayereEntryCount = 0;
            var entries = new List<NodeEntry>(16);
            foreach (var nodeType in GetNodeTypes())
            {
                var path = nodeType.Name;
                var menu = (string[])null;
                var hidden = false;
                var menuAttribute = GraphProcessorEditorUtil.GetNodeMenu(nodeType);
                if (menuAttribute != null)
                {
                    path = menuAttribute.path;
                    menu = menuAttribute.menu;
                    hidden = menuAttribute.hidden;
                }
                else
                {
                    menu = new string[] { nodeType.Name };
                }

                if (menu.Length > 1)
                    multiLayereEntryCount++;
                entries.Add(new NodeEntry(nodeType, path, menu, hidden));
            }

            entries.QuickSort((a, b) => -(a.menu.Length.CompareTo(b.menu.Length)));
            entries.QuickSort(0, multiLayereEntryCount - 1, (a, b) => String.Compare(a.path, b.path, StringComparison.Ordinal));
            entries.QuickSort(multiLayereEntryCount, entries.Count - 1, (a, b) => String.Compare(a.path, b.path, StringComparison.Ordinal));

            var nodeMenu = ScriptableObject.CreateInstance<NodeMenuWindow>();
            nodeMenu.Initialize("Nodes", this, entries);
            SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), nodeMenu);
        }

        private IEnumerable<Type> GetNodeTypes()
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
