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
using CZToolKit.GraphProcessor;

namespace CZToolKit.BehaviorTree
{
    [TaskIcon("BehaviorTree/Icons/Debug")]
    [NodeMenu("Action/Failure")]
    public class FailureTask : Task
    {
        
    }
    
    [ViewModel(typeof(FailureTask))]
    public class FailureTaskProcessor : ActionTaskProcessor
    {
        public FailureTaskProcessor(FailureTask model) : base(model)
        {
            
        }

        protected override void DoStart()
        {
            SelfStop(false);
        }

        protected override void DoStop()
        {
            SelfStop(false);
        }
    }
}
