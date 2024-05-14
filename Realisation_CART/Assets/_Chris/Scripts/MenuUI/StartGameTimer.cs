using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DiscountDelirium
{
    public class StartGameTimer : MonoBehaviour
    {
        public static Action OnStartingTimeEnded;

        [SerializeField] private TextMeshProUGUI m_timeText;
        [SerializeField] private float m_startingTime;
        [SerializeField] private string m_messageAfterTimer;
        [SerializeField] private int m_messageDuration;

        private float m_timeLeft;
        private bool m_timerEnded;

        void Start()
        {
            m_timeLeft = m_startingTime;
        }

        void Update()
        {
            DecrementTimer();
            UpdateUI();
        }

        private void DecrementTimer()
        {
            if (m_timeLeft >= 0)
            {
                m_timeLeft -= Time.deltaTime;
            }

            if (m_timerEnded == false && m_timeLeft <= 0)
            {
                m_timerEnded = true;
                OnStartingTimeEnded.Invoke();
                StartCoroutine("ShowTextAfterTimer");
            }
        }

        private void UpdateUI()
        {
            if (!m_timerEnded) 
            {
                m_timeText.text = Mathf.Ceil(m_timeLeft).ToString();
            }
            
        }

        IEnumerator ShowTextAfterTimer() 
        {
            m_timeText.text = m_messageAfterTimer;
            yield return new WaitForSeconds(m_messageDuration);
            m_timeText.text = "";
        }
    }
}
