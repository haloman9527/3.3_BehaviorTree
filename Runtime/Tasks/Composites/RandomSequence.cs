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
    [TaskIcon("BehaviorTree/Icons/RandomSequence")]
    [NodeTitle("随机顺序")]
    [NodeTooltip("以随机顺序执行行为，遇Failure或Running中断，并返回该状态")]
    [NodeMenu("Composite/Random Sequence")]
    public class RandomSequence : Composite
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSequence))]
    public class RandomSequenceVM : CompositeVM
    {
        int index;

        public RandomSequenceVM(RandomSequence model) : base(model)
        {
            this[nameof(RandomSequence.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);
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
                if (tmpStatus == TaskResult.Failure)
                {
                    return TaskResult.Failure;
                }
                if (tmpStatus == TaskResult.Running)
                {
                    return TaskResult.Running;
                }
                index++;
            }
            return TaskResult.Success;
        }
    }
}
