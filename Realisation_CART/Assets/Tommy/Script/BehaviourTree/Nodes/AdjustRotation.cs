using UnityEngine;

namespace BehaviourTree
{
	public class AdjustRotation : LeafNode
	{
		[Range(0.5f, 10)] public float m_steeringSpeed = 1;
		[Range(0, 89)] public float m_steerForce;
		private const float MAX_STEERING = 5;
		private float m_steerValue = 0;

		private Vector3 m_targetDir;
		private Vector3 m_forward;
		private float m_angle;
		private float m_steerPercent;
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{		
		}

		protected override State OnUpdate()
		{
			m_targetDir = new Vector3(m_blackboard.m_target.x,
											m_blackboard.m_thisClient.transform.position.y,
											m_blackboard.m_target.z) - m_blackboard.m_thisClient.transform.position;

			m_forward = m_blackboard.m_thisClient.transform.forward;
			m_angle = Vector3.SignedAngle(m_targetDir, m_forward, Vector3.up);
			m_blackboard.m_targetAngle = m_angle;

			m_steerPercent = (Mathf.Abs(m_angle) / (90 - m_steerForce));

			if (m_angle > 2)
			{
				m_steerValue = -m_steerPercent;
			}
			else if (m_angle < -2)
			{
				m_steerValue = m_steerPercent;
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
