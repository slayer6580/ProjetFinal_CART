using UnityEngine;

namespace BehaviourTree
{
	public class AdjustRotation : LeafNode
	{
		[Range(0.5f, 10)] public float m_steeringSpeed = 1;
		[Range(0, 89)] public float m_steerForce;
		private const float MAX_STEERING = 5;
		private float m_steerValue = 0;

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			Vector3 targetDir = new Vector3(m_blackboard.m_target.x,
											m_blackboard.m_thisClient.transform.position.y,
											m_blackboard.m_target.z) - m_blackboard.m_thisClient.transform.position;

			Vector3 forward = m_blackboard.m_thisClient.transform.forward;
			float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
			m_blackboard.m_targetAngle = angle;

			float steerPercent = (Mathf.Abs(angle) / (90 - m_steerForce));

			if (angle > 2)
			{
				m_steerValue = -steerPercent;
			}
			else if (angle < -2)
			{
				m_steerValue = steerPercent;
			}
			else
			{
				m_steerValue = 0;				
			}

			m_steerValue = Mathf.Clamp(m_steerValue, -MAX_STEERING, MAX_STEERING);

			
			m_blackboard.m_cartStateMachine.OnSteer(m_steerValue);

			return State.Success;
		}
	}
}
