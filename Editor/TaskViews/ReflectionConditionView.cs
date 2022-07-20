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
    [CustomView(typeof(ReflectionCondition))]
    public class ReflectionConditionView : TaskNodeView { }

    [CustomObjectEditor(typeof(ReflectionConditionView))]
    public class ReflectionConditionInspector : BaseNodeInspector
    {
        public override void OnInspectorGUI()
        {
            var view = Target as ReflectionConditionView;
            var node = view.T_ViewModel as ReflectionConditionVM;
            if (node == null)
                return;

            EditorGUILayoutExtension.BeginHorizontalBoxGroup();
            EditorGUILayoutExtension.BeginVerticalBoxGroup();
            GUILayout.Box("LeftValue", GUILayout.ExpandWidth(true));
            node.LeftValueType = (ReflectionCondition.ValueType)EditorGUILayout.EnumPopup(node.LeftValueType);
            switch (node.LeftValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    node.LeftValue = EditorGUILayout.Toggle(node.LeftValue);
                    break;
                case ReflectionCondition.ValueType.Function:
                    node.LeftFunctionName = EditorGUILayout.TextField(node.LeftFunctionName);
                    break;
            }
            EditorGUILayoutExtension.EndVerticalBoxGroup();
            GUILayout.Label("=", EditorStyles.boldLabel, GUILayout.ExpandHeight(true), GUILayout.Width(12));
            EditorGUILayoutExtension.BeginVerticalBoxGroup();
            GUILayout.Box("RightValue", GUILayout.ExpandWidth(true));
            node.RightValueType = (ReflectionCondition.ValueType)EditorGUILayout.EnumPopup(node.RightValueType);
            switch (node.RightValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    node.RightValue = EditorGUILayout.Toggle(node.RightValue);
                    break;
                case ReflectionCondition.ValueType.Function:
                    node.RightFunctionName = EditorGUILayout.TextField(node.RightFunctionName);
                    break;
            }
            EditorGUILayoutExtension.EndVerticalBoxGroup();
            EditorGUILayoutExtension.EndHorizontalBoxGroup();
        }
    }
}
