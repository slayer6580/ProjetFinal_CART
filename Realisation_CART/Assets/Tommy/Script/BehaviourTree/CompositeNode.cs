using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	//Composite node process one or more children in a given order (or random),
	//and at some stage will consider their processing complete and pass either success or failure to their parent,
	//often determined by the success or failure of the child nodes.
	public abstract class CompositeNode : Node
    {
		[HideInInspector] public List<Node> m_children = new List<Node>();

		//To allow multiple instance of the same BehaviourTree to execute at the same time
		public override Node Clone()
		{
			CompositeNode node = Instantiate(this);
			node.m_children = m_children.ConvertAll(c => c.Clone());
			return node;
		}
	}
}
