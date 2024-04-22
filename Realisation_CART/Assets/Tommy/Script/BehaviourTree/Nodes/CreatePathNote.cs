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

		protected override void OnStart()
		{
			EvaluateBestPath();
			FindRandomPremadePath();
			CheckForPath();



		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{

			if (m_blackboard.m_chosenPathListCopy.Count == 0)
			{
				EvaluateBestPath();
				FindRandomPremadePath();
			}

			if (m_blackboard.m_path.Count == 0)
			{
				CheckForPath();
			}

			return State.Success;
		}


		private void FindRandomPremadePath()
		{
			int randomTarget = 0;

			//If the premade path is clear
			if (m_blackboard.m_chosenPathListCopy.Count == 0)
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
		}


		public void EvaluateBestPath()
		{
			for(int i =0;i < m_blackboard.m_possiblePathScript.ListOfPath.Count;i++)
			{

			}
			int numberOfShelfInPath = 0;
			int shelfActive = 0;
			foreach(List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath) 
			{ 
				foreach(GameObject pathPoint in pathList)
				{
					ClientPathHelper pathHelper = pathPoint.gameObject.GetComponent<ClientPathHelper>();
					numberOfShelfInPath += pathHelper.m_closeShelfList.Count;
					foreach(Shelf shelf in pathHelper.m_closeShelfList)
					{
						if (shelf.CanTakeItem())
						{
							shelfActive++;
						}
					}
				}
				Debug.LogWarning("nb of counted shelf: " + numberOfShelfInPath);
				Debug.LogWarning("nb of active shelf in this path: " + shelfActive);
				numberOfShelfInPath = 0;
				shelfActive = 0;
			}
		}




		private void CheckForPath()
		{

			//If the path made by the navMesh is clear
			if (m_blackboard.m_path.Count == 0)
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
}
