using System;
using UnityEngine;

namespace DiscountDelirium
{
    public class GetReadyState : GameState
    {
        public static Action OnPlayerReady;
        public static Action OnGameStarted;

        private bool m_playerReady = false;

        public override void OnEnter()
        {
            Debug.LogWarning("GameState : GetReady");
            MainMenuInputHandler.SelectEvent += PlayerReady;
            StartGameTimer.OnStartingTimeEnded += StartGame;
            m_gameStateMachine.m_playerSM.IsPaused = true;
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.LogWarning("GameState Exit : GetReady");
        }

        public override bool CanEnter(IState currentState)
        {
            return !m_gameStateMachine.m_isGameStarted;
        }

        public override bool CanExit()
        {
            return m_gameStateMachine.m_isGameStarted;
        }

        private void StartGame()
        {
            OnGameStarted?.Invoke();
            m_gameStateMachine.m_playerSM.IsPaused = false;
            m_gameStateMachine.m_isGameStarted = true;
        }

        private void PlayerReady() 
        {
            if (!m_playerReady) //to delete eventually
            {
                OnPlayerReady?.Invoke();
                m_playerReady = true;
            }
        }
    }
}
