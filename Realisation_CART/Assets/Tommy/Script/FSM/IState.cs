using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
	public void OnEnter();
	public void OnUpdate();
	public void OnFixedUpdate();
	public void OnExit();
    public bool CanEnter(IState currentState);
    public bool CanExit();

    


}
