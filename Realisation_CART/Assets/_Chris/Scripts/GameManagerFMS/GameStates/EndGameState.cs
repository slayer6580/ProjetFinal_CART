using CartControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class EndGameState : GameState
    {
        public override void OnEnter()
        {
            Debug.LogWarning("GameState : EndGame");
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
