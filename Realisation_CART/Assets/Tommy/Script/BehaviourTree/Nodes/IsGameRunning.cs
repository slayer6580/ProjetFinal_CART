using DiscountDelirium;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class IsGameRunning : CompositeNode
	{
		GameStateMachine m_stateMachine;
		protected override void OnStart()
		{
			m_stateMachine = GameStateMachine.Instance;
		}

		protected override void OnStop()
		{
		
		}

		protected override State OnUpdate()
		{
			
			if (m_stateMachine.GetCurrentState() is EndGameState)
			{
				//Reset/Stop everthing
				m_blackboard.m_cartStateMachine.CartRB.velocity = Vector3.zero;
				m_blackboard.m_cartStateMachine.ForwardPressedPercent = 0;
				m_blackboard.m_cartStateMachine.BackwardPressedPercent = 0;
				m_blackboard.m_cartStateMachine.SteeringValue = 0;
				m_blackboard.m_cartStateMachine.HumanAnimCtrlr.SetTrigger("EndGame");
				m_blackboard.m_isGameStarted = false;

				if (m_children.Count > 1)
				{
					return m_children[1].Update();
				}
				return State.Failure;

			}
			else
			{
				m_children[0].Update();
				return State.Running;
				
			}


		}
	}
}
