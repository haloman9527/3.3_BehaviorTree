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
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [NodeTitle("随机选择")]
    [NodeTooltip("以随机顺序执行行为，直到返回Success或Running")]
    [NodeMenu("Composite/Random Selector")]
    public class RandomSelector : Composite
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSelector))]
    public class RandomSelectorVM : CompositeVM
    {
        int index;

        public RandomSelectorVM(RandomSelector model) : base(model)
        {
            this[nameof(RandomSelector.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);
        }

        protected override void OnStart()
        {
            base.OnStart();
            index = 0;
            for (int i = tasks.Count; i > 0; i--)
            {
                var index = Random.Range(0, i);
                tasks.Add(tasks[index]);
                tasks.RemoveAt(index);
            }
        }

        protected override TaskResult OnUpdate()
        {
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var tmpStatus = task.Update();
                if (tmpStatus == TaskResult.Success)
                {
                    return TaskResult.Success;
                }
                if (tmpStatus == TaskResult.Running)
                {
                    return TaskResult.Running;
                }
                index++;
            }
            return TaskResult.Failure;
        }
    }
}
