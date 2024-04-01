using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class RootNode : Node
	{
		[HideInInspector] public Node m_child;
		protected override void OnStart()
		{}

		protected override void OnStop()
		{}

		protected override State OnUpdate()
		{
			return m_child.Update();
		}

		//To allow multiple instance of the same BehaviourTree to execute at the same time
		public override Node Clone()
		{
			RootNode node = Instantiate(this);
			node.m_child = m_child.Clone();
			return node;
		}
	}
}
