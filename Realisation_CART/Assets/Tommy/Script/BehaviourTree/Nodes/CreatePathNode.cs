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
		public List<float> m_pathScores = new List<float>();
		public List<float> m_allClosestDistance = new List<float>();

		private int bestPath = 0;
		private float m_totalScore;
		private int m_chosenPath;
		private float m_pathScorePercent;
		private ClientPathHelper m_pathHelper;
		private float m_numberOfShelfInPath;
		private float m_shelfActive;
		
		
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
			m_pathScores.Clear();

			foreach (List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath)
			{
				m_pathScores.Add(0);
			}


			//SCORING METHODS HERE
			ScoreForActiveShelf();
			ScoreForCloseShelf();
			///

			//Choose a path, the bigger the score, the bigger the chance to pick it
			m_totalScore = 0;
			foreach(float score in m_pathScores)
			{
				m_totalScore += score;
			}		
			m_chosenPath = Random.Range(0, (int)m_totalScore);
			foreach (float score in m_pathScores)
			{
				m_chosenPath -= (int)score;
				if(m_chosenPath<=0) 
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
				m_numberOfShelfInPath = 0;
				m_shelfActive = 0;
				foreach (GameObject pathPoint in pathList)
				{
					//Find total number of shelves
					m_pathHelper = pathPoint.gameObject.GetComponent<ClientPathHelper>();
					m_numberOfShelfInPath += m_pathHelper.m_closeShelfList.Count;

					//Find how many are active
					foreach (Shelf shelf in m_pathHelper.m_closeShelfList)
					{
						if (shelf.CanTakeItem())
						{
							m_shelfActive++;
						}
					}
				}

				//Calculate a score depending of client stats.
				m_pathScorePercent = m_shelfActive / Mathf.Clamp(m_numberOfShelfInPath, 1,float.PositiveInfinity) * 100;
				m_pathScorePercent = Mathf.Exp(m_pathScorePercent * m_blackboard.m_wantMostActiveShelves/100);
			
				//Add the score to the list of path score
				m_pathScores[loopIteration] += m_pathScorePercent;
				loopIteration++;
			}
		}

		
		private void ScoreForCloseShelf()
		{
			
			float minDistance = Mathf.Infinity;
			float maxDistance = Mathf.NegativeInfinity;
			

			foreach (List<GameObject> pathList in m_blackboard.m_possiblePathScript.ListOfPath)
			{
				float closestEntryInThisPath = Vector3.Distance(m_blackboard.m_thisClient.transform.position, pathList[0].transform.position);
				float reversePathEntryDist = Vector3.Distance(m_blackboard.m_thisClient.transform.position, pathList[pathList.Count-1].transform.position);

				if(reversePathEntryDist < closestEntryInThisPath)
				{
					closestEntryInThisPath = reversePathEntryDist;
				}
				if (minDistance > closestEntryInThisPath)
				{
					minDistance = closestEntryInThisPath;
				}
				if (maxDistance < closestEntryInThisPath)
				{
					maxDistance = closestEntryInThisPath;
				}

				m_allClosestDistance.Add(closestEntryInThisPath);		
			}

			int loopIteration = 0;
			foreach (float distance in m_allClosestDistance)
			{
				//Calculate a score depending of client stats.
				m_pathScorePercent = 100 - ((distance - minDistance) * 100) / (maxDistance - minDistance);
				m_pathScorePercent = Mathf.Exp(m_pathScorePercent * m_blackboard.m_wantClosestPath / 100);

				//Add the score to the list of path score
				m_pathScores[loopIteration] += m_pathScorePercent;
				loopIteration++;
			}

			m_allClosestDistance.Clear();
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
				NavMesh.CalculatePath(m_blackboard.m_thisClient.transform.position,
										new Vector3(m_blackboard.m_chosenPathListCopy[0].transform.position.x,
													m_blackboard.m_thisClient.transform.position.y,
													m_blackboard.m_chosenPathListCopy[0].transform.position.z),
										NavMesh.AllAreas, m_creatingPath);

				if (m_creatingPath.status == NavMeshPathStatus.PathComplete)
				{
					for (int i = 1; i < m_creatingPath.corners.Length-1; i++)
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
