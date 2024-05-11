using BoxSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
	public class IsAttackWorthIt : CompositeNode
	{
		private List<float> m_attackScores = new List<float>();
		public bool m_chooseToAttack;
		public float m_minTimeBetweenAttack;
		private float m_lastLoopTime = 0;
		private int m_targetsBoxCount;
		private int m_boxCountDif;
		private float m_attackScore;


		protected override void OnStart()
		{
			m_chooseToAttack = false;
		}

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			m_blackboard.m_lastAttackTimer += Time.time - m_lastLoopTime;
			m_lastLoopTime = Time.time;

			if (m_blackboard.m_isAttacking)
			{
				return State.Success;
			}

			if (m_blackboard.m_clientInSight.Count > 0 && m_chooseToAttack == false && m_blackboard.m_lastAttackTimer > m_minTimeBetweenAttack)
			{

				m_attackScores.Clear();

				foreach (GameObject client in m_blackboard.m_clientInSight)
				{
					m_attackScores.Add(0);
				}

				///Attack condition here (all result must be between -100 and 100)
				CalculateTacticalAttack();
				///

				//Get best target
				float chosenTargetScore = Mathf.NegativeInfinity;
				int loopIteration = 0;
				foreach (float clientScore in m_attackScores)
				{
					if (clientScore > chosenTargetScore)
					{
						m_blackboard.m_chosenTarget = loopIteration;
						chosenTargetScore = clientScore;
					}
					loopIteration++;
				}


				int numberOfCondition = 1;  //CalculateTacticalAttack
				int numberOfConditionModifier = 100+ (100 * numberOfCondition);

				float randomChanceToAttack = Random.Range(m_blackboard.m_aggressiveness, m_blackboard.m_aggressiveness + numberOfConditionModifier);

				//Now that the target is chosen, do we attack?
				if (randomChanceToAttack > (numberOfConditionModifier - chosenTargetScore))
				{
					m_chooseToAttack = true;
					m_blackboard.m_currentPursuitStartTime += Time.time;
					return m_children[0].Update();
				}
				else
				{
					if (m_children.Count > 1)
					{
						return m_children[1].Update();
					}				
				}
			}
			else if (m_chooseToAttack)
			{
				return m_children[0].Update();
			}
			else if (m_children.Count > 1)
			{
				return m_children[1].Update();
			}

			return State.Success;
		}

		public void CalculateTacticalAttack()
		{
			int loopIteration = 0;
			
			foreach (GameObject target in m_blackboard.m_clientInSight)
			{
				if(target.transform.Find("Parent/Tower") == null)
				{
					Debug.LogWarning("NO TOWER FOUND");
				}
				m_targetsBoxCount = target.transform.Find("Parent/Tower").GetComponent<TowerBoxSystem>().GetBoxCount();

				//Calculate a score depending of client stats.
				m_boxCountDif = m_targetsBoxCount - m_blackboard.m_thisTower.GetBoxCount();
				m_attackScore = Mathf.Clamp((m_boxCountDif * m_blackboard.m_tacticalAttack) , - 100.0f,100.0f);

				//Add the score to the list of path score
				this.m_attackScores[loopIteration] = m_attackScore;
				loopIteration++;
			}
		}
	}
}
