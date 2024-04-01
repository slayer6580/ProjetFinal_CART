using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace BehaviourTree
{
	//This class manage the inspector section of the tool interface
	public class InspectorView : VisualElement
    {
        Editor m_editor;

		public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> 
        { 
        }

        internal void UpdateSelection(NodeView nodeView)
        {
            //Remove value of last selected node
            Clear();
            Object.DestroyImmediate(m_editor);

            //Create editor with the value of the newly selected node
            //TODO - () =>  - Pourquoi vide?
            m_editor = Editor.CreateEditor(nodeView.m_node);
            IMGUIContainer container = new IMGUIContainer(() => { m_editor.OnInspectorGUI(); });
            this.Add(container);

		}
    }
}
