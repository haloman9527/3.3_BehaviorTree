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
    [NodeMenuItem("Compsite", "随机选择执行")]
    [NodeTooltip("以随机顺序执行行为，直到返回Success或Running")]
    public class RandomSelector : Compsite
    {
        public int randomSeed;
    }

    [ViewModel(typeof(RandomSelector))]
    public class RandomSelectorVM : CompsiteVM
    {
        int index;

        public RandomSelectorVM(BaseNode model) : base(model)
        {
            var t_model = Model as RandomSelector;
            this[nameof(RandomSelector.randomSeed)] = new BindableProperty<int>(() => t_model.randomSeed, v => t_model.randomSeed = v);
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

        protected override TaskStatus OnUpdate()
        {
            for (int i = index; i < tasks.Count; i++)
            {
                var task = tasks[i];
                var tmpStatus = task.Update();
                if (tmpStatus == TaskStatus.Success)
                {
                    return TaskStatus.Success;
                }
                if (tmpStatus == TaskStatus.Running)
                {
                    return TaskStatus.Running;
                }
                index++;
            }
            return TaskStatus.Failure;
        }
    }
}
