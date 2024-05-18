using System.Collections.Generic;
using UnityEngine;
using BoxSystem;
using Box = BoxSystem.Box;
using DiscountDelirium;
using Unity.VisualScripting;
using Cinemachine;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [field: SerializeField] public TowerBoxSystem _TowerBoxSystem { get; private set; }

        public static ScoreManager _ScoreManager { get; private set; }

        [field: SerializeField] private int m_cartokenValueMultiplier = 1;

        private bool m_isCheckingOut;
		private List<Box> m_boxList = new List<Box>();
        private Vector3 m_counterPos;
        private float m_checkoutTimer;
        private int m_boxCheckoutActivated;
        private int m_boxCheckoutIsDone;
		private float fallingBoxOverTime;
        private CheckoutZone m_currentCheckoutZone;
        private List<GameObject> m_bonusText = new List<GameObject>();
		private const float BOX_FALL_HEIGHT = 3;
		[Header("Checkout Settings")]
		public float m_timeBetweenBoxFall;
		public float m_restTimeAfterAllBoxOnCounter;
		public float m_showBoxGoesUpTime;


		private void Awake()
        {
            if (_ScoreManager != null)
            {
                Debug.LogWarning("ScoreManager already exists.");
                Destroy(gameObject);
                return;
            }

            _ScoreManager = this;
        }

        /// <summary> Vide le panier et rend le nombre d'items de la tour et le score total </summary>
        public Vector3 EmptyCartAndGetScore()
        {
            CalculateCartokens();

            int totalScore = 0;
            int nbOfItems = 0;
            int nbOfCartokens = CalculateCartokens();
            Debug.Log("Cartokens: " + nbOfCartokens);

            while (_TowerBoxSystem.GetBoxCount() > 0)
            {
                Box topBox = _TowerBoxSystem.GetTopBox();
                List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();

                for (int i = 0; i < itemsInBox.Count; i++)
                {
                    nbOfItems++;
                    totalScore += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
                }
                _TowerBoxSystem.RemoveBoxImpulse();
            }

            return new Vector3(nbOfItems, totalScore, nbOfCartokens);
        }

		public void Update()
		{
            if (m_isCheckingOut)
            {         
                m_checkoutTimer += Time.deltaTime;
               
				for (int i = 0; i < m_boxList.Count; i++)
				{
					//Do only once by box
					if (m_boxCheckoutActivated <= i && m_checkoutTimer > m_timeBetweenBoxFall * i )
                    {
						m_boxList[i].transform.position = new Vector3(m_counterPos.x, m_counterPos.y + BOX_FALL_HEIGHT, m_counterPos.z);
						m_boxList[i].GetComponent<Rigidbody>().isKinematic = false;
                        m_boxCheckoutActivated++;	
					}

					//Once a box reach its desired position on the counter
					if (m_boxList[i].transform.position.y <= m_counterPos.y + (i * BoxManager.GetInstance().BoxHeight))
                    {
						//Stop it from falling
						m_boxList[i].transform.position = new Vector3(m_counterPos.x, m_counterPos.y + (i * BoxManager.GetInstance().BoxHeight), m_counterPos.z);

						//m_boxCheckoutIsDone is used to do this section only once by box
						if (m_boxCheckoutIsDone <= i)
                        {
							//Spawn a text to show obtained bonus
							GameObject bonusText = Instantiate(m_currentCheckoutZone.BonusText, new Vector3(
                                                                                                m_currentCheckoutZone.BonusText.transform.position.x,
																								m_currentCheckoutZone.BonusText.transform.position.y + (i * BoxManager.GetInstance().BoxHeight),
																								m_currentCheckoutZone.BonusText.transform.position.z),m_currentCheckoutZone.BonusText.transform.rotation, m_currentCheckoutZone.transform);
							m_bonusText.Add(bonusText);
							bonusText.GetComponent<TextMesh>().text = "x" + Mathf.Pow(2,i).ToString();
							bonusText.SetActive(true);
							m_boxCheckoutIsDone++;

							//When all boxes are on the counter
							if (m_boxCheckoutIsDone >= m_boxList.Count)
							{
								//Get the time to manage next step of the animation
								fallingBoxOverTime = m_checkoutTimer;
								//Stop the camera animation
								m_currentCheckoutZone.ThisAnimatior.speed = 0;
								
							}
						}
					}				
				}

                if(m_checkoutTimer > fallingBoxOverTime + m_restTimeAfterAllBoxOnCounter)
                {

					foreach (Box box in m_boxList)
                    {
                        box.transform.Translate(Vector3.up * 2);
                    }

					//Reset everything and stop checkout
                    if(m_checkoutTimer > fallingBoxOverTime + m_restTimeAfterAllBoxOnCounter +  m_showBoxGoesUpTime)
                    {
						foreach (Box box in m_boxList)
						{
                            Destroy(box.gameObject);
						}
						
						foreach (GameObject text in m_bonusText)
						{
							Destroy(text);
						}

						m_boxList.Clear();
						m_bonusText.Clear();
						m_isCheckingOut = false;
						m_currentCheckoutZone.CheckoutDone();
					}        
				}
			}
		}

		public Vector3 EmptyCartAndGetScoreV2(Vector3 counterPos, CheckoutZone checkZone)
		{
            m_currentCheckoutZone = checkZone;
			m_counterPos = counterPos;
            m_checkoutTimer = 0;
            m_boxCheckoutActivated = 0;
            m_boxCheckoutIsDone = 0;
            fallingBoxOverTime = 1000;
			m_currentCheckoutZone.ThisAnimatior.speed = 1;
			m_boxList.Clear();

			CalculateCartokens();
			int totalScore = 0;
			int nbOfItems = 0;
			int nbOfCartokens = CalculateCartokens();
			Debug.Log("Cartokens: " + nbOfCartokens);

			while (_TowerBoxSystem.GetBoxCount() > 0)
			{			
				Box topBox = _TowerBoxSystem.GetTopBox();
				//Create a temporary box list for the checkout animation
				m_boxList.Insert(0, topBox);

				List<Box.ItemInBox> itemsInBox = topBox.GetItemsInBox();
				for (int i = 0; i < itemsInBox.Count; i++)
				{
					nbOfItems++;
					totalScore += itemsInBox[i].m_item.GetComponent<Item>().m_data.m_cost;
				}

				_TowerBoxSystem.RemoveBoxImpulse(false);
			}

			//Replace everybox in the sky over the counter
			for (int i = 0; i < m_boxList.Count; i++)
			{			
				m_boxList[i].transform.position = new Vector3(m_counterPos.x, m_counterPos.y + 3 + i, m_counterPos.z);
				m_boxList[i].GetComponent<Rigidbody>().isKinematic = true;
			}
			//m_isCheckingOut will activate the animation in Update()
			m_isCheckingOut = true;

			return new Vector3(nbOfItems, totalScore, nbOfCartokens);
		}

		public void RemoveAllBoxImpulse(TowerBoxSystem tower)
        {
			while (tower.GetBoxCount() > 0)
			{
				tower.RemoveBoxImpulse();
			}
		}

        private int CalculateCartokens()
        {
            return (int)(Mathf.Pow(_TowerBoxSystem.GetBoxCount(), m_cartokenValueMultiplier));  
        }
    }
}
