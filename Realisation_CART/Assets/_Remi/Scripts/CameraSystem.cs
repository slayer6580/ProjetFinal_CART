using CartControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Camera
{
    [ExecuteInEditMode]
    public class CameraSystem : MonoBehaviour
    {

        void Start()
        {
            Scene scene = gameObject.scene;
            GameObject[] gameObjects = scene.GetRootGameObjects();
            GameObject character = null;

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<CartStateMachine>() == null) continue;
                
                if (gameObject.name != "Character") continue;

                character = gameObject;
            }

            if (character == null) Debug.LogError("Character not found");

            Cinemachine.CinemachineVirtualCamera VirtualCamera = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
            if (VirtualCamera == null) Debug.LogError("No virtual camera found in children of " + gameObject.name);

            VirtualCamera.Follow = character.transform;
            VirtualCamera.LookAt = character.transform;
        }
    }
}
