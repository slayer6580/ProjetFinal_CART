using System.Linq;
using UnityEditor;
using UnityEngine;
using static Manager.AudioManager;

namespace Manager
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        AudioManager audioManager;
        private int selectedPlayerSoundIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // The pool can be filled not in play mode in the editor
            // without acces to the Ausiosystem singleton (that would require play mode)
            audioManager = (AudioManager)target; 

            if (GUILayout.Button("Refresh Sound Pool"))
            {
                Debug.Log("Refreshing sound pool...");
                audioManager.RefreshSoundPool();
            }

            EditorGUILayout.LabelField("Client Sounds");
            if (audioManager.GetDictionary().TryGetValue(ESoundType.Client, out var playerSounds))
            {
                string[] options = playerSounds.Select(clip => clip.name).ToArray();
                selectedPlayerSoundIndex = EditorGUILayout.Popup("Select Sound", selectedPlayerSoundIndex, options);

                if (GUILayout.Button("Play Player Sound"))
                {
                    if (playerSounds.Length > selectedPlayerSoundIndex)
                    {
                        AudioClip selectedClip = playerSounds[selectedPlayerSoundIndex];
                        Debug.Log("Playing player sound: " + selectedClip.name);
                        audioManager.PlaySoundInAudioSystemSource(selectedClip);  
                    }
                }
            }
        }
    }
}
