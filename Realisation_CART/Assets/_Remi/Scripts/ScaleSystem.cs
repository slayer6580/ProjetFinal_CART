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
        [field: SerializeField] private Transform DoorOne { get; set; } = null;
        [field: SerializeField] private Transform DoorTwo { get; set; } = null;

        [SerializeField] private float m_weightToOpenDoor = 20;

        public UnityEvent<Collider, bool> m_onTriggerEnter;
        public UnityEvent<Collider, bool> m_onTriggerExit;

        private Vector3 m_newDiegeticScale = Vector3.zero;

        private const int PLAYER_BODY_LAYER = 3;
        private const float  PLAYER_BODY_WEIGHT = 150;
        private const float DIEGETIC_UI_THRESHOLD = 0.1f;
        private const float DOOR_ANGLE_THRESHOLD = 0.1f;
        private const int DIEGETIC_UI_MIN_SCALE = 0;
        private const int DIEGETIC_UI_MAX_SCALE = 20;
        private const int DOOR_ANGLE_MIN = 0;
        private const int DOOR_ANGLE_MAX = 90;

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

        private void HandleTriggerEnter(Collider other, bool isInsideBackstore)
        {
            if (other.gameObject.layer != PLAYER_BODY_LAYER) return;

            m_isInsideBackstore = isInsideBackstore;

            if (m_isPlayerInTriggerZone) return;
            m_isPlayerInTriggerZone = true;

            if (isInsideBackstore)
            {
                //Debug.Log("Player is inside backstore");
                m_isPlayerCanPass = true;
            }
            else
            {
                //Debug.Log("Player is outside backstore");
                m_isScaleSystemActive = true;
                CalculateWPlayerWeight();
                Debug.Log("Player weight: " + m_currentPlayerWeight);
            }
        }

        private void HandleTriggerExit(Collider other, bool isInsideBackstore)
        {

            if (other.gameObject.layer != PLAYER_BODY_LAYER) return;

            m_isInsideBackstore = isInsideBackstore;

            if (!m_isPlayerInTriggerZone) return;
            m_isPlayerInTriggerZone = false;

            Debug.Log("Exit collision with: " + other.gameObject.name);


            if (isInsideBackstore)
            {
                // The mechanism has to wait so that the doors do not close on the player
                // while he is leaving the trigger carpet when inside the backstore
                Invoke("CloseTheDoors", 3f);
            }
            else
            {
                CloseTheDoors();
            }
        }

        private void CloseTheDoors()
        {
            m_currentPlayerWeight = 0;
            m_isPlayerInTriggerZone = false;
            m_isPlayerCanPass = false;
        }

        private void CalculateWPlayerWeight()
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

        private void Update()
        {
            UpdateDiegeticScaleUI();
            UpdateDoorsRotation();
        }

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
                //Debug.Log("Player is on scale and weight is more or equal to m_weightToOpenDoor");
                m_newDiegeticScale.z = DIEGETIC_UI_MAX_SCALE;
            }

            DiegeticScaleValue.localScale = math.lerp(DiegeticScaleValue.localScale, m_newDiegeticScale, Time.deltaTime);

            if (DiegeticScaleValue.localScale.z >= DIEGETIC_UI_MAX_SCALE - DIEGETIC_UI_THRESHOLD)
            {
                m_isPlayerCanPass = true;
                Debug.Log("Player can pass");
            }

            //if (DiegeticScaleValue.localScale.z <= DIEGETIC_UI_MIN_SCALE + DIEGETIC_UI_SCALE_THRESHOLD && !m_isInsideBackstore)
            //{
            //    m_isDiegeticUIActive = false;
            //    Debug.Log("The backstore system can be deactivated");
            //}
        }

        private void UpdateDoorsRotation()
        {
            int doorAngle = DOOR_ANGLE_MIN;

            if (m_isPlayerCanPass == true)
            {
                doorAngle = DOOR_ANGLE_MAX;
                //Debug.Log("Doors are open");
            }
            
            DoorOne.rotation = Quaternion.Lerp(DoorOne.rotation, Quaternion.Euler(0, -doorAngle, 0), Time.deltaTime);
            DoorTwo.rotation = Quaternion.Lerp(DoorTwo.rotation, Quaternion.Euler(0, doorAngle, 0), Time.deltaTime);
            //Vector3 eulerAngles = DoorTwo.transform.eulerAngles;
            //Debug.Log("DoorTwo.rotation.y: " + eulerAngles.y);
            Debug.Log("The backstore system can be deactivated DoorTwo.rotation.y: " + DoorTwo.transform.eulerAngles.y + " <= " + (DOOR_ANGLE_MIN + DOOR_ANGLE_THRESHOLD));

            if (DoorTwo.transform.eulerAngles.y <= DOOR_ANGLE_MIN + DOOR_ANGLE_THRESHOLD && !m_isInsideBackstore)
            {
                m_isScaleSystemActive = false;
                Debug.Log("The backstore system can be deactivated DoorTwo.rotation.y: " + DoorTwo.transform.eulerAngles.y + " <= " + (DOOR_ANGLE_MIN + DOOR_ANGLE_THRESHOLD));
            }
            else if (DoorTwo.transform.eulerAngles.y >= DOOR_ANGLE_MIN - DOOR_ANGLE_THRESHOLD)
            {
                m_isScaleSystemActive = true;
                Debug.Log("The backstore system is active");
            }
        }

        public bool GetIsScaleSystemActive()
        {
            return m_isScaleSystemActive;
        }
    }
}
