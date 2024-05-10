using UnityEngine;
using static Manager.AudioManager;

namespace AudioControl
{
    public class MainMenuSoundSystem : MonoBehaviour
    {
        [field: SerializeField] public EMusic MainMenuMusic { get; private set; } = EMusic.ThemeMusic;

        private void Start()
        {
            _AudioManager.SetMainMenuMusic(MainMenuMusic);
            _AudioManager.StartCurrentSceneMusic();
        }
    }
}
