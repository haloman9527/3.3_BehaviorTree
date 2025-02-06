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

#if UNITY_EDITOR
using Moyo.GraphProcessor.Editors;

namespace Moyo.BehaviorTree.Editors
{
    [CustomView(typeof(Entry))]
    public class EntryView : TaskView
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.Deletable = false;
            this.Movable = false;
        }
    }
}
#endif
