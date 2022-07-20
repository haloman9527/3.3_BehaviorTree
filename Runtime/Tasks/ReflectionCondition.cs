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
using CZToolKit.Core.ViewModel;
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

        public ValueType leftValueType;
        public bool leftValue;
        public string leftFunctionName;

        public ValueType rightValueType;
        public bool rightValue;
        public string rightFunctionName;
    }

    public class ReflectionConditionVM : ActionTaskVM
    {
        Func<bool> leftFunction;
        Func<bool> rightFunction;

        public ReflectionCondition.ValueType LeftValueType
        {
            get { return GetPropertyValue<ReflectionCondition.ValueType>(nameof(ReflectionCondition.leftValueType)); }
            set { SetPropertyValue(nameof(ReflectionCondition.leftValueType), value); }
        }
        public bool LeftValue
        {
            get { return (Model as ReflectionCondition).leftValue; }
            set { (Model as ReflectionCondition).leftValue = value; }
        }
        public string LeftFunctionName
        {
            get { return GetPropertyValue<string>(nameof(ReflectionCondition.leftFunctionName)); }
            set { SetPropertyValue(nameof(ReflectionCondition.leftFunctionName), value); }
        }
        public ReflectionCondition.ValueType RightValueType
        {
            get { return GetPropertyValue<ReflectionCondition.ValueType>(nameof(ReflectionCondition.rightValueType)); }
            set { SetPropertyValue(nameof(ReflectionCondition.rightValueType), value); }
        }
        public bool RightValue
        {
            get { return (Model as ReflectionCondition).rightValue; }
            set { (Model as ReflectionCondition).rightValue = value; }
        }
        public string RightFunctionName
        {
            get { return GetPropertyValue<string>(nameof(ReflectionCondition.rightFunctionName)); }
            set { SetPropertyValue(nameof(ReflectionCondition.rightFunctionName), value); }
        }

        public ReflectionConditionVM(BaseNode model) : base(model)
        {
            var t_model = Model as ReflectionCondition;
            this[nameof(ReflectionCondition.leftValueType)] = new BindableProperty<ReflectionCondition.ValueType>(() => t_model.leftValueType, v => t_model.leftValueType = v);
            this[nameof(ReflectionCondition.leftValue)] = new BindableProperty<bool>(() => t_model.leftValue, v => { t_model.leftValue = v; });
            this[nameof(ReflectionCondition.leftFunctionName)] = new BindableProperty<string>(() => t_model.leftFunctionName, v => t_model.leftFunctionName = v);
            this[nameof(ReflectionCondition.rightValueType)] = new BindableProperty<ReflectionCondition.ValueType>(() => t_model.rightValueType, v => t_model.rightValueType = v);
            this[nameof(ReflectionCondition.rightValue)] = new BindableProperty<bool>(() => t_model.rightValue, v => { t_model.rightValue = v; });
            this[nameof(ReflectionCondition.rightFunctionName)] = new BindableProperty<string>(() => t_model.rightFunctionName, v => t_model.rightFunctionName = v);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Refresh();

            this[nameof(ReflectionCondition.leftValueType)].RegisterValueChangedEvent<ReflectionCondition.ValueType>(_ => { Refresh(); });
            this[nameof(ReflectionCondition.leftValue)].RegisterValueChangedEvent<bool>(_ => { Refresh(); });
            this[nameof(ReflectionCondition.leftFunctionName)].RegisterValueChangedEvent<string>(_ => { Refresh(); });
            this[nameof(ReflectionCondition.rightValueType)].RegisterValueChangedEvent<ReflectionCondition.ValueType>(_ => { Refresh(); });
            this[nameof(ReflectionCondition.rightValue)].RegisterValueChangedEvent<bool>(_ => { Refresh(); });
            this[nameof(ReflectionCondition.rightFunctionName)].RegisterValueChangedEvent<string>(_ => { Refresh(); });
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
            switch (LeftValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    leftFunction = () => { return LeftValue; };
                    break;
                case ReflectionCondition.ValueType.Function:
                    MethodInfo method = null;
                    if (!string.IsNullOrEmpty(LeftFunctionName))
                        method = Agent.GetType().GetMethod(LeftFunctionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (method != null && method.GetParameters().Length == 0)
                        leftFunction = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), Agent, method);
                    else
                        leftFunction = () => { return false; };
                    break;
            }

            switch (RightValueType)
            {
                case ReflectionCondition.ValueType.Value:
                    rightFunction = () => { return RightValue; };
                    break;
                case ReflectionCondition.ValueType.Function:
                    MethodInfo method = null;
                    if (!string.IsNullOrEmpty(RightFunctionName))
                        method = Agent.GetType().GetMethod(RightFunctionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (method != null && method.GetParameters().Length == 0)
                        rightFunction = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), Agent, method);
                    else
                        rightFunction = () => { return false; };
                    break;
            }
        }
    }
}