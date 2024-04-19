using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class WaitNode : LeafNode
	{
		public float duration = 1f;
		float startTime;

		protected override void OnStart()
		{
			startTime = Time.time;
		}

		protected override void OnStop()
		{
			
		}

		protected override State OnUpdate()
		{
			if (Time.time - startTime > duration)
			{
				Debug.Log("Back time:" + (Time.time - startTime));
				return State.Success;
			}
			return State.Running;
		}
	}
}
