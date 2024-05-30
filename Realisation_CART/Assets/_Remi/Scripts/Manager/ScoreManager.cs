using System.Collections.Generic;
using UnityEngine;
using BoxSystem;
using Box = BoxSystem.Box;
using DiscountDelirium;
using static Manager.AudioManager;

namespace Manager
{
    public class ScoreManager : MonoBehaviour
    {
        [field: SerializeField] public TowerBoxSystem _TowerBoxSystem { get; private set; }

        public static ScoreManager _ScoreManager { get; private set; }

        [field: SerializeField] private int m_cartokenValueMultiplier = 1;

        private bool m_isCheckingOut;
        private List<Box> m_boxList = new List<Box>();
        private Transform m_counterPos;
        private float m_checkoutTimer;
        private int m_boxCheckoutActivated;
        private int m_boxCheckoutIsDone;
        private float m_fallingBoxOverTime;
		private CheckoutZone m_currentCheckoutZone;
        private List<GameObject> m_bonusText = new List<GameObject>();
        private const float BOX_FALL_HEIGHT = 3;

        [Header("Checkout Settings")]
        [field: SerializeField] private float m_timeBetweenBoxFall;
        [field: SerializeField] private float m_restTimeAfterAllBoxOnCounter;
        [field: SerializeField] private float m_showBoxGoesUpTime;
        private bool m_checkOutEnding = false;


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

        public void Update()
        {
            if (m_isCheckingOut)
            {
                m_checkoutTimer += Time.deltaTime;

                for (int i = 0; i < m_boxList.Count; i++)
                {
                    //Do only once by box
                    if (m_boxCheckoutActivated <= i && m_checkoutTimer > m_timeBetweenBoxFall * i)
                    {
                        m_boxList[i].transform.position = new Vector3(m_counterPos.position.x, m_counterPos.position.y + BOX_FALL_HEIGHT, m_counterPos.position.z);
                        m_boxList[i].transform.rotation = m_counterPos.transform.rotation;
                        m_boxList[i].GetComponent<Rigidbody>().isKinematic = false;
                        m_boxCheckoutActivated++;
                    }

                    //Once a box reach its desired position on the counter
                    if (m_boxList[i].transform.position.y <= m_counterPos.position.y + (i * BoxManager.GetInstance().BoxHeight))
                    {


                        //Stop it from falling
                        m_boxList[i].transform.position = new Vector3(m_counterPos.position.x, m_counterPos.position.y + (i * BoxManager.GetInstance().BoxHeight), m_counterPos.position.z);

                        //m_boxCheckoutIsDone is used to do this section only once by box
                        if (m_boxCheckoutIsDone <= i)
                        {
                            _AudioManager.PlaySoundEffectsOneShot(ESound.BoxDropOnCounter, transform.position);
                            _AudioManager.PlaySoundEffectsOneShot(ESound.BoxDropSpecial, transform.position, 1f, 0.5f + (i * 0.05f));

                            //Spawn a text to show obtained bonus
                            GameObject bonusText = Instantiate(m_currentCheckoutZone.BonusText, new Vector3(
                                                                                                m_currentCheckoutZone.BonusText.transform.position.x,
                                                                                                m_currentCheckoutZone.BonusText.transform.position.y + (i * BoxManager.GetInstance().BoxHeight),
                                                                                                m_currentCheckoutZone.BonusText.transform.position.z), m_currentCheckoutZone.BonusText.transform.rotation, m_currentCheckoutZone.transform);
                            m_bonusText.Add(bonusText);

		                           
                            bonusText.GetComponent<TextMesh>().text = "+" + (1+(i*2)).ToString();
                            bonusText.SetActive(true);
                            m_boxCheckoutIsDone++;

                            //When all boxes are on the counter
                            if (m_boxCheckoutIsDone >= m_boxList.Count)
                            {
                                //Get the time to manage next step of the animation
                                m_fallingBoxOverTime = m_checkoutTimer;
                                //Stop the camera animation
                                m_currentCheckoutZone.ThisAnimatior.speed = 0;

                            }
                        }
                    }
                }

                if (m_checkoutTimer > m_fallingBoxOverTime + m_restTimeAfterAllBoxOnCounter)
                {

                    if (m_checkOutEnding == false)
                    {
                        _AudioManager.PlaySoundEffectsOneShot(ESound.CashRegister, transform.position, 1f);
                        m_checkOutEnding = true;
                        foreach (Box box in m_boxList)
                        {
                            m_currentCheckoutZone.CashExplosion(box.transform.position);
                        }
                    }

                    foreach (Box box in m_boxList)
                    {
                        box.transform.Translate(Vector3.up * 0.5f);
                    }

                    //Reset everything and stop checkout
                    if (m_checkoutTimer > m_fallingBoxOverTime + m_restTimeAfterAllBoxOnCounter + m_showBoxGoesUpTime)
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

        public Vector3 EmptyCartAndGetScoreV2(Transform counterPos, CheckoutZone checkZone)
        {
            m_currentCheckoutZone = checkZone;
            m_checkOutEnding = false;
            m_counterPos = counterPos;
            m_checkoutTimer = 0;
            m_boxCheckoutActivated = 0;
            m_boxCheckoutIsDone = 0;
            m_fallingBoxOverTime = 1000;
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
                m_boxList[i].transform.position = new Vector3(m_counterPos.position.x, m_counterPos.position.y + 3 + i, m_counterPos.position.z);
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

        /// <summary> Deselect all button for better UI button interaction </summary>
        public void DeselectAllButtons()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }

        private int CalculateCartokens()
        {
            return (int)(Mathf.Pow(_TowerBoxSystem.GetBoxCount(), m_cartokenValueMultiplier));
        }
    }
}
