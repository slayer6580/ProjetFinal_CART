using Unity.VisualScripting;

namespace BehaviourTree
{
    public class DriftIfBigAngle : LeafNode
	{
		public float m_minAngleToStartDrifting;
		public float m_minAngleToStartBreaking;

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if (m_blackboard.m_targetAngle > m_minAngleToStartDrifting || m_blackboard.m_targetAngle < -m_minAngleToStartDrifting)
			{
				m_blackboard.m_cartStateMachine.OnDrift(1);
			}
			else
			{
				m_blackboard.m_cartStateMachine.OnDrift(0);
			}

			return State.Success;
		}
	}
}
