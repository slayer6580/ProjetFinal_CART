using UnityEngine;

namespace Shader
{
    public class RaindropDectection : MonoBehaviour
    {
        [field: SerializeField] private RaidropShaderUpdater _RaidropShaderUpdater { get; set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != GameConstants.PLAYER_COLLIDER)
                return;

            Debug.Log("Player entered raindrop area");
            _RaidropShaderUpdater.ActivateRaindrops();
        }
    }
}
