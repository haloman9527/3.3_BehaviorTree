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
using CZToolKit.Core.Editors;
using CZToolKit.GraphProcessor.Editors;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomNodeView(typeof(ReflectionCondition))]
    public class ReflectionConditionView : TaskNodeView { }

    [CustomObjectEditor(typeof(ReflectionConditionView))]
    public class ReflectionConditionInspector : BaseNodeInspector
    {
        public override void OnInspectorGUI()
        {
            var view = Target as ReflectionConditionView;
            var node = view.Model as ReflectionCondition;

            EditorGUILayoutExtension.BeginVerticalBoxGroup();
            node.LeftValueType = (ReflectionCondition.ValueType)EditorGUILayout.EnumPopup(node.LeftValueType);
            switch (node.LeftValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    node.LeftValue = EditorGUILayout.Toggle("LeftValue", node.LeftValue);
                    break;
                case ReflectionCondition.ValueType.Function:
                    node.LeftFunctionName = EditorGUILayout.TextField("LeftValue", node.LeftFunctionName);
                    break;
            }
            EditorGUILayoutExtension.EndVerticalBoxGroup();

            node.RightValueType = (ReflectionCondition.ValueType)EditorGUILayout.EnumPopup(node.RightValueType);
            switch (node.RightValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    node.RightValue = EditorGUILayout.Toggle("RightValue", node.RightValue);
                    break;
                case ReflectionCondition.ValueType.Function:
                    node.RightFunctionName = EditorGUILayout.TextField("RightValue", node.RightFunctionName);
                    break;
            }
        }
    }
}
