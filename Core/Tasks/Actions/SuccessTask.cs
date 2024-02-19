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
    [NodeMenu("Action/Success")]
    public class SuccessTask : Task
    {
        
    }
    
    [ViewModel(typeof(SuccessTask))]
    public class SuccessTaskProcessor : ActionTaskProcessor
    {
        public SuccessTaskProcessor(SuccessTask model) : base(model)
        {
            
        }

        protected override void DoStart()
        {
            SelfStop(true);
        }

        protected override void DoStop()
        {
            SelfStop(false);
        }
    }
}
