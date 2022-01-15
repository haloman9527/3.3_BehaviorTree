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
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using UnityObject = UnityEngine.Object;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomGraphWindow(typeof(BehaviorTree))]
    public class BehaviorTreeGraphWindow : BaseGraphWindow
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            titleContent = new GUIContent("Behavior Tree");
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnSelectionChange()
        {
            if (Selection.activeGameObject == null)
                return;
            var agent = Selection.activeGameObject.GetComponent<BehaviorTreeAgent>();
            if (agent != null && (agent != (UnityObject)GraphOwner || GraphAsset != agent.GraphAsset || Graph != agent.Graph))
            {
                if (agent.GraphAsset != null)
                {
                    Load(agent as IGraphAssetOwner);
                }
                else
                {
                    Load(agent as IGraphOwner);
                }
            }
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                    Reload();
                    break;
                default:
                    break;
            }
        }

        protected override BaseGraphView NewGraphView(BaseGraph graph, CommandDispatcher commandDispatcher)
        {
            return new BehaviorTreeGraphView(graph, this, commandDispatcher);
        }

        protected override void BuildToolbar(ToolbarView toolbar)
        {
            base.BuildToolbar(toolbar);
            ToolbarButton btnSave = new ToolbarButton();
            btnSave.text = "Save";
            btnSave.clicked += Save;
            toolbar.AddButtonToRight(btnSave);
        }

        protected override void KeyDownCallback(KeyDownEvent evt)
        {
            base.KeyDownCallback(evt);
            if (evt.commandKey || evt.ctrlKey)
            {
                switch (evt.keyCode)
                {
                    case KeyCode.S:
                        Save();
                        break;
                }
            }
        }

        void Save()
        {
            if (GraphAsset is IGraphAsset graphAsset)
            {
                graphAsset.SaveGraph(Graph);
            }
            if (GraphOwner is IGraphOwner graphOwner)
            {
                graphOwner.SaveVariables();
            }
            GraphView.SetDirty(true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            GraphView.UnsetDirty();
        }
    }
}
