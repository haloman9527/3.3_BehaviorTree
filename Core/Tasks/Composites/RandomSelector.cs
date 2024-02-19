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
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */

#endregion

using System;
using CZToolKit;
using CZToolKit.GraphProcessor;

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
    public class RandomSelectorProcessor : CompositeTaskProcessor
    {
        private int currentIndex;
        private Random random;

        public RandomSelectorProcessor(RandomSelector model) : base(model)
        {
            this[nameof(RandomSelector.randomSeed)] = new BindableProperty<int>(() => model.randomSeed, v => model.randomSeed = v);
            
            random = new Random(model.randomSeed);
        }

        protected override void DoStart()
        {
            if (Children.Count == 0)
                SelfStop(true);
            else
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    var index = random.Next(0, i + 1);
                    var temp = Children[i];
                    Children[i] = Children[index];
                    Children[index] = temp;
                }
            
                currentIndex = 0;
                Children[currentIndex].Start();
            }
        }

        protected override void OnChildStopped(TaskProcessor child, bool result)
        {
            if (result)
                SelfStop(true);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                SelfStop(false);
        }

        private void Continue()
        {
            Children[++currentIndex].Start();
        }
    }
}