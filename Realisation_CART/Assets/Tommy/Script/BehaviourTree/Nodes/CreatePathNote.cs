using BehaviourTree;
using BoxSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace DiscountDelirium
{
	public class CreatePathNote : LeafNode
	{
		public GameObject m_debugBox;
		private int m_lastRandomTarget = -1;
		public NavMeshPath m_creatingPath;
		int bestPath = 0;

		protected override void OnStart()
		{
			EvaluateBestPath();
			CopyChosenPath();
			CreatePathWithNavMesh();
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			//If there's no more premade points to follow from the chosen list
			if (m_blackboard.m_chosenPathListCopy.Count == 0)
			{
				EvaluateBestPath();
				CopyChosenPath();
			}

			//If there's no more points made by the navMesh to help to reach the next premade point
			if (m_blackboard.m_path.Count == 0)
			{
				CreatePathWithNavMesh();
			}

			return State.Success;
		}


		public void EvaluateBestPath()
		{
			bestPath = 0;
			float bestPathScore = 0;
			int numberOfShelfInPath = 0;
			int shelfActive = 0;
			int loopIteration = 0;

			foreach (List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath)
			{
				foreach (GameObject pathPoint in pathList)
				{
					ClientPathHelper pathHelper = pathPoint.gameObject.GetComponent<ClientPathHelper>();
					numberOfShelfInPath += pathHelper.m_closeShelfList.Count;
					foreach (Shelf shelf in pathHelper.m_closeShelfList)
					{
						if (shelf.CanTakeItem())
						{
							shelfActive++;
						}
					}
				}

				float pathScore = shelfActive / numberOfShelfInPath * 100;
				if (pathScore > bestPathScore)
				{
					bestPathScore = pathScore;
					bestPath = loopIteration;
				}

				//Reset
				loopIteration++;
				numberOfShelfInPath = 0;
				shelfActive = 0;
			}

			
		}

		private void CopyChosenPath()
		{
			//Copy the chosen list so we can progressivly remove some element (once reached)
			foreach (GameObject targetPath in m_blackboard.m_possiblePathScript.ListOfPath[bestPath])
			{
				m_blackboard.m_chosenPathListCopy.Add(targetPath);
			}
		}
		
		private void CreatePathWithNavMesh()
		{
			if (m_blackboard.m_chosenPathListCopy.Count > 0)
			{
				m_creatingPath = new NavMeshPath();
				NavMesh.CalculatePath(m_blackboard.m_thisClient.transform.position, m_blackboard.m_chosenPathListCopy[0].transform.position, NavMesh.AllAreas, m_creatingPath);

				if (m_creatingPath.status == NavMeshPathStatus.PathComplete)
				{
					for (int i = 1; i < m_creatingPath.corners.Length; i++)
					{
						m_blackboard.m_path.Add(m_creatingPath.corners[i]);
						GameObject debugBox = Instantiate(m_debugBox, m_creatingPath.corners[i], Quaternion.identity);
						m_blackboard.m_pathDebugBox.Add(debugBox);
					}
					m_blackboard.m_path.Add(m_blackboard.m_chosenPathListCopy[0].transform.position);
					GameObject lastDebugBox = Instantiate(m_debugBox, m_blackboard.m_chosenPathListCopy[0].transform.position, Quaternion.identity);
					m_blackboard.m_pathDebugBox.Add(lastDebugBox);
				}
				else
				{
					Debug.Log("Path incomplete");
				}
			}
		}
	}
}
