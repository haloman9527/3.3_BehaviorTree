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
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract partial class Compsite : Task
    {
        public IEnumerable<Task> Children
        {
            get
            {
                foreach (var node in GetConnections("Children"))
                {
                    if (node is Task task)
                        yield return task;
                }
            }
        }

        [NonSerialized] protected List<Task> tasks;

        protected override void OnEnabled()
        {
            base.OnEnabled();
            AddPort(new BasePort("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
            AddPort(new BasePort("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(Task)));
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (tasks == null)
                tasks = new List<Task>(Children);

            Ports["Children"].onConnected += _ => { Refresh(); };
            Ports["Children"].onDisconnected += _ => { Refresh(); };
            Ports["Children"].onSorted += Refresh;

            void Refresh()
            {
                tasks.Clear();
                tasks.AddRange(Children);
            }
        }
    }
}