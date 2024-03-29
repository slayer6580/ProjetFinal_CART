using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : IState
{
	protected List<T> m_possibleStates = new List<T>();
	protected T m_currentState;

	protected virtual void Start()
	{
		CreatePossibleStateList();
		m_currentState = m_possibleStates[0];
	}

	protected virtual void CreatePossibleStateList()
	{

	}

	protected virtual void Update()
	{
		m_currentState.OnUpdate();
		TryToChangeState();
	}

	protected virtual void FixedUpdate()
	{

	}

	protected virtual void TryToChangeState()
	{
		foreach (var state in m_possibleStates)
		{
			if (m_currentState.Equals(state))
			{
				continue;
			}

			if (m_currentState.CanExit() && state.CanEnter(m_currentState))
			{
				m_currentState.OnExit();
				m_currentState = state;
				m_currentState.OnEnter();
				return;
			}
		}
	}
}
