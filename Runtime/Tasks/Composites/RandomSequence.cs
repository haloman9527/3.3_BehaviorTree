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
    [NodeTooltip("以随机顺序执行行为，遇Failure停止，并返回Failure，否则返回Success")]
    [NodeMenu("Composite/Random Sequence")]
    public class RandomSequence : Task
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSequence))]
    public class RandomSequenceVM : CompositeTaskVM
    {
        private int currentIndex;

        public RandomSequenceVM(RandomSequence model) : base(model)
        {
            this[nameof(RandomSequence.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
            {
                Stopped(true);
                return;
            }

            for (int i = Children.Count; i > 0; i--)
            {
                var index = Random.Range(0, i);
                Children.Add(Children[index]);
                Children.RemoveAt(index);
            }
            currentIndex = 0;
            Children[currentIndex].Start();
        }

        protected override void DoStop()
        {
            Children[currentIndex].Stop();
        }

        protected override void OnChildStopped(TaskVM child, bool result)
        {
            if (!result)
                Stopped(false);
            else if (++currentIndex < Children.Count)
                Restart();
            else
                Stopped(true);
        }

        private void Restart()
        {
            Children[currentIndex].Start();
        }
    }
}
