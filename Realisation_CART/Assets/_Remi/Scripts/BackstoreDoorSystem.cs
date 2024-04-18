using BoxSystem;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static BoxSystem.Box;
using static ItemData;
using Item = BoxSystem.Item;

namespace BackstoreScale
{
    public class BackstoreDoorSystem : MonoBehaviour
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
        private float  m_currentPlayerWeight = 0;
        private bool m_isPlayerOnScale = false;
        private bool m_isPlayerCanPass = false;

        private void Start()
        {
            m_onTriggerEnter.AddListener(HandleTriggerEnter);
            m_onTriggerExit.AddListener(HandleTriggerExit);
        }

        private void HandleTriggerEnter(Collider other, bool isInsideBackstore)
        {
            if (other.gameObject.layer != PLAYER_BODY_LAYER) return;
            if (m_isPlayerOnScale) return;

            //Debug.Log("Enter collision with: " + other.gameObject.name);

            m_isPlayerOnScale = true;

            if (isInsideBackstore)
            {
                Debug.Log("Player is inside backstore");
s                m_isPlayerCanPass = true;
            }
            else
            {
                Debug.Log("Player is outside backstore");
                CalculateWPlayerWeight();
            }
        }

        private void HandleTriggerExit(Collider other, bool isInsideBackstore)
        {
            if (other.gameObject.layer != PLAYER_BODY_LAYER) return;
            if (!m_isPlayerOnScale) return;

            Debug.Log("Exit collision with: " + other.gameObject.name);

            m_currentPlayerWeight = 0;
            m_isPlayerOnScale = false;
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

            if (m_isPlayerOnScale == false)
                m_newDiegeticScale.z = 0;
            else if (m_isPlayerOnScale && m_currentPlayerWeight < m_weightToOpenDoor)
                m_newDiegeticScale.z = m_currentPlayerWeight * 20 / m_weightToOpenDoor;
            else if (m_isPlayerOnScale && m_currentPlayerWeight > m_weightToOpenDoor)
                m_newDiegeticScale.z = 20;

            DiegeticScaleValue.localScale = math.lerp(DiegeticScaleValue.localScale, m_newDiegeticScale, Time.deltaTime);

            //Debug.Log("DiegeticScaleValue.localScale.z: " + DiegeticScaleValue.localScale.z);
            if (DiegeticScaleValue.localScale.z >= 20 - 1)
                m_isPlayerCanPass = true;
        }

        private void UpdateDoorsRotation()
        {
            int doorAngle = 0;

            if (m_isPlayerCanPass == true) doorAngle = 90;
            //if (m_isPlayerCanPass == false)
            //    Debug.Log("Is player can pass: " + m_isPlayerCanPass);
            DoorOne.rotation = Quaternion.Lerp(DoorOne.rotation, Quaternion.Euler(0, -doorAngle, 0), Time.deltaTime);
            DoorTwo.rotation = Quaternion.Lerp(DoorTwo.rotation, Quaternion.Euler(0, doorAngle, 0), Time.deltaTime);
        }
    }
}
