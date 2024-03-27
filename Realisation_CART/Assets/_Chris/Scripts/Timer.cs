using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiscountDelirium
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Image m_clockImage;
        [SerializeField] private TextMeshProUGUI m_timeText;
        [SerializeField] private float m_timeAtStart;
        private float m_timeLeft;
        private bool m_isMatchStarted;
        private bool m_timerEnded;

        public static Action TimesUp;

        private void Awake()
        {
            GameplayState.OnGameStarted += StartTimer;
        }

        private void Start () 
        {
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
