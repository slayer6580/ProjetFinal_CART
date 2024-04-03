using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Manager.AudioSystem;

namespace Manager
{
    [CustomEditor(typeof(AudioSystem))]
    public class AudioSystemEditor : Editor
    {
        AudioSystem audioSystem;
        private int selectedPlayerSoundIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // The pool can be filled not in play mode in the editor
            // without acces to the Ausiosystem singleton (that would require play mode)
            audioSystem = (AudioSystem)target; 

            if (GUILayout.Button("Refresh Sound Pool"))
            {
                Debug.Log("Refreshing sound pool...");
                audioSystem.RefreshSoundPool();
            }

            //// Play player sound type
            //if (GUILayout.Button("Play Player Sound"))
            //{
            //    // Select player sound to play

            //    Debug.Log("Playing player sound...");
            //    audioSystem.PlaySoundByType(SoundType.Player);
            //}

            EditorGUILayout.LabelField("Player Sounds");
            if (audioSystem.GetDictionary().TryGetValue(SoundType.Player, out var playerSounds))
            {
                string[] options = playerSounds.Select(clip => clip.name).ToArray();
                selectedPlayerSoundIndex = EditorGUILayout.Popup("Select Sound", selectedPlayerSoundIndex, options);

                if (GUILayout.Button("Play Player Sound"))
                {
                    if (playerSounds.Length > selectedPlayerSoundIndex)
                    {
                        AudioClip selectedClip = playerSounds[selectedPlayerSoundIndex];
                        Debug.Log("Playing player sound: " + selectedClip.name);
                        audioSystem.PlaySound(selectedClip);  
                    }
                }
            }
        }
    }
}
