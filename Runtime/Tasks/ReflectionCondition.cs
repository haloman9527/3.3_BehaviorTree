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
using System;
using System.Reflection;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [Serializable]
    [NodeMenuItem("Condition", "条件方法")]
    [NodeTooltip("从Agent中查找条件方法，做成委托")]
    public class ReflectionCondition : ActionTask
    {
        public enum ValueType
        {
            Value, Function
        }

        [SerializeField] ValueType leftValueType;
        [SerializeField] bool leftValue;
        [SerializeField] string leftFunctionName;
        [NonSerialized] Func<bool> leftFunction;

        [SerializeField] ValueType rightValueType;
        [SerializeField] bool rightValue;
        [SerializeField] string rightFunctionName;
        [NonSerialized] Func<bool> rightFunction;

        public ValueType LeftValueType
        {
            get { return GetPropertyValue<ValueType>(nameof(leftValueType)); }
            set { SetPropertyValue(nameof(leftValueType), value); }
        }
        public bool LeftValue
        {
            get { return leftValue; }
            set { leftValue = value; }
        }
        public string LeftFunctionName
        {
            get { return GetPropertyValue<string>(nameof(leftFunctionName)); }
            set { SetPropertyValue(nameof(leftFunctionName), value); }
        }

        public ValueType RightValueType
        {
            get { return GetPropertyValue<ValueType>(nameof(rightValueType)); }
            set { SetPropertyValue(nameof(rightValueType), value); }
        }
        public bool RightValue
        {
            get { return rightValue; }
            set { rightValue = value; }
        }
        public string RightFunctionName
        {
            get { return GetPropertyValue<string>(nameof(rightFunctionName)); }
            set { SetPropertyValue(nameof(rightFunctionName), value); }
        }

        protected override void OnEnabled()
        {
            base.OnEnabled();

            this[nameof(leftValueType)] = new BindableProperty<ValueType>(leftValueType, v => leftValueType = v);
            this[nameof(leftValue)] = new BindableProperty<bool>(leftValue, v => { leftValue = v; });
            this[nameof(leftFunctionName)] = new BindableProperty<string>(leftFunctionName, v => leftFunctionName = v);

            this[nameof(rightValueType)] = new BindableProperty<ValueType>(rightValueType, v => rightValueType = v);
            this[nameof(rightValue)] = new BindableProperty<bool>(rightValue, v => { rightValue = v; });
            this[nameof(rightFunctionName)] = new BindableProperty<string>(rightFunctionName, v => rightFunctionName = v);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Refresh();

            this[nameof(leftValueType)].AsBindableProperty<ValueType>().RegesterValueChangedEvent(_ => { Refresh(); });
            this[nameof(leftValue)].AsBindableProperty<bool>().RegesterValueChangedEvent(_ => { Refresh(); });
            this[nameof(leftFunctionName)].AsBindableProperty<string>().RegesterValueChangedEvent(_ => { Refresh(); });

            this[nameof(rightValueType)].AsBindableProperty<ValueType>().RegesterValueChangedEvent(_ => { Refresh(); });
            this[nameof(rightValue)].AsBindableProperty<bool>().RegesterValueChangedEvent(_ => { Refresh(); });
            this[nameof(rightFunctionName)].AsBindableProperty<string>().RegesterValueChangedEvent(_ => { Refresh(); });
        }

        protected override TaskStatus OnUpdate()
        {
            bool left = leftFunction();
            bool right = rightFunction();
            if (left == right)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }


        void Refresh()
        {
            switch (leftValueType)
            {
                case ValueType.Value:
                    leftFunction = () => { return leftValue; };
                    break;
                case ValueType.Function:
                    MethodInfo method = null;
                    if (!string.IsNullOrEmpty(leftFunctionName))
                        method = Agent.GetType().GetMethod(leftFunctionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (method != null)
                        leftFunction = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), Agent, method);
                    else
                        leftFunction = () => { return false; };
                    break;
            }

            switch (rightValueType)
            {
                case ValueType.Value:
                    rightFunction = () => { return rightValue; };
                    break;
                case ValueType.Function:
                    MethodInfo method = null;
                    if (!string.IsNullOrEmpty(rightFunctionName))
                        method = Agent.GetType().GetMethod(rightFunctionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (method != null)
                        rightFunction = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), Agent, method);
                    else
                        rightFunction = () => { return false; };
                    break;
            }
        }
    }
}