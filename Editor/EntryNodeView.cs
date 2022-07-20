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
using CZToolKit.GraphProcessor.Editors;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomView(typeof(Entry))]
    public class EntryNodeView : TaskNodeView
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetMovable(false);
            SetDeletable(false);
            SetSelectable(false);
        }
    }
}
