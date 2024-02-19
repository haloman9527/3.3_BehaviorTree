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
using CZToolKit.GraphProcessor.Editors;
using UnityEngine;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomView(typeof(Entry))]
    public class EntryView : TaskView
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetMovable(false);
            SetDeletable(false);
        }
    }
}
