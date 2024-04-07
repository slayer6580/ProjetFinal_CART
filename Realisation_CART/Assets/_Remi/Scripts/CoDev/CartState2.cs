namespace CartControl
{
	public class CartState2 : IState
	{
		protected CartStateMachine2 m_cartStateMachine;

		public virtual void OnStart(CartStateMachine2 cartStateMachine)
		{
			m_cartStateMachine = cartStateMachine;
		}

		public virtual void OnEnter()
		{
			throw new System.NotImplementedException();
		}

		public virtual void OnUpdate()
		{
			throw new System.NotImplementedException();
		}

		public virtual void OnFixedUpdate()
		{
			throw new System.NotImplementedException();
		}


		public virtual void OnExit()
		{
			throw new System.NotImplementedException();
		}

		public virtual bool CanEnter(IState currentState)
		{
			throw new System.NotImplementedException();
		}

		public virtual bool CanExit()
		{
			throw new System.NotImplementedException();
		}

	}
}
