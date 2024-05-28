
#if UNITY_EDITOR
using UnityEditor;

namespace Spawner
{
    [CustomEditor(typeof(CharacterBuilder))]
    public class MyComponentEditor : Editor
    {
        CharacterBuilder characterBodyParts;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterBuilder characterBodyParts = (CharacterBuilder)target;

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
            else if (UnityEngine.GUILayout.Button("Randomize hair"))
            {
                characterBodyParts.RandomizeHair();
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
#endif