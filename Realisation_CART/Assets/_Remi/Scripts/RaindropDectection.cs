using UnityEngine;

namespace Shader
{
    public class RaindropDectection : MonoBehaviour
    {
        [field: SerializeField] private RaidropShaderUpdater _RaidropShaderUpdater { get; set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Collider : " + other.gameObject.name);
            //Debug.Log("Collider tag : " + other.gameObject.tag);
            //if (other.gameObject.tag != "MainCamera") return;
            if (other.gameObject.layer != GameConstants.CAMERA) return;

            Debug.Log("Camera entered raindrop area");
            _RaidropShaderUpdater.ActivateRaindrops();
        }
    }
}
