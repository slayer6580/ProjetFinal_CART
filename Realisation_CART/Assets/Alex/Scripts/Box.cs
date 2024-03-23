using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

namespace BoxSystem
{
    [RequireComponent(typeof(BoxSetup))]

    public class Box : MonoBehaviour
    {
        struct MultiSlots
        {
            public MultiSlots(List<int> slotIndexes)
            {
                m_slotIndexes = slotIndexes;
            }

            public List<int> m_slotIndexes;
        }

        public struct ItemInBox
        {
            public ItemInBox(GameObject item, List<int> slotIndex, Vector3 localPosition)
            {
                m_item = item;
                m_slotIndex = slotIndex;
                m_localPositionInsideBox = localPosition;
            }

            public GameObject m_item;
            public List<int> m_slotIndex;
            public Vector3 m_localPositionInsideBox;
        }

        [Serializable] // TEST
        public struct SlotInfo // public TEST
        {
            public SlotInfo(Transform slotPosition, bool isAvailable)
            {
                m_slotTransform = slotPosition;
                m_isAvailable = isAvailable;
            }

            public Transform m_slotTransform;
            public bool m_isAvailable;
        }

        public List<SlotInfo> m_slotsInfo = new List<SlotInfo>(); // public TEST
        private Stack<ItemInBox> m_itemsInBox = new Stack<ItemInBox>();
        private List<MultiSlots> m_doubleSlots = new List<MultiSlots>();
        private List<MultiSlots> m_fourSlots = new List<MultiSlots>();
        private int m_availableSlotsLeft;
        private Tower m_tower;
        private BoxSetup m_boxSetup;
        private const int MEDIUM_SIZE = 2;
        private const int LARGE_SIZE = 4;
        private int m_itemCount = 0;

        private void Awake()
        {
            m_boxSetup = GetComponent<BoxSetup>();
        }

        /// <summary> BoxSetup va nous donner les infos des slots </summary>
        public void AddSlotInList(Transform slotTransform)
        {
            m_slotsInfo.Add(new SlotInfo(slotTransform, true));
        }

        /// <summary> BoxSetup va nous donner les infos des slots double </summary>
        public void AddDoubleSlotInList(List<int> indexes)
        {
            m_doubleSlots.Add(new MultiSlots(indexes));
        }

        /// <summary> BoxSetup va nous donner les infos des slots de 4 </summary>
        public void AddFourSlotInList(List<int> indexes)
        {
            m_fourSlots.Add(new MultiSlots(indexes));
        }

        /// <summary> BoxSetup va nous donner le nombre de slot total </summary>
        public void InitAvailableSlots(int numberOfSlots)
        {
            m_availableSlotsLeft = numberOfSlots;
        }

        /// <summary> Regarde si on peut prendre l'objet selon sa taille </summary>
        public bool CanPutItemInsideBox(ItemData.ESize size)
        {
            if (size == ItemData.ESize.small)
                return CanPutSmallItemInBox();

            else
                return CanPutMultiSlotItemInBox(size);
        }

        /// <summary> Regarde si on peut prendre un petit objet </summary>
        private bool CanPutSmallItemInBox()
        {
            if (m_availableSlotsLeft > 0)
                return true;

            return false;
        }

        /// <summary> Regarde si on peut prendre un objet multi slot </summary>
        private bool CanPutMultiSlotItemInBox(ItemData.ESize size)
        {
            int sizeInt = 0;
            List<MultiSlots> multiSlotList = new List<MultiSlots>();

            if (size == ItemData.ESize.medium)
            {
                sizeInt = MEDIUM_SIZE;
                multiSlotList = m_doubleSlots;
            }
            else if (size == ItemData.ESize.large)
            {
                sizeInt = LARGE_SIZE;
                multiSlotList = m_fourSlots;
            }

            if (m_availableSlotsLeft < sizeInt)
                return false;

            foreach (MultiSlots multiSlot in multiSlotList)
            {
                List<bool> slotsAvailable = new List<bool>();
                for (int i = 0; i < sizeInt; i++)
                {
                    slotsAvailable.Add(m_slotsInfo[multiSlot.m_slotIndexes[i]].m_isAvailable);
                }

                if (AllSlotIsAvailable(slotsAvailable))
                {
                    return true;
                }
            }

            // TODO Try Reorganize box Here
            // TODO After Reorganize, try the foreach again

            Debug.Log("Can't place it in this box");
            return false;
        }

        /// <summary> Pour réorganiser la boite </summary>
        private void ReorganizeBox()
        {
            // TODO
        }

        /// <summary> Pour mettre l'objet dans la boite selon sa taille </summary>
        public void PutItemInBox(GameObject item, ItemData.ESize size)
        {
            switch (size)
            {
                case ItemData.ESize.small:
                    PutSmallItemInBox(item);
                    break;
                case ItemData.ESize.medium:
                    PutMultiSlotItemInBox(item, MEDIUM_SIZE, m_doubleSlots);
                    break;
                case ItemData.ESize.large:
                    PutMultiSlotItemInBox(item, LARGE_SIZE, m_fourSlots);
                    break;
                default:
                    break;
            }

            AddNewSrpingJoint(GetLastIndex());
            DeactivateLastSpring();

            if (IsBoxFull())
                m_tower.AddBoxToTower();
        }

        ///// <summary> Pour mettre un petit objet dans la boite </summary>
        //private void PutSmallItemInBox(GameObject item)
        //{
        //    for (int i = 0; i < m_slotsInfo.Count; i++)
        //    {
        //        if (m_slotsInfo[i].m_isAvailable)
        //        {
        //            m_itemCount++;
        //            m_availableSlotsLeft--;
        //            Transform slotTransform = m_slotsInfo[i].m_slotTransform;
        //            m_slotsInfo[i] = new SlotInfo(slotTransform, false);
        //            List<int> allIndex = new List<int>();
        //            allIndex.Add(i);
        //            //m_itemsInBox.Push(new ItemInBox(item, allIndex, slotTransform.localPosition));
        //            item.name = "Item " + m_itemCount;
        //            //item.transform.position = slotTransform.position + transform.position;
        //            ItemSetParentInBox(item, slotTransform.position, slotTransform.rotation);
        //            //Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; index: " + allIndex[0] + "; localPosition: " + slotTransform.localPosition);
        //            return;
        //        }
        //    }
        //}


        /// <summary> Pour mettre un petit objet dans la boite </summary>
        private void PutSmallItemInBox(GameObject GO)
        {
            for (int i = 0; i < m_slotsInfo.Count; i++)
            {
                if (m_slotsInfo[i].m_isAvailable)
                {
                    m_availableSlotsLeft--;
                    Transform slotTransform = m_slotsInfo[i].m_slotTransform;
                    m_slotsInfo[i] = new SlotInfo(slotTransform, false);
                    List<int> allIndex = new List<int>();
                    allIndex.Add(i);
                    m_itemsInBox.Push(new ItemInBox(GO, allIndex, slotTransform.localPosition));
                    ItemSetParentInBox(GO, slotTransform.localPosition);
                    return;
                }
            }
        }

        /// <summary> Pour mettre un objet multi slot dans la boite </summary>
        private void PutMultiSlotItemInBox(GameObject item, int size, List<MultiSlots> multiSlotsList)
        {
            foreach (MultiSlots multiSlot in multiSlotsList)
            {
                List<bool> slotsAvailable = new List<bool>();

                for (int i = 0; i < size; i++)
                {
                    slotsAvailable.Add(m_slotsInfo[multiSlot.m_slotIndexes[i]].m_isAvailable);
                }

                if (AllSlotIsAvailable(slotsAvailable))
                {
                    m_availableSlotsLeft -= size;
                    List<int> indexes = new List<int>();
                    List<Transform> transforms = new List<Transform>();
                    List<Vector3> allLocalPositions = new List<Vector3>();
                    for (int i = 0; i < size; i++)
                    {
                        indexes.Add(multiSlot.m_slotIndexes[i]);
                        transforms.Add(m_slotsInfo[indexes[i]].m_slotTransform);
                        allLocalPositions.Add(transforms[i].localPosition);
                        m_slotsInfo[indexes[i]] = new SlotInfo(transforms[i], false);
                    }

                    Vector3 localPosition = CreateLocalPositionForItem(allLocalPositions);
                    m_itemsInBox.Push(new ItemInBox(item, indexes, localPosition));

                    if (size == MEDIUM_SIZE && Mathf.Abs((indexes[0] - indexes[1])) != 1)
                    {
                        item.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }

                    ItemSetParentInBox(item, localPosition);

                    Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; localPosition: " + localPosition);


                    return;
                }
            }
        }

        /// <summary> Retourne l'index des derniers items sur le stack </summary>
        private int GetLastIndex()
        {
            if (m_itemCount > 1)
            {
                return 0;
            }
            else
            {
                return m_itemCount - 1;
            }
        }

        /// <summary> Desactive le string entre l'item ajoute precedemment et la boite</summary>
        private void DeactivateLastSpring()
        {
            if (m_itemCount > 1)
            {

                //Debug.Log("m_itemsInBox.ToArray()[0] " + m_itemsInBox.ToArray()[0].m_item.name);
                //Debug.Log("m_itemsInBox.ToArray()[1] " + m_itemsInBox.ToArray()[1].m_item.name);
                //Debug.Log("m_itemsInBox.ToArray()[GetLastIndex()] " + m_itemsInBox.ToArray()[GetLastIndex()].m_item.name);

                m_itemsInBox.ToArray()[0].m_item.transform.position = transform.position + m_itemsInBox.ToArray()[0].m_localPositionInsideBox + new Vector3(0, m_boxSetup.SlotHeight / 1.5f, 0);
                m_itemsInBox.ToArray()[1].m_item.transform.rotation = transform.rotation;
                m_itemsInBox.ToArray()[1].m_item.transform.position = transform.position + m_itemsInBox.ToArray()[1].m_localPositionInsideBox + new Vector3(0, m_boxSetup.SlotHeight / 1.5f, 0);

                SpringJoint springJoint = m_itemsInBox.ToArray()[1].m_item.GetComponent<SpringJoint>();
                if (springJoint != null)
                {
                    springJoint.connectedBody = null;
                    Destroy(springJoint);
                }

                Rigidbody rigidbody = m_itemsInBox.ToArray()[1].m_item.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                    Destroy(rigidbody);
                }

                Vector3 vector3 = transform.position + m_itemsInBox.ToArray()[1].m_localPositionInsideBox;
                vector3 += new Vector3(0, m_boxSetup.SlotHeight / 1.5f, 0);

                m_itemsInBox.ToArray()[1].m_item.transform.rotation = transform.rotation;
                m_itemsInBox.ToArray()[1].m_item.transform.position = new Vector3(vector3.x, vector3.y, vector3.z);
                //Debug.Log("Item location " + m_itemsInBox.ToArray()[1].m_item.transform.position);
            }

        }

        /// <summary> Ajoute et attache un joint de type spring entre le dernier item et la boite</summary>
        private void AddNewSrpingJoint(int lastIndex)
        {
            Debug.Log("AddNewSrpingJoint lastIndex: " + lastIndex);

            if (lastIndex < 0) return;

            GameObject itemInBox = m_itemsInBox.ToArray()[lastIndex].m_item;
            if (itemInBox == null)
            {
                Debug.LogWarning("lastIndex is out of range");
                return;
            }

            SpringJoint springJoint = itemInBox.AddComponent<SpringJoint>();
            springJoint.connectedBody = GetComponent<Rigidbody>();
            springJoint.spring = 5;
            springJoint.damper = 0;
            springJoint.minDistance = 0;
            springJoint.maxDistance = 0.2f;
            springJoint.tolerance = 0.06f;
            springJoint.enableCollision = true;
        }

        /// <summary> Place l'objet dans la hierarchie enfant de la boite </summary>
        private void ItemSetParentInBox(GameObject item, Vector3 localPosition, Quaternion localRotation)
        {
            item.transform.SetParent(gameObject.transform);
            ////Debug.Log("Parent name: " + item.transform.parent.name);
            //Vector3 globalLocation = item.transform.parent.position;
            ////Debug.Log("Local position: " + localPosition);
            //Debug.Log("globalLocation: " + globalLocation);
            ////Debug.Log("localRotation: " + localRotation);
            //Debug.Log("item.transform.parent.rotation: " + item.transform.parent.rotation);
            item.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0);
            ////item.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0); // TEST
            //item.transform.localPosition = localPosition; // TEST
            ////item.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0); // TEST
            //item.transform.localRotation = item.transform.parent.rotation;
        }

        /// <summary> Place l'objet dans la hierarchie enfant de la boite </summary>
        private void ItemSetParentInBox(GameObject item, Vector3 localPosition)
        {
            item.transform.SetParent(gameObject.transform);
            item.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0); // TEST
        }

        /// <summary> Regarde si une liste de bool est toute vrai </summary>
        private bool AllSlotIsAvailable(List<bool> slotsAvailable)
        {
            foreach (bool slotAvailable in slotsAvailable)
            {
                if (!slotAvailable)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary> Trouve l'emplacement locale des objets multi slot dans la boite </summary>
        private Vector3 CreateLocalPositionForItem(List<Vector3> slotLocalPositions)
        {
            Vector3 localposition = Vector3.zero;
            int nbOfPositions = slotLocalPositions.Count;

            foreach (Vector3 localPosition in slotLocalPositions)
            {
                localposition += localPosition;
            }

            return localposition / nbOfPositions;
        }

        /// <summary> Regarde si la boite es pleine </summary>
        private bool IsBoxFull()
        {
            if (m_availableSlotsLeft == 0)
            {
                Debug.Log("Item picked up just fill the box");
                return true;
            }

            return false;
        }

        /// <summary> Pour avoir une référence a la tour lors de sa création </summary>
        public void SetTower(Tower tower)
        {
            m_tower = tower;
        }

    }
}
