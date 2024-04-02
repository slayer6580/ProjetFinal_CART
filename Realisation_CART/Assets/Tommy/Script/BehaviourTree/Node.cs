using UnityEngine;

namespace BehaviourTree
{
    public abstract class Node : ScriptableObject
    {
		public enum State
		{
			Running,
			Failure,
			Success
		}

		[HideInInspector] public State m_state = State.Running;
		[HideInInspector] public bool m_started = false;
		[HideInInspector] public string m_guid;   //An Id that will be assign when creating a node
		public Vector2 m_position;
		[HideInInspector] public Blackboard m_Blackboard;
        //[HideInInspector] public AiAgent m_agent;
        [TextArea] public string m_description;

        
		public State Update()
        {
            if (!m_started)
            {
                OnStart();
                m_started = true;
            }
            
            m_state = OnUpdate();

            if(m_state == State.Failure || m_state == State.Success)
            {
                OnStop();
                m_started = false;
            }

            return m_state;
        }

		//Instantiate this so we can have multiple instance of the same BehaviourTree
		public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}
