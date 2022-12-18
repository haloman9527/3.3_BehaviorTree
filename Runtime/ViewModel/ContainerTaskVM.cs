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

namespace CZToolKit.BehaviorTree
{
    public abstract class ContainerTaskVM : TaskVM
    {
        protected ContainerTaskVM(Task model) : base(model)
        {
        }
        
        public void ChildStopped(TaskVM child, bool result)
        {
            this.OnChildStopped(child, result);
        }
        
        protected abstract void OnChildStopped(TaskVM child, bool result);
    }
}