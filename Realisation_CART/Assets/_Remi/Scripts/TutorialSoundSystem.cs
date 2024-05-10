using UnityEngine;
using static Manager.AudioManager;

namespace AudioControl
{
    public class TutorialSoundSystem : MonoBehaviour
    {
        [field: SerializeField] public EMusic TutorialMusic { get; private set; } = EMusic.WaitingRoomMusic;

        private void Start()
        {
            _AudioManager.SetTutorialMusic(TutorialMusic);
            _AudioManager.StartCurrentSceneMusic();
        }
    }
}
