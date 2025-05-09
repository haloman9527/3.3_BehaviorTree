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
using System.Reflection;
using Atom.GraphProcessor.Editors;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Atom.BehaviorTree.Editors
{
    [CustomView(typeof(Task))]
    public class TaskView : BaseNodeView<TaskProcessor>
    {
        Image icon;
        IconBadge badge;
        VisualElement stateBorder;
        float anim = 0;
        public TaskView() : base()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("BehaviorTree/Uss/Task"));
            icon = new Image() { name = "icon" };
            controls.Add(icon);

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

        protected override void DoInit()
        {
            base.DoInit();
            if (T_ViewModel.CurrentState == TaskState.Active)
                OnStart();
            T_ViewModel.OnStart += OnStart;
            T_ViewModel.OnStop += OnStop;
            switch (ViewModel)
            {
                case ActionTaskProcessor:
                {
                    this.AddToClassList("action-task");
                    break;
                }
                case DecoratorTaskProcessor:
                {
                    this.AddToClassList("decorator-task");
                    break;
                }
                case CompositeTaskProcessor:
                {
                    this.AddToClassList("composite-task");
                    break;
                }
            }
            var iconAttr = ViewModel.ModelType.GetCustomAttribute<TaskIconAttribute>(true);
            if (iconAttr != null)
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

        protected override void DoUnInit()
        {
            base.DoUnInit();
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
            if (!Application.isPlaying)
                return;
            anim = Mathf.Clamp01(anim - 0.2f);
            if (T_ViewModel.CurrentState == TaskState.Active)
                anim = 1;
            stateBorder.style.opacity = anim;
        }

        private void OnStart()
        {
            if (!Application.isPlaying)
                return;
            anim = 1;
            stateBorder.RemoveFromClassList("success");
            stateBorder.RemoveFromClassList("failure");
            stateBorder.RemoveFromClassList("running");
            
            stateBorder.AddToClassList("running");
        }

        private void OnStop(bool result)
        {
            if (!Application.isPlaying)
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
#endif