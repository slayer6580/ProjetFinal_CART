using System;
using UnityEngine;

namespace DiscountDelirium
{
    public class PauseState : GameState
    {
        public static Action OnPlayerReady;
        public override void OnEnter()
        {
            Debug.LogWarning("GameState : PAUSE");
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
        }

        public override bool CanEnter(IState currentState)
        {
            return m_gameStateMachine.IsPaused;
        }

        public override bool CanExit()
        {
            return !m_gameStateMachine.IsPaused;
        }
    }
}
