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
    [CustomNodeView(typeof(Task))]
    public class TaskNodeView : BaseSimpleNodeView<Task>
    {
        Image icon;
        IconBadge badge;
        VisualElement stateBorder;
        float anim = 0;
        public TaskNodeView() : base()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("BehaviorTree/Uss/Task"));

            this.style.width = 100;
            icon = new Image() { name = "icon" };
            icon.style.height = 60;
            icon.style.marginBottom = 10;
            icon.style.marginLeft = 10;
            icon.style.marginRight = 10;
            icon.style.marginTop = 10;
            icon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            icon.style.unityBackgroundImageTintColor = new Color(1, 1, 1, 0.7f);
            base.controlsContainer.style.height = 60;
            base.controlsContainer.Add(icon);

            stateBorder = new VisualElement();
            stateBorder.name = "state-border";
            stateBorder.StretchToParentSize();
            stateBorder.pickingMode = PickingMode.Ignore;
            Insert(0, stateBorder);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Util_Attribute.TryGetTypeAttribute(T_Model.GetType(), out TaskIconAttribute iconAttr))
            {
                icon.style.backgroundImage = Resources.Load<Texture2D>(iconAttr.path);
            }
            if (T_Model.Ports.ContainsKey("Children"))
            {
                badge = IconBadge.CreateError("需要子节点");
                RefreshBadge();
                T_Model.Ports["Children"].onConnected += _ =>
                {
                    RefreshBadge();
                };
                T_Model.Ports["Children"].onDisconnected += _ =>
                {
                    RefreshBadge();
                };
            }

            T_Model.onUpdate += OnUpdate;
        }

        void RefreshBadge()
        {
            if (T_Model.Ports["Children"].Connections.Count != 0)
                RemoveBadge(_ => _ == badge);
            else
                AddBadge(badge);
        }

        public void UpdateState()
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            anim = Mathf.Clamp01(anim - 0.2f);
            if (T_Model.Started)
                anim = 1;
            stateBorder.style.opacity = anim;
        }

        private void OnUpdate()
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            if (T_Model.Ports.ContainsKey("Parent") && T_Model.Ports["Parent"].Connections.Count == 0)
                return;
            anim = 1;
            stateBorder.RemoveFromClassList("success");
            stateBorder.RemoveFromClassList("failure");
            stateBorder.RemoveFromClassList("running");

            switch (T_Model.Status)
            {
                case TaskStatus.Success:
                    stateBorder.AddToClassList("success");
                    break;
                case TaskStatus.Failure:
                    stateBorder.AddToClassList("failure");
                    break;
                case TaskStatus.Running:
                    stateBorder.AddToClassList("running");
                    break;
                default:
                    break;
            }
        }
    }
}
