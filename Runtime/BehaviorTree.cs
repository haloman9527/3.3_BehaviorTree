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
using CZToolKit.Core.SharedVariable;
using CZToolKit.GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CZToolKit.BehaviorTree
{
    public class BehaviorTree : BaseGraph
    {
        [SerializeField] [HideInInspector] public string entryGUID;
    }

    [ViewModel(typeof(BehaviorTree))]
    public class BehaviorTreeVM : BaseGraphVM
    {
        [NonSerialized] internal EntryVM entry;
        [NonSerialized] internal List<SharedVariable> variables;

        public IGraphOwner GraphOwner
        {
            get;
            private set;
        }
        public IReadOnlyList<SharedVariable> Variables
        {
            get { return variables; }
        }
        public BehaviorTreeAgent Agent
        {
            get;
            private set;
        }

        public BehaviorTreeVM(BaseGraph model) : base(model)
        {
            variables = new List<SharedVariable>();
            var t_model = Model as BehaviorTree;
            if (!string.IsNullOrEmpty(t_model.entryGUID) && !Nodes.ContainsKey(t_model.entryGUID))
                t_model.entryGUID = string.Empty;
            if (string.IsNullOrEmpty(t_model.entryGUID))
                t_model.entryGUID = Nodes.Values.FirstOrDefault(node => node is Entry)?.GUID;
            if (string.IsNullOrEmpty(t_model.entryGUID))
                t_model.entryGUID = AddNode<Entry>(InternalVector2.zero).GUID;
            entry = Nodes[t_model.entryGUID] as EntryVM;

            OnNodeAdded += NodeAdded;
        }

        public void Initialize(IGraphOwner graphOwner)
        {
            GraphOwner = graphOwner;
            Agent = GraphOwner as BehaviorTreeAgent;

            foreach (var node in Nodes.Values)
            {
                if (node is TaskVM task)
                    task.Initialize();
            }

            if (GraphOwner is IVariableOwner variableOwner)
            {
                foreach (var node in Nodes.Values)
                {
                    variables.AddRange(SharedVariableUtility.CollectionObjectSharedVariables(node));
                }
                foreach (var variable in variables)
                {
                    variable.InitializePropertyMapping(variableOwner);
                }
            }

            OnInitialized();
        }

        protected virtual void OnInitialized() { }

        public void NodeAdded(BaseNodeVM node)
        {
            if (!(node is TaskVM task))
                return;
            if (GraphOwner != null)
                task.Initialize();

            IEnumerable<SharedVariable> nodeVariables = SharedVariableUtility.CollectionObjectSharedVariables(node);
            variables.AddRange(nodeVariables);
            if (GraphOwner is IVariableOwner variableOwner)
            {
                foreach (var variable in nodeVariables)
                {
                    variable.InitializePropertyMapping(variableOwner);
                }
            }
        }

        public TaskStatus Update()
        {
            return entry.Update();
        }

        public void OnDrawGizmos()
        {
            foreach (var node in Nodes.Values)
            {
                if (node is TaskVM task)
                    task.OnDrawGizmos();
            }
        }
    }
}