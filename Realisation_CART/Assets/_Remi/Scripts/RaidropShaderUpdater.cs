using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Shader
{
    public class RaidropShaderUpdater : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_playerRigidbody;
        [SerializeField] private Material m_raindropMaterial;
        private Vector3 m_previousVelocity;
        private string m_playerSpeedPropertyName = "_PlayerSpeed";
        private string m_playerGForcePropertyName = "_PlayerGForce";
        private string m_playerRotationPullPropertyName = "_PlayerRotationPull";
        private float m_previousResult = 0.0f;

        void Update()
        {
            if (m_playerRigidbody == null || m_raindropMaterial == null) return;

            Vector3 playerVelocity = m_playerRigidbody.velocity;
            float playerMagnitude = m_playerRigidbody.velocity.magnitude;

            // Calculate cross product to determine turning direction
            Vector3 crossProduct = Vector3.Cross(m_previousVelocity.normalized, playerVelocity.normalized);
            m_previousVelocity = playerVelocity;
            float direction = 1;
            if (crossProduct.y > 0)
            {
                //Debug.Log("Player is turning right");
                direction = 1;
            }
            else if (crossProduct.y < 0)
            {
                //Debug.Log("Player is turning left");
                direction = -1;
            }

            float displacementX = math.abs(playerVelocity.x);
            float displacementY = math.abs(playerVelocity.y);
            float displacementZ = math.abs(playerVelocity.z);

            displacementX = math.round(displacementX * 100) / 100;
            displacementY = math.round(displacementY * 100) / 100;
            displacementZ = math.round(displacementZ * 100) / 100;

            float result = playerMagnitude * direction;
            float lerpResult = Mathf.Lerp(m_previousResult, result, Time.deltaTime);
            m_previousResult = lerpResult;

            playerMagnitude *= 0.1f;
            playerMagnitude = math.abs(playerMagnitude);
            m_raindropMaterial.SetFloat(m_playerSpeedPropertyName, playerMagnitude);
            m_raindropMaterial.SetFloat(m_playerRotationPullPropertyName, lerpResult);
            //m_previousVelocity = playerVelocity;
        }
    }
}
