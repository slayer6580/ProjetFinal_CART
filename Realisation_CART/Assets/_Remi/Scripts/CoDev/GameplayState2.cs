using CartControl;
using System;
using System.Collections;
using UnityEngine;

namespace DiscountDelirium
{
    public class GameplayState2 : GameState2
    {
        public static Action OnPlayerReady;
        public static Action OnGameStarted;

        private bool m_playerReady = false;

        public override void OnEnter()
        {
            Debug.LogWarning("GameState : GAMEPLAY");
            StartGameTimer.OnStartingTimeEnded += StartGame;
            Timer.TimesUp += GameOver;
            m_gameStateMachine.m_playerSM.IsPaused = true;
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !m_playerReady) //to delete eventually
            {
                Debug.Log("Player Ready");
                OnPlayerReady?.Invoke();
                m_playerReady = true;
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.LogWarning("GameState Exit : GAMEPLAY");
        }

        public override bool CanEnter(IState currentState)
        {
            return !m_gameStateMachine.m_isGameOver;
        }

        public override bool CanExit()
        {
            return m_gameStateMachine.m_isGameOver;
        }

        private void GameOver() 
        {
            m_gameStateMachine.m_isGameOver = true;
            Debug.Log("GameOver");
        }

        private void StartGame() 
        {
            OnGameStarted?.Invoke();
            m_gameStateMachine.m_playerSM.IsPaused = false;
        }
    }
}