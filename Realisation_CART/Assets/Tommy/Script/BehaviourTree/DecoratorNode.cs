using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	//Decorator node can only have one child. Their function is either to transform
	//the result they receive from their child node's status.
	public abstract class DecoratorNode : Node
    {
		[HideInInspector] public Node m_child;

		//To allow multiple instance of the same BehaviourTree to execute at the same time
		public override Node Clone()
		{
			DecoratorNode node = Instantiate(this);
			node.m_child = m_child.Clone();
			return node;
		}
	}
}
