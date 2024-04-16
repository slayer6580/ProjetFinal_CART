using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace BehaviourTree
{
	//This class manage the node section of the tool interface
    public class BehaviourTreeView : GraphView
    {
		public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
		public Action<NodeView> m_onNodeSelected;	
		private BehaviourTree m_tree;

		public BehaviourTreeView()
		{
			//Add UI stylesheet for the background
			Insert(0, new GridBackground());

			//Add function to manipulate the area
			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor/BehaviourTreeEditor.uss");
			styleSheets.Add(styleSheet);

			Undo.undoRedoPerformed += OnUndoRedo;
		}

		private void OnUndoRedo()
		{
			PopulateView(m_tree);
			AssetDatabase.SaveAssets();
		}

		//NodeView have all the information on how to show the node. NodeView have a reference to the node it represent.
		//We have assing an ID to each node when created
		NodeView FindNodeView(Node node)
		{
			return GetNodeByGuid(node.m_guid) as NodeView;
		}

		//Called when we select the BehaviourTree in the Project tab to show everything
		internal void PopulateView(BehaviourTree tree)
		{
			
			m_tree = tree;

			//Delete what we had and show the new selected one
			graphViewChanged -= OnGraphViewChanged;
			DeleteElements(graphElements);
			graphViewChanged += OnGraphViewChanged;

			//If there's no Root node, create one
			if (m_tree.m_allTheNodes.OfType<RootNode>().Any() == false)
			{
				m_tree.m_rootNode = m_tree.CreateNode(typeof(RootNode)) as RootNode;

				//Need this because some times change get lost after assembly reload
				EditorUtility.SetDirty(m_tree);
				AssetDatabase.SaveAssets();
			}

			//Creates node view in the graph
			tree.m_allTheNodes.ForEach(n => CreateNodeView(n));     //Equivalent of this:
															//foreach (Node n in tree.nodes)
															//{
															//	  CreateNodeView(n);
															//}
			//Creates edges(lines) in the graph
			foreach (Node n in tree.m_allTheNodes)
			{
				List<Node> children = tree.GetChildren(n);

				foreach(Node child in children)
				{
					NodeView parentView = FindNodeView(n);
					NodeView childView = FindNodeView(child);

					Edge edge = parentView.m_output.ConnectTo(childView.m_input);
					AddElement(edge);
				}			
			}
		}
		
		//Called when we try to create/connect an edge
		//Tells which port we can attach the edge. Without this we can't connect any nodes
		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			List<Port> compatiblePortList = new List<Port>();

			foreach(Port endPort in ports)
			{
				if(endPort.direction != startPort.direction && endPort.node != startPort.node)
				{
					compatiblePortList.Add(endPort);
				}
			}

			return compatiblePortList;
			
		} 

		//Called everytime we modify something in the behaviourTree interface
		private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
		{
			//When we remove something, is automatically added to elementsToRemove
			if (graphViewChange.elementsToRemove != null)
			{
				foreach(var elem in graphViewChange.elementsToRemove)
				{
					//Remove a node
					NodeView nodeView = elem as NodeView;
					if(nodeView != null)
					{
						m_tree.DeleteNode(nodeView.m_node);
					}

					//Remove a connection between nodes
					Edge edge = elem as Edge;
					if(edge != null)
					{
						NodeView parentView = edge.output.node as NodeView;
						NodeView childView = edge.input.node as NodeView;
						m_tree.RemoveChild(parentView.m_node, childView.m_node);
					}
				}
			}


			//Save new connection between nodes
			if(graphViewChange.edgesToCreate != null)
			{
				graphViewChange.edgesToCreate.ForEach(edge =>
				{
					NodeView parentView = edge.output.node as NodeView;
					NodeView childView = edge.input.node as NodeView;
					m_tree.AddChild(parentView.m_node, childView.m_node);
				});
			}

			//Call sorting children of composite when moving nodes.
			if(graphViewChange.movedElements != null)
			{
				nodes.ForEach((n) =>
				{
					NodeView view = n as NodeView;
					view.SortChildren();
				});
			}
			return graphViewChange;
		}

		public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{			
			//Get the mouse position in the tree panel
			Vector2 localMousePos = ElementAt(0).ChangeCoordinatesTo(ElementAt(1), evt.localMousePosition);

			{
				var types = TypeCache.GetTypesDerivedFrom<LeafNode>();
				foreach (var type in types)
				{			
					evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (async) => CreateNode(type, localMousePos));
				}
			}

			{
				var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
				foreach (var type in types)
				{
					evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (async) => CreateNode(type, localMousePos));
				}
			}

			{
				var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
				foreach (var type in types)
				{
					evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (async) => CreateNode(type, localMousePos));
				}
			}
		}
		
		void CreateNode(System.Type type, Vector2 mousePosition)
		{
			Node node = m_tree.CreateNode(type);
			node.m_position = mousePosition;
			CreateNodeView(node);
		}

		void CreateNodeView(Node node)
		{
			NodeView nodeView = new NodeView(node);

			//Here OnNodeSelected become a reference of m_onNodeSelected so they become both the same delegate
			nodeView.m_OnNodeSelected = m_onNodeSelected;
			AddElement(nodeView);
		}

		public void UpdateNodeState()
		{
			nodes.ForEach(n =>
			{
				NodeView view = n as NodeView;
				view.UpdateState();
			});
		}
	}
}
