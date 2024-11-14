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
    [TaskIcon("BehaviorTree/Icons/RandomSequence")]
    [NodeTitle("随机顺序")]
    [NodeTooltip("随机顺序执行，直到某一行为失败，则返回失败，若所有行为都成功，则返回成功")]
    [NodeMenu("Composite/Random Sequence")]
    public class RandomSequence : Task
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSequence))]
    public class RandomSequenceProcessor : CompositeTaskProcessor
    {
        private int currentIndex;
        private Random random;

        public int RandomSeed
        {
            get => (Model as RandomSequence).randomSeed;
            set => SetFieldValue(ref (Model as RandomSequence).randomSeed, value, nameof(RandomSequence.randomSeed));
        }

        public RandomSequenceProcessor(RandomSequence model) : base(model)
        {
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
            if (!result)
                SelfStop(false);
            else if (currentIndex + 1 < Children.Count)
                Continue();
            else
                SelfStop(true);
        }

        private void Continue()
        {
            currentIndex++;
            Children[currentIndex].Start();
        }
    }
}