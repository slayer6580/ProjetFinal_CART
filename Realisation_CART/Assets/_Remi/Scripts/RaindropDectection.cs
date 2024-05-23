using UnityEngine;

namespace Shader
{
    public class RaindropDectection : MonoBehaviour
    {
        [field: SerializeField] private RaidropShaderUpdater _RaidropShaderUpdater { get; set; } = null;

        private void Awake()
        {
            if (gameObject.layer != GameConstants.RAINDROPSCREEN_COLLIDER)
                gameObject.layer = GameConstants.RAINDROPSCREEN_COLLIDER;

            // If no rigidbody is attached to the object, add one
            if (GetComponent<Rigidbody>() == null)
            {
                gameObject.AddComponent<Rigidbody>();
                GetComponent<Rigidbody>().isKinematic = true;
            }

            _RaidropShaderUpdater = FindObjectOfType<RaidropShaderUpdater>();
            if (_RaidropShaderUpdater == null) Debug.LogError("RaindropDectection: _RaidropShaderUpdater not found");
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Collider tag : " + other.gameObject.tag);
            //if (other.gameObject.tag != "MainCamera") return;
            if (other.gameObject.layer != GameConstants.CAMERA) return;
            Debug.Log("Collider : " + other.gameObject.name);

            Debug.Log("Camera entered raindrop area");
            _RaidropShaderUpdater.ActivateRaindrops();
        }
    }
}
