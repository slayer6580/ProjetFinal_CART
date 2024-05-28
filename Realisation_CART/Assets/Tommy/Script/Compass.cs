using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private Transform m_compassTarget; 
        [SerializeField] private GameObject m_arrowMesh; 
        [SerializeField] private GameObject m_text; 
        [SerializeField] private Animator m_animator; 
        [SerializeField] private Animator m_textAnimator; 
        [SerializeField] private TowerBoxSystem m_boxSystem; 
        [SerializeField] private List<GameObject> m_checkoutTextList; 

        [SerializeField] private float m_minDistanceToShowCompass; 
  

        [SerializeField] private float m_minSize; 
        [SerializeField] private float m_maxSize; 
        private Quaternion m_newRotation = new Quaternion();


		[SerializeField] private int m_nbBoxesForMaxArrowSpeed;



		// Update is called once per frame
		void Update()
        {
            float distance = Vector3.Distance(m_compassTarget.position, this.transform.position);
    
            //Calculate remaining time value
            Timer timer = Timer.Instance;
			float gameAdvancementPercent = (timer.GetTimeAtStart() - timer.GetTimeLeft()) / timer.GetTimeAtStart();
           
            //Calculate boxes count
            float boxCountPercent = m_boxSystem.GetBoxCount() / (float)m_nbBoxesForMaxArrowSpeed;
			//print("Box count percent: " + boxCountPercent);
			float totalCompassPercentEmergency = Mathf.Clamp((gameAdvancementPercent + boxCountPercent) /2, 0,1);

            if(distance > m_minDistanceToShowCompass)
            {
				m_arrowMesh.SetActive(true);
				m_text.SetActive(true);
                foreach(GameObject txt in m_checkoutTextList)
                {
                    txt.SetActive(false);
                }
			}
            else
            {
                m_arrowMesh.SetActive(false);
				m_text.SetActive(false);
				foreach (GameObject txt in m_checkoutTextList)
				{
					txt.SetActive(true);
				}
			}

			m_animator.SetFloat("ArrowSpeed", totalCompassPercentEmergency);
			m_textAnimator.SetFloat("TextSpeed", totalCompassPercentEmergency);

		}
    }
}
