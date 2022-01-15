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

namespace CZToolKit.BehaviorTree
{
    public class BehaviorTreeAgent : GraphAssetOwner<BehaviorTreeAsset, BehaviorTree>
    {
        public event Action<Task> onTaskExecute;

        private void Awake()
        {
            T_Graph.Initialize(this);
        }

        private void Update()
        {
            T_Graph.Update();
        }

        private void OnDrawGizmos()
        {
            if (T_Graph != null)
                T_Graph.OnDrawGizmos();
        }
    }
}