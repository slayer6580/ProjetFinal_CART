using CartControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class EndGameState : GameState
    {
        public static Action OnEndGame;
        public override void OnEnter()
        {
            Debug.LogWarning("GameState : EndGame");
            m_gameStateMachine.m_scoreUI.EnableUI(true);
            m_gameStateMachine.m_scoreUI.ShowScore(m_gameStateMachine.m_playerScore, m_gameStateMachine.m_nbItems, m_gameStateMachine.m_nbOfCartokens);
            m_gameStateMachine.m_playerSM.IsPaused = true;
            OnEndGame.Invoke();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnExit()
        {

        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is GameplayState)
            {
                return m_gameStateMachine.m_isGameOver;
            }
            return false;
        }

        public override bool CanExit()
        {
            return false;
        }

    }
}
