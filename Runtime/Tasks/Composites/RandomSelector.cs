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

using CZToolKit.Common.ViewModel;
using CZToolKit.GraphProcessor;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    [NodeTitle("随机选择")]
    [NodeTooltip("随机顺序执行，直到某一行为成功，则返回成功，若所有行为都失败，则返回失败")]
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

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                var index = Random.Range(0, i + 1);
                var temp = Children[i];
                Children[i] = Children[index];
                Children[index] = temp;
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
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                Stopped(false);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}