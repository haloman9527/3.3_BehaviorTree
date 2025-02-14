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

#if UNITY_EDITOR
using System.Collections.Generic;
using Moyo;
using Moyo.GraphProcessor;
using Moyo.GraphProcessor.Editors;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Group = Moyo.GraphProcessor.Group;
using UnityObject = UnityEngine.Object;

namespace Moyo.BehaviorTree.Editors
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
                    LoadFromGraphAssetOwner(graphAssetOwner);
            }
            else if (agent is IGraphOwner graphOwner)
            {
                if (agent != GraphOwner)
                    LoadFromGraphOwner(graphOwner);
            }
        }

        protected override BaseGraphView NewGraphView()
        {
            return new BehaviorTreeGraphView();
        }

        protected override void OnBtnSaveClick()
        {
            if (GraphAsset == null)
            {
                var path = EditorUtility.SaveFilePanelInProject("保存", "New BehavorTree", "asset", "Create BehaviorTree Asset");
                if (string.IsNullOrEmpty(path))
                    return;
                GraphAsset = ScriptableObject.CreateInstance<BehaviorTreeAsset>();
                AssetDatabase.CreateAsset((ScriptableObject)GraphAsset, path);
            }

            base.OnBtnSaveClick();
        }

        protected override void OnKeyDownCallback(KeyDownEvent evt)
        {
            base.OnKeyDownCallback(evt);
            if (evt.commandKey || evt.ctrlKey)
            {
                switch (evt.keyCode)
                {
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
            List<Group> groups = new List<Group>();
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
                    case GroupView groupView:
                        groups.Add(groupView.ViewModel.Model);
                        break;
                }
            }

            GraphView.CommandDispatcher.BeginGroup();

            var nodesStr = Sirenix.Serialization.SerializationUtility.SerializeValue(nodes, DataFormat.Binary);
            var connectionsStr = Sirenix.Serialization.SerializationUtility.SerializeValue(connections, DataFormat.Binary);
            var groupsStr = Sirenix.Serialization.SerializationUtility.SerializeValue(groups, DataFormat.Binary);

            nodes = Sirenix.Serialization.SerializationUtility.DeserializeValue<Dictionary<int, BaseNode>>(nodesStr, DataFormat.Binary);
            connections = Sirenix.Serialization.SerializationUtility.DeserializeValue<List<BaseConnection>>(connectionsStr, DataFormat.Binary);
            groups = Sirenix.Serialization.SerializationUtility.DeserializeValue<List<Group>>(groupsStr, DataFormat.Binary);

            var graph = GraphView.ViewModel;
            var nodeMaps = new Dictionary<int, BaseNodeProcessor>();

            GraphView.ClearSelection();
            var selectables = new List<ISelectable>(32);

            foreach (var pair in nodes)
            {
                pair.Value.id = graph.NewID();
                pair.Value.position += new InternalVector2Int(50, 50);
                var vm = ViewModelFactory.ProduceViewModel(pair.Value) as BaseNodeProcessor;
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

                var vm = ViewModelFactory.ProduceViewModel(connection) as BaseConnectionProcessor;
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

                group.id = graph.NewID();
                GraphView.CommandDispatcher.Do(new AddGroupCommand(graph, group));
                selectables.Add(GraphView.GroupViews[group.id]);
            }

            GraphView.CommandDispatcher.EndGroup();

            GraphView.AddToSelection(selectables);
        }
    }
}
#endif