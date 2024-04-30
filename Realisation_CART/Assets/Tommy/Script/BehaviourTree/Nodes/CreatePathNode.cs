using BehaviourTree;
using BoxSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DiscountDelirium
{
	public class CreatePathNode : LeafNode
	{	
		public GameObject m_debugBox;
		public NavMeshPath m_creatingPath;
		List<float> m_pathScores = new List<float>();
		int bestPath = 0;
		
		protected override void OnStart()
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

		protected override void OnStop()
		{
		}
		
		public void EvaluateBestPath()
		{
			bestPath = 0;
			int loopIteration = 0;
			int m_pathCount = 0;
			m_pathScores.Clear();

			if (m_blackboard == null) Debug.LogError("BlackBoard not found");
			if (m_blackboard.m_possiblePathScript == null) Debug.LogError("ClientPathList not found");
			foreach (List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath)
			{
				m_pathScores.Add(0);
				m_pathCount++;
			}


			//SCORING METHODS HERE
			ScoreForActiveShelf();
			///

			//Choose a path, the bigger the score, the bigger the chance to pick it
			float total = 0;
			foreach(float score in m_pathScores)
			{
				total += score;
			}		
			int chosenPath = Random.Range(0, (int)total);
			foreach (float score in m_pathScores)
			{
				chosenPath -= (int)score;
				if(chosenPath<=0) 
				{
					bestPath = loopIteration;
					break;
				}
				loopIteration++;
			}
		
		}

		private void ScoreForActiveShelf()
		{
			int loopIteration = 0;
			//Find percentage of active shelves in path
			foreach (List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath)
			{
				float numberOfShelfInPath = 0;
				float shelfActive = 0;
				foreach (GameObject pathPoint in pathList)
				{
					//Find total number of shelves
					ClientPathHelper pathHelper = pathPoint.gameObject.GetComponent<ClientPathHelper>();
					numberOfShelfInPath += pathHelper.m_closeShelfList.Count;

					//Find how many are active
					foreach (Shelf shelf in pathHelper.m_closeShelfList)
					{
						if (shelf.CanTakeItem())
						{
							shelfActive++;
						}
					}
				}

				//Calculate a score depending of client stats.
				float pathScorePercent = shelfActive / numberOfShelfInPath * 100;
				pathScorePercent = Mathf.Exp(pathScorePercent * m_blackboard.m_wantMostActiveShelves/100);
				
				//Add the score to the list of path score
				m_pathScores[loopIteration] = pathScorePercent;
				loopIteration++;
			}
		}

		private void CopyChosenPath()
		{
			//Get closest entry point
			float firstPointDistance = Vector3.Distance(m_blackboard.m_thisClient.transform.position,
														m_blackboard.m_possiblePathScript.ListOfPath[bestPath][0].transform.position);
			float lastPointDistance = Vector3.Distance(m_blackboard.m_thisClient.transform.position,
														m_blackboard.m_possiblePathScript.ListOfPath[bestPath][m_blackboard.m_possiblePathScript.ListOfPath[bestPath].Count-1].transform.position);

			//Copy the chosen list so we can progressivly remove some element (once reached)
			if(firstPointDistance < lastPointDistance)
			{
				foreach (GameObject targetPath in m_blackboard.m_possiblePathScript.ListOfPath[bestPath])
				{
					m_blackboard.m_chosenPathListCopy.Add(targetPath);
				}
			}
			else
			{
				foreach (GameObject targetPath in m_blackboard.m_possiblePathScript.ListOfPath[bestPath])
				{
					m_blackboard.m_chosenPathListCopy.Insert(0,targetPath);
				}
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
			}
		}
	}
}
