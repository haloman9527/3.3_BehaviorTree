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
    [NodeTooltip("以随机顺序执行行为，遇Success停止，并返回Success，否则返回Failure")]
    [NodeMenu("Composite/Random Selector")]
    public class RandomSelector : Task
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSelector))]
    public class RandomSelectorVM : CompositeTaskVM
    {
        private int currentIndex;

        public RandomSelectorVM(RandomSelector model) : base(model)
        {
            this[nameof(RandomSelector.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);
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
            if (result)
                Stopped(true);
            else if (++currentIndex < Children.Count)
                Restart();
            else
                Stopped(false);
        }

        private void Restart()
        {
            Children[currentIndex].Start();
        }
    }
}