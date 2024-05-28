using Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class Timer : MonoBehaviour
    {
		public static Timer Instance { get; private set; }
		[SerializeField] private Image m_clockImage;
        [SerializeField] private TextMeshProUGUI m_timeText;
        private float m_timeAtStart;
        private float m_timeLeft;
        private bool m_isMatchStarted;
        private bool m_timerEnded;

        public static Action TimesUp;

        private void Awake()
        {
			if (Instance != null)
			{
				Debug.LogWarning("Timer already exists.");
				Destroy(gameObject);
				return;
			}

			Instance = this;
		}

        public float GetTimeAtStart()
        {
            return m_timeAtStart;
        }

		public float GetTimeLeft()
		{
			return m_timeLeft;
		}
		private void Start () 
        {
			m_timeAtStart = GameStateMachine.Instance.GameLength;
			GetReadyState.OnGameStarted += StartTimer;
			m_timeLeft = m_timeAtStart;
        }

        public void StartTimer() 
        {
            m_isMatchStarted = true;
            Debug.Log("Match Started");
        }

        private void Update()
        {
            DecrementTimer();
            UpdateUI();
        }

        private void DecrementTimer() 
        {
            if (m_isMatchStarted && m_timeLeft >= 0) 
            {
                m_timeLeft -= Time.deltaTime;
            }

            if (m_timerEnded == false && m_timeLeft <= 0) 
            {
                TimerEnd();
            }
        }

        private void UpdateUI()
        {
            m_clockImage.fillAmount = m_timeLeft/m_timeAtStart;
            m_timeText.text = Mathf.Ceil(m_timeLeft).ToString();
        }

        private void TimerEnd() 
        {
            m_timerEnded = true;
            TimesUp.Invoke();
        }
    }
}
