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
using CZToolKit.BehaviorTree;
using System.Diagnostics;

public class ExampleAgent : BehaviorTreeAgent
{
    public bool leftValue;
    public bool rightValue;

    public bool RightValue()
    {
        return rightValue;
    }
    public bool LeftValue()
    {
        return leftValue;
    }
}
