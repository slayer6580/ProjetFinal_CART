using BoxSystem;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static BoxSystem.Box;
using static ItemData;
using Item = BoxSystem.Item;

namespace BackstoreSystems
{
    public class ScaleSystem : MonoBehaviour
    {
        [field: SerializeField] private TowerBoxSystem _TowerBoxSystem { get; set; } = null;
        [field: SerializeField] private Transform DiegeticScaleValue { get; set; } = null;
        [field: SerializeField] private GameObject Camera { get; set; } = null;
        [field: SerializeField] private Transform DoorOne { get; set; } = null;
        [field: SerializeField] private Transform DoorTwo { get; set; } = null;

        [SerializeField] private float m_weightingSpeed = 20;
        [SerializeField] private float m_doorsSpeed = 20;

        [SerializeField] private float m_weightToOpenDoor = 20;

        public UnityEvent<Collider, bool> m_onTriggerEnter;
        public UnityEvent<Collider, bool> m_onTriggerExit;

        private Vector3 m_newDiegeticScale = Vector3.zero;

        private const float  PLAYER_BODY_WEIGHT = 150;
        private const float DIEGETIC_UI_THRESHOLD = 0.1f;
        private const float DOOR_ANGLE_THRESHOLD = 0.1f;
        private const int DIEGETIC_UI_MIN_SCALE = 0;
        private const int DIEGETIC_UI_MAX_SCALE = 20;
        private const int DOOR_ANGLE_MIN = 0;
        private const int DOOR_ANGLE_MAX = -90;

        private float  m_currentPlayerWeight = 0;
        private bool m_isPlayerInTriggerZone = false;
        private bool m_isPlayerCanPass = false;
        private bool m_isScaleSystemActive = false;
        private bool m_isInsideBackstore = false;

        private void Start()
        {
            m_onTriggerEnter.AddListener(HandleTriggerEnter);
            m_onTriggerExit.AddListener(HandleTriggerExit);
        }

        private void Update()
        {
            UpdateDiegeticScaleUI();
            UpdateDoorsRotation();
        }

        /// <summary> Observer Pattern. Handles the player entering the trigger zone </summary>
        private void HandleTriggerEnter(Collider other, bool isInsideBackstore)
        {
            
            if (other.gameObject.layer != GameConstants.PLAYER_BODY) return;

            if (other.gameObject.transform.parent.parent.name != "Character") return;
			

			m_isInsideBackstore = isInsideBackstore;

            if (m_isPlayerInTriggerZone) return;
            m_isPlayerInTriggerZone = true;
			

			if (isInsideBackstore)
            {
                m_isPlayerCanPass = true;
            }
            else
            {
				Camera.SetActive(true);
				m_isScaleSystemActive = true;
                CalculatePlayerWeight();
            }
        }

        /// <summary> Observer Pattern. Handles the player exiting the trigger zone </summary>
        private void HandleTriggerExit(Collider other, bool isInsideBackstore)
        {

            if (other.gameObject.layer != GameConstants.PLAYER_BODY) return;

			if (other.gameObject.transform.parent.parent.name != "Character") return;
			Camera.SetActive(false);
			m_isInsideBackstore = isInsideBackstore;

            if (!m_isPlayerInTriggerZone) return;
            m_isPlayerInTriggerZone = false;
			

			if (isInsideBackstore)
            {
                // The mechanism has to wait so that the doors do not close on the player
                // while he is leaving the trigger carpet when inside the backstore
                Invoke("CloseTheDoors", 3f);
            }
            else
            {
				// CloseTheDoors();
				Invoke("CloseTheDoors", 2f);
			}
        }

        /// <summary> Resets the variables that keep the doors open (closes the doors) </summary>
        private void CloseTheDoors()
        {
            m_currentPlayerWeight = 0;
            m_isPlayerInTriggerZone = false;
            m_isPlayerCanPass = false;
        }

        /// <summary> Calculates the player's weight based on the items in the cart </summary>
        private void CalculatePlayerWeight()
        {
            ESize currentItemSize = ESize.small;
            m_currentPlayerWeight = PLAYER_BODY_WEIGHT; // TODO Remi: find a better place for the initial player weight

            List<Box> allBoxesInCart = _TowerBoxSystem.GetAllBoxes();

            foreach (Box box in allBoxesInCart)
            {
                List <ItemInBox> itemsInBox = box.GetItemsInBox();
                foreach (ItemInBox item in itemsInBox)
                {
                    currentItemSize = item.m_item.GetComponent<Item>().m_data.m_size; // TODO Remi: Ask Alex for access to Item to avoid usign GetComponent in game loop
                    
                    switch (currentItemSize)
                    {
                        case ESize.small:
                            m_currentPlayerWeight += 0.5f;
                            break;
                        case ESize.medium:
                            m_currentPlayerWeight += 2;
                            break;
                        case ESize.large:
                            m_currentPlayerWeight += 8;
                            break;
                    }
                }
            }
        }

        /// <summary> Updates the diegetic scale UI based on the player's weight </summary>
        private void UpdateDiegeticScaleUI()
        {
            m_newDiegeticScale = DiegeticScaleValue.localScale;

            if (m_isPlayerInTriggerZone == false)
            {
                //Debug.Log("Player is not on scale");
                m_newDiegeticScale.z = DIEGETIC_UI_MIN_SCALE;
            }
            else if (m_isPlayerInTriggerZone && m_currentPlayerWeight <= m_weightToOpenDoor)
            {
                //Debug.Log("Player is on scale and weight is less or equal to  m_weightToOpenDoor");
                m_newDiegeticScale.z = m_currentPlayerWeight * DIEGETIC_UI_MAX_SCALE / m_weightToOpenDoor;
            }
            else if (m_isPlayerInTriggerZone && m_currentPlayerWeight >= m_weightToOpenDoor)
            {
                m_newDiegeticScale.z = DIEGETIC_UI_MAX_SCALE;
            }

            DiegeticScaleValue.localScale = math.lerp(DiegeticScaleValue.localScale, m_newDiegeticScale, Time.deltaTime * m_weightingSpeed);

            if (DiegeticScaleValue.localScale.z >= DIEGETIC_UI_MAX_SCALE - DIEGETIC_UI_THRESHOLD)
            {
                m_isPlayerCanPass = true;
            }
        }

        /// <summary> Updates the doors rotation based on the player's weight </summary>
        private void UpdateDoorsRotation()
        {
            int doorAngle = DOOR_ANGLE_MIN;

            if (m_isPlayerCanPass == true)
            {
                doorAngle = DOOR_ANGLE_MAX;
            }


				DoorOne.rotation = Quaternion.Lerp(DoorOne.rotation, Quaternion.Euler(0, -doorAngle, 0), Time.deltaTime * m_doorsSpeed);
            DoorTwo.rotation = Quaternion.Lerp(DoorTwo.rotation, Quaternion.Euler(0, doorAngle, 0), Time.deltaTime * m_doorsSpeed);

            if (DoorTwo.transform.eulerAngles.y <= DOOR_ANGLE_MIN + DOOR_ANGLE_THRESHOLD && !m_isInsideBackstore)
            {
                m_isScaleSystemActive = false;
            }
            else if (DoorTwo.transform.eulerAngles.y >= DOOR_ANGLE_MIN - DOOR_ANGLE_THRESHOLD)
            {
                m_isScaleSystemActive = true;
            }
        }

        /// <summary> Returns true if the scale system is active </summary>
        public bool GetIsScaleSystemActive()
        {
            return m_isScaleSystemActive;
        }
    }
}
