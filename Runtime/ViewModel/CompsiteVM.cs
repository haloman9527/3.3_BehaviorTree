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
using System.Collections.Generic;

namespace CZToolKit.BehaviorTree
{
    public abstract class CompsiteVM : TaskVM
    {
        protected List<TaskVM> tasks;

        public IEnumerable<TaskVM> Children
        {
            get
            {
                foreach (var node in GetConnections("Children"))
                {
                    if (node is TaskVM task)
                        yield return task;
                }
            }
        }

        protected CompsiteVM(BaseNode model) : base(model)
        {
            base.OnEnabled();
            AddPort(new BasePortVM("Parent", BasePort.Orientation.Vertical, BasePort.Direction.Input, BasePort.Capacity.Single, typeof(Task)));
            AddPort(new BasePortVM("Children", BasePort.Orientation.Vertical, BasePort.Direction.Output, BasePort.Capacity.Multi, typeof(Task)));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Ports["Children"].onSorted += Refresh;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (tasks == null)
                tasks = new List<TaskVM>(Children);
        }

        void Refresh()
        {
            if (tasks != null)
            {
                tasks.Clear();
                tasks.AddRange(Children);
            }
        }
    }
}