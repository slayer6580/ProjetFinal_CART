using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class TowerBalanceAnimCtrlr : MonoBehaviour
    {
        [SerializeField] private Animator m_towerAnimator;
        [SerializeField] private float m_towerBlendValue;
        private float m_stationnaryMovement;
        private bool m_stationnaryMovementUp;
        private float m_lerpAnim;

        private float m_maxTurningForce = 30;
        [SerializeField] private float m_tiltSpeed;

		public List<GameObject> m_boxes;
		public List<bool> m_boxesHasFallen;
		private int m_remainingBoxes;

		private float m_tiltValueToFall = 0.05f;
		private bool m_tiltingLeft;
		private GameObject m_checkLastBox;
		private float m_fallingTimer;

		public GameObject m_emptyBoxPrefab;
		// Start is called before the first frame update
		void Start()
        {
			m_remainingBoxes = m_boxes.Count;

		}

        // Update is called once per frame
        void Update()
        {
			if (m_boxesHasFallen[m_remainingBoxes -1] == false)
			{
				if (m_towerBlendValue < 0.5f - m_tiltValueToFall)
				{
					m_tiltingLeft = true;
					m_checkLastBox = m_boxes[m_remainingBoxes -1];
				}
				if(m_towerBlendValue > 0.5f + m_tiltValueToFall)
				{
					m_tiltingLeft = false;
					m_checkLastBox = m_boxes[m_remainingBoxes - 1];
				}
			}
			

			if(m_checkLastBox != null)
			{
				m_fallingTimer += Time.deltaTime;

				if ((m_tiltingLeft == false && m_towerBlendValue < 0.5f + m_tiltValueToFall) 
					|| (m_tiltingLeft == true && m_towerBlendValue > 0.5f - m_tiltValueToFall))
				{
					m_fallingTimer = 0;
				}

				if(m_fallingTimer > 2)
				{
					m_boxesHasFallen[m_remainingBoxes - 1] = true;
					m_checkLastBox.SetActive(false);
					GameObject fallingBox = Instantiate(m_emptyBoxPrefab, m_checkLastBox.transform.position, m_checkLastBox.transform.rotation);
					
					m_checkLastBox = null;
					m_fallingTimer = 0;
					m_remainingBoxes--;

				}
			}



			m_towerAnimator.SetFloat("TowerTilt", m_towerBlendValue);

			if (m_towerBlendValue > 0.49f && m_towerBlendValue < 0.51f)
			{
				if (m_stationnaryMovementUp)
				{
					m_stationnaryMovement += Time.deltaTime * 0.2f;
					if (m_stationnaryMovement >= 0.1f)
					{
						m_stationnaryMovement = 0.1f;
						m_stationnaryMovementUp = false;
					}
				}
				else
				{
					m_stationnaryMovement -= Time.deltaTime * 0.2f;
					if (m_stationnaryMovement <= -0.1f)
					{
						m_stationnaryMovement = -0.1f;
						m_stationnaryMovementUp = true;
					}
				}
			}
			else
			{
				m_stationnaryMovement = 0;
			}
			m_towerAnimator.SetFloat("Stationnary", m_stationnaryMovement);
		}



		public void SetTowerTilt(float value)
		{
			if (value > 5 || value < -5)
			{

				float blendCalcul = (Mathf.Clamp(Mathf.Abs(value) / m_maxTurningForce, 0, 1)) / 2; //This will give value between 0 and 0.5

				if (value > 0)
				{
					m_lerpAnim = 0.5f + blendCalcul;
				}
				else
				{
					m_lerpAnim = 0.5f - blendCalcul;
				}
			}
			else
			{
				m_lerpAnim = 0.5f;
				value = 0;
			}

			float tiltSpeedMultiply = 6 - Mathf.Abs(Mathf.Clamp(value, -5, 5));
			m_towerBlendValue = Mathf.Lerp(m_towerBlendValue, m_lerpAnim, Time.deltaTime * m_tiltSpeed * tiltSpeedMultiply);

			//print("INCLINAISON: " + m_towerBlendValue);
		}



		IEnumerator DropABox(GameObject boxToDrop)
		{
			yield return new WaitForSeconds(1);
			boxToDrop.SetActive(false);
		}
	}
}
