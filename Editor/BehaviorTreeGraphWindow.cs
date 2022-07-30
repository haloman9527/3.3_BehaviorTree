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
using CZToolKit.GraphProcessor.Editors;
using UnityEditor;
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
            var agent = Selection.activeTransform.GetComponent<IGraphAssetOwner>();
            if (agent == null)
                return;
            if ((UnityObject)agent == (UnityObject)GraphOwner && agent.Graph == GraphOwner.Graph)
                return;
            if (agent.GraphAsset != null)
                ForceLoad(agent);
            else
                ForceLoad(agent as IGraphOwner);
        }

        protected override BaseGraphView NewGraphView(BaseGraphVM graph)
        {
            return new BehaviorTreeGraphView();
        }

        protected override void OnGraphLoaded()
        {
            base.OnGraphLoaded();

            ToolbarButton btnSave = new ToolbarButton();
            btnSave.text = "Save";
            btnSave.clicked += Save;
            btnSave.style.width = 80;
            btnSave.style.unityTextAlign = TextAnchor.MiddleCenter;
            ToolbarRight.Add(btnSave);

            GraphView.RegisterCallback<KeyDownEvent>(KeyDownCallback);
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
                }
            }
        }

        void Save()
        {
            if (GraphAsset is IGraphSerialization graphSerialization)
                graphSerialization.SaveGraph(Graph.Model);
            GraphView.SetDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            GraphView.SetUndirty();
        }
    }
}
