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
using CZToolKit.Core;
using CZToolKit.GraphProcessor.Editors;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CZToolKit.BehaviorTree.Editors
{
    [CustomView(typeof(Task))]
    public class TaskNodeView : BaseSimpleNodeView<TaskVM>
    {
        Image icon;
        IconBadge badge;
        VisualElement stateBorder;
        float anim = 0;
        public TaskNodeView() : base()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("BehaviorTree/Uss/Task"));

            icon = new Image() { name = "icon" };
            base.controlsContainer.Add(icon);
            icon.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0.7f);

            stateBorder = new VisualElement();
            stateBorder.name = "state-border";
            stateBorder.StretchToParentSize();
            stateBorder.pickingMode = PickingMode.Ignore;
            Insert(0, stateBorder);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Util_Attribute.TryGetTypeAttribute(ViewModel.Model.GetType(), out TaskIconAttribute iconAttr))
            {
                icon.style.backgroundImage = Resources.Load<Texture2D>(iconAttr.path);
            }
            if (ViewModel.Ports.ContainsKey("Children"))
            {
                badge = IconBadge.CreateError("需要子节点");
                RefreshBadge();
                ViewModel.Ports["Children"].onAfterConnected += _ =>
                {
                    RefreshBadge();
                };
                ViewModel.Ports["Children"].onAfterDisconnected += _ =>
                {
                    RefreshBadge();
                };
            }
        }

        void RefreshBadge()
        {
            if (T_ViewModel.Ports["Children"].Connections.Count != 0)
                RemoveBadge(_ => _ == badge);
            else
                AddBadge(badge);
        }

        public void UpdateState()
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            anim = Mathf.Clamp01(anim - 0.2f);
            if (T_ViewModel.CurrentState == TaskState.Active)
                anim = 1;
            stateBorder.style.opacity = anim;
        }

        private void OnUpdate()
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            if (T_ViewModel.Ports.ContainsKey("Parent") && T_ViewModel.Ports["Parent"].Connections.Count == 0)
                return;
            anim = 1;
            stateBorder.RemoveFromClassList("success");
            stateBorder.RemoveFromClassList("failure");
            stateBorder.RemoveFromClassList("running");

            // switch (T_ViewModel.Status)
            // {
            //     case TaskResult.Success:
            //         stateBorder.AddToClassList("success");
            //         break;
            //     case TaskResult.Failure:
            //         stateBorder.AddToClassList("failure");
            //         break;
            //     case TaskResult.Running:
            //         stateBorder.AddToClassList("running");
            //         break;
            //     default:
            //         break;
            // }
        }
    }
}
