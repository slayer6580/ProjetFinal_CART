using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;
using UnityEditor.IMGUI.Controls;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace BehaviourTree
{
	public class BehaviourTreeEditor : EditorWindow
	{
		BehaviourTreeView m_treeView;		//Section with the nodes
		InspectorView m_inspectorView;	//Section with the values of the selected node.
		IMGUIContainer m_blackboardView;

		SerializedObject m_treeObject;
		SerializedProperty m_blackboardProperty;

		//To create a button in the editor toolbar to open the BehaviourTree editor
		[MenuItem("BehaviourTreeEditor/Editor...")]
		public static void OpenWindow()
		{
			BehaviourTreeEditor window = GetWindow<BehaviourTreeEditor>();
			window.titleContent = new GUIContent("BehaviourTreeEditor");
		}

		//To open the tool by double cliking on it in Project
		[OnOpenAsset]
		public static bool OnOpenAsset(int instanceId, int line)
		{
			if(Selection.activeObject is BehaviourTree)
			{
				OpenWindow();
				return true;
			}
			return false;
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

			m_blackboardView = root.Q<IMGUIContainer>();

			m_blackboardView.onGUIHandler = () =>
			{
				if(m_treeObject != null)
				{
					m_treeObject.Update();
					EditorGUILayout.PropertyField(m_blackboardProperty);
					m_treeObject.ApplyModifiedProperties();
				}
				
			};
			


			//Because the BehaviourTreeView don't have the inspectorView in reference we subscribe the inspector update to an Action<>
			m_treeView.m_onNodeSelected = OnNodeSelectionChanged;
			
			//Once everyting is set, update the view
			OnSelectionChange();
		}

		private void OnEnable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
		}

		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			switch (obj)
			{
				case PlayModeStateChange.EnteredEditMode:
					OnSelectionChange();
					break;

				case PlayModeStateChange.ExitingEditMode:
					break;

				case PlayModeStateChange.EnteredPlayMode:
					OnSelectionChange();
					break;

				case PlayModeStateChange.ExitingPlayMode:
					break;
			}
		}
		//This is called when a file is selected in the project tab.
		public void OnSelectionChange()
		{
			//Check if the selected object is a BehaviourTree (A scriptable objet, not the script itself)
			BehaviourTree tree = Selection.activeObject as BehaviourTree;

			//If the BehaviourTree is not selected in Project
			if (!tree)
			{
				//Check the gameObject selected in hierachy if it has a BehaviourTree on it
				if (Selection.activeGameObject)
				{
					BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
					if (runner)
					{
						tree = runner.m_tree;
					}
				}
			}

			if(Application.isPlaying)
			{
				if (tree)
				{
					m_treeView?.PopulateView(tree);				
				}
			}
			//AssetDatabase.. is to fix some bugs(don't know exaclty what)
			else if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
			{
				m_treeView.PopulateView(tree);
			}
			
			if(tree != null)
			{
				m_treeObject = new SerializedObject(tree);
				m_blackboardProperty = m_treeObject.FindProperty("m_blackboard");
			}
		}

		void OnNodeSelectionChanged(NodeView node)
		{
			m_inspectorView.UpdateSelection(node);
		}

		private void OnInspectorUpdate()
		{
			m_treeView?.UpdateNodeState();
		}
	}
}
