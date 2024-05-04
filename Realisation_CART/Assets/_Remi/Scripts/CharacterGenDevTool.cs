using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Spawner
{
    [CustomEditor(typeof(CharacterBodyParts))]
    public class MyComponentEditor : Editor
    {
        CharacterBodyParts characterBodyParts;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterBodyParts characterBodyParts = (CharacterBodyParts)target;

            if (UnityEngine.GUILayout.Button("Initialize variables"))
            {
                characterBodyParts.InitializeVariables();
            }

            if (UnityEngine.GUILayout.Button("Empty all variables"))
            {
                characterBodyParts.EmptyAllVariables();
            }

            if (UnityEngine.GUILayout.Button("Disable all body parts"))
            {
                characterBodyParts.DisableAllbodyParts();
            }

            if (UnityEngine.GUILayout.Button("Randomize face"))
            {
                characterBodyParts.RandomizeFace();
            }

            if (UnityEngine.GUILayout.Button("Randomize Full body"))
            {
                characterBodyParts.RandomizeFullBody();
            }

            if (UnityEngine.GUILayout.Button("Give me Bob!"))
            {
                characterBodyParts.GiveMeBob();
            }
        }
    }
}