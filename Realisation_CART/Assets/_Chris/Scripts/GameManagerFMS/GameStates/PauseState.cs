using System;
using UnityEngine;

namespace DiscountDelirium
{
    public class PauseState : GameState
    {
        public static Action OnPause;
        public static Action OnResume;
        public override void OnEnter()
        {
            Debug.LogWarning("GameState : PAUSE");
   
            OnPause?.Invoke();
            
            Time.timeScale = 0;
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {
            Debug.LogWarning("GameState Exit : PAUSE");

			Time.timeScale = 1;
			OnResume?.Invoke();
            
            
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is GameplayState)
            {
                return m_gameStateMachine.m_playerSM.IsPaused && m_gameStateMachine.m_isGameStarted && !m_gameStateMachine.IsCheckingOut;
            }
            return false;
        }

        public override bool CanExit()
        {
            return !m_gameStateMachine.m_playerSM.IsPaused;
        }
    }
}
