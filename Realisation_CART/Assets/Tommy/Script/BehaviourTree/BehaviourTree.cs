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
            m_allTheNodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
		}

        public void DeleteNode(Node node)
        {
            m_allTheNodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }
       
		//Children are the node that are connected from the parent's output
        public void AddChild(Node parent, Node child)
        {
			CompositeNode composite = parent as CompositeNode;
			if (composite)
			{
				composite.m_children.Add(child);
				return;
			}

			DecoratorNode decorator = parent as DecoratorNode;
            if(decorator)
            {
                decorator.m_child = child;
				return;
            }

			RootNode rootNode = parent as RootNode;
			if (rootNode)
			{
				rootNode.m_child = child;
				return;
			}
		}

		public void RemoveChild(Node parent, Node child)
		{
			CompositeNode composite = parent as CompositeNode;
			if (composite)
			{
				composite.m_children.Remove(child);
				return;
			}

			DecoratorNode decorator = parent as DecoratorNode;
			if (decorator)
			{
				decorator.m_child = null;
				return;
			}

			RootNode rootNode = parent as RootNode;
			if (rootNode)
			{
				rootNode.m_child = null;
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
			return tree;
		}
	}
}
