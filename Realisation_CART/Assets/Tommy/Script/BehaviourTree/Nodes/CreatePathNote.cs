using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
	public class CreatePathNote : LeafNode
	{
		public GameObject m_debugBox;
		private int m_lastRandomTarget;

		protected override void OnStart()
		{
			

		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			int randomTarget = 0;

			//If the premade path is clear
			if(m_blackboard.m_chosenPathListCopy.Count == 0)
			{
				//Find a path (a list of point) different from the last one (if there's more than one)
				while (true)
				{
					randomTarget = Random.Range(0, m_blackboard.m_possiblePathScript.ListOfPath.Count);
					if (randomTarget != m_lastRandomTarget)
					{
						m_lastRandomTarget = randomTarget;
						break;
					}

					if (m_blackboard.m_possiblePathScript.ListOfPath.Count < 2)
					{
						break;
					}
				}

				//Copy the chosen list so we can progressivly remove some element (once reached)
				foreach (GameObject targetPath in m_blackboard.m_possiblePathScript.ListOfPath[randomTarget])
				{
					m_blackboard.m_chosenPathListCopy.Add(targetPath);
				}

			}

			//If the path made by the navMesh is clear
			if (m_blackboard.m_path.Count == 0)
			{
				if (m_blackboard.m_chosenPathListCopy.Count > 0)
				{
					m_blackboard.m_navAgent.SetDestination(m_blackboard.m_chosenPathListCopy[0].transform.position);
					for (int i = 1; i < m_blackboard.m_navAgent.path.corners.Length; i++)
					{
						m_blackboard.m_path.Add(m_blackboard.m_navAgent.path.corners[i]);
						GameObject debugBox = Instantiate(m_debugBox, m_blackboard.m_navAgent.path.corners[i], Quaternion.identity);
						m_blackboard.m_pathDebugBox.Add(debugBox);
					}

				}
			}

			if (m_blackboard.m_path.Count != m_blackboard.m_navAgent.path.corners.Length)
			{
				return State.Running;
			}
			

			return State.Success;
		}
	}
}
