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
using CZToolKit.Common;
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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            evt.menu.AppendAction($"Execute", _ => { T_ViewModel.Start(); });
            evt.menu.AppendAction($"Stop", _ => { T_ViewModel.Stop(); });
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

        protected override void OnBindingProperties()
        {
            base.OnBindingProperties();
            if (T_ViewModel.CurrentState == TaskState.Active)
                OnStart();
            T_ViewModel.OnStart += OnStart;
            T_ViewModel.OnStop += OnStop;
        }

        protected override void OnUnBindingProperties()
        {
            base.OnUnBindingProperties();
            T_ViewModel.OnStart -= OnStart;
            T_ViewModel.OnStop -= OnStop;
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

        private void OnStart()
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            anim = 1;
            stateBorder.RemoveFromClassList("success");
            stateBorder.RemoveFromClassList("failure");
            stateBorder.RemoveFromClassList("running");
            
            stateBorder.AddToClassList("running");
        }

        private void OnStop(bool result)
        {
            if (!Application.isPlaying || Owner.GraphWindow.GraphOwner == null)
                return;
            anim = 1;
            stateBorder.RemoveFromClassList("success");
            stateBorder.RemoveFromClassList("failure");
            stateBorder.RemoveFromClassList("running");

            if (result)
                stateBorder.AddToClassList("success");
            else
                stateBorder.AddToClassList("failure");
        }
    }
}
