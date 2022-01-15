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
using System;

namespace CZToolKit.BehaviorTree
{
    public class TaskIconAttribute : Attribute
    {
        public readonly string path;
        public TaskIconAttribute(string path)
        {
            this.path = path;
        }
    }
}
