using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
		public Node m_rootNode;
        public Node.State m_treeState = Node.State.Running;
        public List<Node> m_allTheNodes = new List<Node>();
		public Blackboard m_blackboard = new Blackboard();

        public Node.State Update()
        {
            if(m_rootNode.m_state == Node.State.Running)
            {
				//Update the root, that will update his child
                m_treeState = m_rootNode.Update();
            }
            return m_treeState;
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.m_guid = GUID.Generate().ToString();

			Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
			m_allTheNodes.Add(node);

			if (!Application.isPlaying)
			{
				AssetDatabase.AddObjectToAsset(node, this);
			}

			//Since we used the tree(this) to undo, we need to specify which parameter to specifically undo
			Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
            AssetDatabase.SaveAssets();

			return node;
		}

        public void DeleteNode(Node node)
        {
			Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
            m_allTheNodes.Remove(node);
            
			//AssetDatabase.RemoveObjectFromAsset(node);
			//Instead of the last line, if we want the undo to work, we need to use DestroyObjectImmeditae
			Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }
       
		//Children are the node that are connected from the parent's output
        public void AddChild(Node parent, Node child)
        {
			CompositeNode composite = parent as CompositeNode;
			if (composite)
			{
				Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
				composite.m_children.Add(child);
				EditorUtility.SetDirty(composite);
				return;
			}

			DecoratorNode decorator = parent as DecoratorNode;
            if(decorator)
            {
				Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
				decorator.m_child = child;
				EditorUtility.SetDirty(decorator);
				return;
            }

			RootNode rootNode = parent as RootNode;
			if (rootNode)
			{
				Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
				rootNode.m_child = child;
				EditorUtility.SetDirty(rootNode);
				return;
			}
		}

		public void RemoveChild(Node parent, Node child)
		{
			CompositeNode composite = parent as CompositeNode;
			if (composite)
			{
				Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
				composite.m_children.Remove(child);
				EditorUtility.SetDirty(composite);
				return;
			}

			DecoratorNode decorator = parent as DecoratorNode;
			if (decorator)
			{
				Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
				decorator.m_child = null;
				EditorUtility.SetDirty(decorator);
				return;
			}

			RootNode rootNode = parent as RootNode;
			if (rootNode)
			{
				Undo.RecordObject(rootNode, "Behaviour Tree (RemoveChild)");
				rootNode.m_child = null;
				EditorUtility.SetDirty(rootNode);
				return;
			}
		}

		public List<Node> GetChildren(Node parent)
		{
            List<Node> children = new List<Node>();

			CompositeNode composite = parent as CompositeNode;
			if (composite)
			{
				return composite.m_children;
			}

			DecoratorNode decorator = parent as DecoratorNode;
			if (decorator && decorator.m_child != null)
			{
				children.Add(decorator.m_child);
				return children;
			}

			RootNode rootNode = parent as RootNode;
			if (rootNode && rootNode.m_child != null)
			{
				children.Add(rootNode.m_child);
			}

			return children;
		}

		//Instantiate this so we can have multiple instance of the same BehaviourTree
		public BehaviourTree Clone()
		{
			BehaviourTree tree = Instantiate(this);
			tree.m_rootNode = tree.m_rootNode.Clone();
			tree.m_allTheNodes = new List<Node>();
			
			
			Traverse(tree.m_rootNode, (n) =>
			{
				tree.m_allTheNodes.Add(n);
			});
			
			return tree;
		}


		public void Traverse(Node node, System.Action<Node> visiter)
		{
			if (node)
			{
				visiter.Invoke(node);
				var children = GetChildren(node);
				children.ForEach((n) => Traverse(n, visiter));
			}
		}
		
		//Insert some AiAgent things here...
		public void Bind()
		{
			Traverse(m_rootNode, node =>
			{
				node.m_Blackboard = m_blackboard;
			});
		}
		
	}
}
