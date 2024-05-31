using CartControl;
using Unity.Mathematics;
using UnityEngine;

namespace Shader
{
    [ExecuteInEditMode]
    public class RaidropShaderUpdater : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_playerRigidbody;
        [SerializeField] private Material m_raindropMaterial;
        private Vector3 m_previousVelocity;
        private string m_playerSpeedPropertyName = "_PlayerSpeed";
        private string m_playerRotationPullPropertyName = "_PlayerRotationPull";
        private string m_rainAmountPropertyName = "_RainAmount";
        private string m_isActivePropertyName = "_IsActive";
        private float m_previousResult = 0.0f;
        private float m_rainamount = 0.0f;
        private float m_isRaindropActive = 0.0f;

        private void Awake()
        {
            DeactivateRaindop();
            CartStateMachine cartStateMachine = FindObjectOfType<CartStateMachine>();
            if (cartStateMachine.gameObject.name != "Character") Debug.LogError("CartStateMachine not found or not the Player.");
            m_playerRigidbody = cartStateMachine.GetComponent<Rigidbody>();
            if (m_playerRigidbody == null) Debug.LogError("Player Rigidbody not found.");
        }

        void Update()
        {
            if (m_isRaindropActive == 0.0f) return;

            float playerMagnitude = m_playerRigidbody.velocity.magnitude;

            UpdatePlayerRotationPull(playerMagnitude);

            playerMagnitude = UpdatePlayerAccelerationPull(playerMagnitude);

            UpdateRainAmount(playerMagnitude);
        }

        private void UpdateRainAmount(float playerMagnitude)
        {
            m_rainamount = Mathf.Lerp(m_rainamount, m_rainamount - playerMagnitude, Time.deltaTime * 0.1f);
            m_rainamount = Mathf.Lerp(m_rainamount, 0.0f, Time.deltaTime * 0.1f);

            m_raindropMaterial.SetFloat(m_rainAmountPropertyName, m_rainamount);

            if (m_rainamount <= 0.01f)
            {
                DeactivateRaindop();
            }
        }

        private float UpdatePlayerAccelerationPull(float playerMagnitude)
        {
            playerMagnitude *= 0.1f;
            playerMagnitude = math.abs(playerMagnitude);
            m_raindropMaterial.SetFloat(m_playerSpeedPropertyName, playerMagnitude);
            return playerMagnitude;
        }

        private void UpdatePlayerRotationPull(float playerMagnitude)
        {
            Vector3 playerVelocity = m_playerRigidbody.velocity;
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

            float result = playerMagnitude * direction;
            float lerpedDirectionForce = Mathf.Lerp(m_previousResult, result, Time.deltaTime);
            m_previousResult = lerpedDirectionForce;
            m_raindropMaterial.SetFloat(m_playerRotationPullPropertyName, lerpedDirectionForce);
        }

        public void ActivateRaindrops()
        {
            Debug.Log("Activate Raindrops");
            m_isRaindropActive = 1.0f;
            m_raindropMaterial.SetFloat(m_isActivePropertyName, m_isRaindropActive);
            m_rainamount = 1.0f;
        }

        private void DeactivateRaindop()
        {
            m_isRaindropActive = 0.0f;
            m_raindropMaterial.SetFloat(m_isActivePropertyName, m_isRaindropActive);
            m_rainamount = 0.0f;
        }
    }
}
