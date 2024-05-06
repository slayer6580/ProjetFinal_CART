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

            if (UnityEngine.GUILayout.Button("Empty all variables"))
            {
                characterBodyParts.EmptyAllVariables();
            }
            else if (UnityEngine.GUILayout.Button("Initialize variables"))
            {
                characterBodyParts.VerifyIntegrityOfVariables();
            }
            else if (UnityEngine.GUILayout.Button("Disable all body parts"))
            {
                characterBodyParts.DisableAllbodyParts();
            }
            else if (UnityEngine.GUILayout.Button("Randomize face"))
            {
                characterBodyParts.RandomizeFace();
            }
            else if (UnityEngine.GUILayout.Button("Randomize body"))
            {
                characterBodyParts.RandomizeBody();
            }
            else if (UnityEngine.GUILayout.Button("Randomize materials"))
            {
                characterBodyParts.RandomizeBodyMaterial();
            }
            else if (UnityEngine.GUILayout.Button("Give me Bob!"))
            {
                characterBodyParts.GiveMeBob();
            }
            else  if (UnityEngine.GUILayout.Button("Give me Bob with custom boots!"))
            {
                characterBodyParts.GiveMeBob(true);
            }
        }
    }
}