using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

namespace BehaviourTree
{
	//This class manage how the node is viewed in the UI interface
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
		public Action<NodeView> m_OnNodeSelected;
		public Node m_node;
		public Port m_input;
		public Port m_output;

        public NodeView(Node node) : base("Assets/Tommy/Script/BehaviourTree/NodeView.uxml")
        {
            m_node = node;

			//Assign some value to the variables of GraphView.Node
			this.title = node.name;
			this.viewDataKey = m_node.m_guid;
			this.style.left = node.m_position.x;
			this.style.top = node.m_position.y;

            CreateInputPorts();
            CreateOutputPorts();
			SetupClasses();

			Label descriptionLabel = this.Q<Label>("description");
			descriptionLabel.Bind(new SerializedObject(node));
			descriptionLabel.bindingPath = "m_description";

		}

		//Add classes to each node to modify them with stylesheets
		private void SetupClasses()
		{
			if (m_node is LeafNode)
			{
				AddToClassList("leaf");
			}
			else if (m_node is CompositeNode)
			{
				AddToClassList("composite");
			}
			else if (m_node is DecoratorNode)
			{
				AddToClassList("decorator");
			}
			else if (m_node is RootNode)
			{
				AddToClassList("root");
			}
		}

		//Create the port to assign a node as a child of another one
		private void CreateInputPorts()
		{
			if (m_node is LeafNode)
			{
				m_input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is CompositeNode)
			{
				m_input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is DecoratorNode)
			{
				m_input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
			}
		
			if (m_input != null)
			{
				m_input.portName = "";
				//FlexDirection is to align the port to show the node vertically
				//m_input.style.flexDirection = FlexDirection.Column;
				inputContainer.Add(m_input);
			}
		}

		//Create the port to assign a node as a parent of another one
		private void CreateOutputPorts()
		{
			if (m_node is CompositeNode)
			{
				m_output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
			}
			else if (m_node is DecoratorNode)
			{
				m_output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
			}
			else if (m_node is RootNode)
			{
				m_output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
			}

			if (m_output != null)
			{
				m_output.portName = "";
				//FlexDirection is to align the port to show the node vertically
				m_output.style.flexDirection = FlexDirection.ColumnReverse;
				outputContainer.Add(m_output);
			}

		}

		public override void SetPosition(Rect newPos)
		{
			base.SetPosition(newPos);
			Undo.RecordObject(m_node, "Behaviour Tree (Set Position");
			m_node.m_position.x = newPos.xMin;
			m_node.m_position.y = newPos.yMin;
			EditorUtility.SetDirty(m_node);
		}

		public override void OnSelected()
		{
			base.OnSelected();
			if(m_OnNodeSelected != null)
			{
				m_OnNodeSelected.Invoke(this);
			}
		}

		public void SortChildren()
		{
			CompositeNode composite = m_node as CompositeNode;
			if (composite)
			{
				composite.m_children.Sort(SortByHorizontalPosition);
			}
		}

		private int SortByHorizontalPosition(Node left, Node right)
		{
			return left.m_position.x < right.m_position.x ? -1 : 1;
		}

		public void UpdateState()
		{
			RemoveFromClassList("running");
			RemoveFromClassList("failure");
			RemoveFromClassList("success");

			if (Application.isPlaying)
			{
				switch (m_node.m_state)
				{
					case Node.State.Running:
						if (m_node.m_started)
						{
							AddToClassList("running");
						}				
						break;

					case Node.State.Failure:
						AddToClassList("failure");
						break;

					case Node.State.Success:
						AddToClassList("success");
						break;
				}
			}
			
		}
	}
}
