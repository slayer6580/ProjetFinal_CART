using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviourTree
{
	public class BehaviourTreeEditor : EditorWindow
	{
		BehaviourTreeView m_treeView;		//Section with the nodes
		InspectorView m_inspectorView;	//Section with the values of the selected node.

		//To create a button in the editor toolbar to open the BehaviourTree editor
		[MenuItem("BehaviourTreeEditor/Editor...")]
		public static void OpenWindow()
		{
			BehaviourTreeEditor window = GetWindow<BehaviourTreeEditor>();
			window.titleContent = new GUIContent("BehaviourTreeEditor");
		}

		public void CreateGUI()
		{
			//Each editor window contains a root VisualElement object
			VisualElement root = rootVisualElement;

			//Import UXML - The UXML file define the structure and the hiereachy of the interface
			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
			visualTree.CloneTree(root);

			//The stylesheet contains the visual appearance of UI elements
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
			root.styleSheets.Add(styleSheet);

			//Q for querry, search a child from a certain type
			m_treeView = root.Q<BehaviourTreeView>();
			m_inspectorView = root.Q<InspectorView>();

			//Because the BehaviourTreeView don't have the inspectorView in reference we subscribe the inspector update to an Action<>
			m_treeView.m_onNodeSelected = OnNodeSelectionChanged;
			
			//Once everyting is set, update the view
			OnSelectionChange();
		}

		//This is called when a file is selected in the project tab.
		public void OnSelectionChange()
		{
			//Check if the selected object is a BehaviourTree (A scriptable objet, not the script itself)
			BehaviourTree tree = Selection.activeObject as BehaviourTree;

			//AssetDatabase.. is to fix some bugs..
			if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
			{
				m_treeView.PopulateView(tree);
			}	
		}

		void OnNodeSelectionChanged(NodeView node)
		{
			m_inspectorView.UpdateSelection(node);
		}
	}
}
