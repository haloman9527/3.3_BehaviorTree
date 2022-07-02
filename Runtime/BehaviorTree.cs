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
    public partial class BehaviorTree : BaseGraph
    {
        [SerializeField] internal string entryGUID;
    }

    public partial class BehaviorTree : IGraphForMono
    {
        [NonSerialized] internal Entry entry;
        [NonSerialized] internal List<SharedVariable> variables;

        public IGraphOwner GraphOwner
        {
            get;
            private set;
        }
        public IVariableOwner VarialbeOwner
        {
            get { return Agent; }
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

        protected override void OnEnabled()
        {
            base.OnEnabled();

            if (!string.IsNullOrEmpty(entryGUID) && !Nodes.ContainsKey(entryGUID))
                entryGUID = string.Empty;
            if (string.IsNullOrEmpty(entryGUID))
                entryGUID = Nodes.Values.FirstOrDefault(node => node is Entry)?.GUID;
            if (string.IsNullOrEmpty(entryGUID))
                entryGUID = AddNode<Entry>(InternalVector2.zero).GUID;
            entry = Nodes[entryGUID] as Entry;

            OnNodeAdded += NodeAdded;
        }

        public void Initialize(IGraphOwner graphOwner)
        {
            GraphOwner = graphOwner;
            Agent = GraphOwner as BehaviorTreeAgent;

            foreach (var node in Nodes.Values)
            {
                if (node is INodeForMono monoNode)
                    monoNode.Initialize();
            }

            if (GraphOwner is IVariableOwner variableOwner)
            {
                variables = new List<SharedVariable>();
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

        public void NodeAdded(BaseNode node)
        {
            if (!(node is INodeForMono monoNode))
                return;
            if (GraphOwner != null)
                monoNode.Initialize();

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
                if (node is Task task)
                    task.OnDrawGizmos();
            }
        }
    }
}