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

using CZToolKit.Common.ViewModel;
using CZToolKit.GraphProcessor;
using CZToolKit.GraphProcessor.Editors;
using System.Collections.Generic;
using CZToolKit.Common;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomView(typeof(BehaviorTree))]
    public class BehaviorTreeGraphWindow : BaseGraphWindow
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            titleContent = new GUIContent("Behavior Tree");
        }

        private void OnSelectionChange()
        {
            if (Selection.activeTransform == null)
                return;
            var agent = Selection.activeTransform.GetComponent<IGraphOwner>();
            if (agent == null)
                return;
            if (agent.Graph == null)
                return;
            if (agent is IGraphAssetOwner graphAssetOwner)
            {
                if (graphAssetOwner.GraphAsset != null && (agent != GraphOwner || graphAssetOwner.GraphAsset != GraphAsset || agent.Graph != GraphOwner.Graph))
                    ForceLoad(graphAssetOwner);
            }
            else if (agent is IGraphOwner graphOwner)
            {
                if (agent != GraphOwner)
                    ForceLoad(graphOwner);
            }
        }

        protected override BaseGraphView NewGraphView(object argument)
        {
            if (Graph == null)
                Graph = new BehaviorTreeVM(new BehaviorTree());
            
            var graphView =  new BehaviorTreeGraphView(Graph, this, new CommandDispatcher());
            graphView.RegisterCallback<KeyDownEvent>(KeyDownCallback);
            return graphView;
        }
        

        protected override void BuildToolBar()
        {
            base.BuildToolBar();
            
            ToolbarButton btnSave = new ToolbarButton();
            btnSave.text = "Save";
            btnSave.clicked += Save;
            btnSave.style.width = 80;
            btnSave.style.unityTextAlign = TextAnchor.MiddleCenter;
            ToolbarRight.Add(btnSave);
        }

        void KeyDownCallback(KeyDownEvent evt)
        {
            if (evt.commandKey || evt.ctrlKey)
            {
                switch (evt.keyCode)
                {
                    case KeyCode.S:
                        Save();
                        evt.StopImmediatePropagation();
                        break;
                    case KeyCode.D:
                        Duplicate();
                        evt.StopImmediatePropagation();
                        break;
                }
            }
        }

        private void Duplicate()
        {
            if (GraphView == null)
                return;
            // 收集所有节点，连线
            Dictionary<int, BaseNode> nodes = new Dictionary<int, BaseNode>();
            List<BaseConnection> connections = new List<BaseConnection>();
            List<BaseGroup> groups = new List<BaseGroup>();
            foreach (var item in GraphView.selection)
            {
                switch (item)
                {
                    case BaseNodeView nodeView:
                        nodes.Add(nodeView.ViewModel.ID, nodeView.ViewModel.Model);
                        break;
                    case BaseConnectionView connectionView:
                        connections.Add(connectionView.ViewModel.Model);
                        break;
                    case BaseGroupView groupView:
                        groups.Add(groupView.ViewModel.Model);
                        break;
                }
            }

            GraphView.CommandDispatcher.BeginGroup();

            var nodesStr = SerializationUtility.SerializeValue(nodes, DataFormat.Binary);
            var connectionsStr = SerializationUtility.SerializeValue(connections, DataFormat.Binary);
            var groupsStr = SerializationUtility.SerializeValue(groups, DataFormat.Binary);

            nodes = SerializationUtility.DeserializeValue<Dictionary<int, BaseNode>>(nodesStr, DataFormat.Binary);
            connections = SerializationUtility.DeserializeValue<List<BaseConnection>>(connectionsStr, DataFormat.Binary);
            groups = SerializationUtility.DeserializeValue<List<BaseGroup>>(groupsStr, DataFormat.Binary);

            var graph = GraphView.ViewModel;
            var nodeMaps = new Dictionary<int, BaseNodeVM>();

            GraphView.ClearSelection();
            var selectables = new List<ISelectable>(32);

            foreach (var pair in nodes)
            {
                pair.Value.id = graph.NewID();
                pair.Value.position += new InternalVector2Int(50, 50);
                var vm = ViewModelFactory.CreateViewModel(pair.Value) as BaseNodeVM;
                GraphView.CommandDispatcher.Do(new AddNodeCommand(graph, vm));
                nodeMaps[pair.Key] = vm;
                selectables.Add(GraphView.NodeViews[vm.ID]);
            }

            foreach (var connection in connections)
            {
                if (nodeMaps.TryGetValue(connection.fromNode, out var from))
                    connection.fromNode = from.ID;

                if (nodeMaps.TryGetValue(connection.toNode, out var to))
                    connection.toNode = to.ID;

                var vm = ViewModelFactory.CreateViewModel(connection) as BaseConnectionVM;
                GraphView.CommandDispatcher.Do(new ConnectCommand(graph, vm));
                selectables.Add(GraphView.ConnectionViews[vm]);
            }

            foreach (var group in groups)
            {
                for (int i = group.nodes.Count - 1; i >= 0; i--)
                {
                    if (nodeMaps.TryGetValue(group.nodes[i], out var node))
                        group.nodes[i] = node.ID;
                    else
                        group.nodes.RemoveAt(i);
                }

                var vm = ViewModelFactory.CreateViewModel(group) as BaseGroupVM;
                GraphView.CommandDispatcher.Do(new AddGroupCommand(graph, vm));
                selectables.Add(GraphView.GroupViews[vm]);
            }

            GraphView.CommandDispatcher.EndGroup();
            
            GraphView.AddToSelection(selectables);
        }


        void Save()
        {
            if (GraphAsset == null)
            {
                var path = EditorUtility.SaveFilePanelInProject("保存", "New BehavorTree", "asset", "Create BehaviorTree Asset");
                if (string.IsNullOrEmpty(path))
                    return;
                GraphAsset = ScriptableObject.CreateInstance<BehaviorTreeAsset>();
                AssetDatabase.CreateAsset(GraphAsset, path);
            }

            if (GraphAsset is IGraphSerialization graphSerialization)
            {
                graphSerialization.SaveGraph(Graph.Model);
            }
            GraphView.SetDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            GraphView.SetUnDirty();
        }
    }
}