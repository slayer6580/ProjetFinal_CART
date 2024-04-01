using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviourTree
{
	//This class manage how the node is viewed in the UI interface
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
		public Action<NodeView> m_OnNodeSelected;
		public Node m_node;
		public Port m_input;
		public Port m_output;

        public NodeView(Node node)
        {
            m_node = node;

			//Assign some value to the variables of GraphView.Node
			this.title = node.name;
			this.viewDataKey = m_node.m_guid;
			this.style.left = node.m_position.x;
			this.style.top = node.m_position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

		//Create the port to assign a node as a child of another one
		private void CreateInputPorts()
		{
			if (m_node is LeafNode)
			{
				m_input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is CompositeNode)
			{
				m_input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is DecoratorNode)
			{
				m_input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
		
			if (m_input != null)
			{
				m_input.portName = "in";
				inputContainer.Add(m_input);
			}
		}

		//Create the port to assign a node as a parent of another one
		private void CreateOutputPorts()
		{
			if (m_node is CompositeNode)
			{
				m_output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
			}
			else if (m_node is DecoratorNode)
			{
				m_output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is RootNode)
			{
				m_output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
			}

			if (m_output != null)
			{
				m_output.portName = "out";
				outputContainer.Add(m_output);
			}

		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);
			m_node.m_position.x = newPos.xMin;
			m_node.m_position.y = newPos.yMin;
		}

		public override void OnSelected()
		{
			base.OnSelected();
			if(m_OnNodeSelected != null)
			{
				m_OnNodeSelected.Invoke(this);
			}
		}
	}
}
