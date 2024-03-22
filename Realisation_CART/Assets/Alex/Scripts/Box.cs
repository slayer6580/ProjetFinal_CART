using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BoxSetup))]

public class Box : MonoBehaviour
{


    struct MultiSlots // public TEST
    {
        public MultiSlots(List<int> slotIndexes)
        {
            m_slotIndexes = slotIndexes;
        }

        public List<int> m_slotIndexes;
    }

    [Serializable] // TEST
    public struct ItemInBox // public TEST
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
    [SerializeField] private float itemHeightOffset;

    public List<SlotInfo> m_slotsInfo = new List<SlotInfo>(); // public TEST
    public Stack<ItemInBox> m_itemsInBox = new Stack<ItemInBox>(); // public TEST
    private List<MultiSlots> m_doubleSlots = new List<MultiSlots>();
    private List<MultiSlots> m_fourSlots = new List<MultiSlots>();
    private int m_availableSlotsLeft;
    private Tower m_tower;


    public void AddSlotInList(Transform slotTransform)
    {
        m_slotsInfo.Add(new SlotInfo(slotTransform, true));
    }

    public void AddDoubleSlotInList(List<int> indexes)
    {
        m_doubleSlots.Add(new MultiSlots(indexes));
    }

    public void AddFourSlotInList(List<int> indexes)
    {
        m_fourSlots.Add(new MultiSlots(indexes));
    }

    public void InitAvailableSlots(int numberOfSlots)
    {
        m_availableSlotsLeft = numberOfSlots;
    }

    public bool CanTakeItemInsideBox(ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            return CanTakeSmallItem();
        }
        else
        {
            return CanTakeMultiSlotItem(size);
        }

    }

    private bool CanTakeSmallItem()
    {
        if (m_availableSlotsLeft > 0)
        {
            return true;
        }

        return false;
    }

    private bool CanTakeMultiSlotItem(ItemData.ESize size)
    {
        int sizeInInt = 0;
        if (size == ItemData.ESize.medium)
        {
            sizeInInt = 2;
        }
        else if (size == ItemData.ESize.large)
        {
            sizeInInt = 4;
        }

        if (m_availableSlotsLeft < sizeInInt)
        {
            Debug.Log("Not enough space for multiSlot item");
            return false;
        }

        if (CanPlaceItemBySize(size))
        {
            return true;
        }

        // TODO Try Reorganize box Here
        // TODO After Reorganize, try CanPlaceItRightAway again

        return false;
    }

    private void ReorganizeBox()
    {
        // TODO
    }


    private bool CanPlaceItemBySize(ItemData.ESize size)
    {
        if (size == ItemData.ESize.medium)
        {
            foreach (MultiSlots multiSlot in m_doubleSlots)
            {
                bool firstIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[0]].m_isAvailable;
                bool secondIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[1]].m_isAvailable;

                if (firstIndexIsAvailable && secondIndexIsAvailable)
                {
                    return true;
                }
            }
        }
        else if (size == ItemData.ESize.large)
        {
            foreach (MultiSlots multiSlot in m_fourSlots)
            {
                bool firstIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[0]].m_isAvailable;
                bool secondIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[1]].m_isAvailable;
                bool thirdIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[2]].m_isAvailable;
                bool fourthIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[3]].m_isAvailable;

                if ((firstIndexIsAvailable && secondIndexIsAvailable) && (thirdIndexIsAvailable && fourthIndexIsAvailable))
                {
                    return true;
                }
            }
        }

        Debug.Log("Can't place it in this box");
        return false;
    }


    public void TakeItem(GameObject item, ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            TakeSmallItem(item);
        }
        else if (size == ItemData.ESize.medium)
        {
            TakeMediumItem(item);
        }
        else
        {
            TakeLargeItem(item);
        }
    }

    private void TakeSmallItem(GameObject item)
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
                m_itemsInBox.Push(new ItemInBox(item, allIndex, slotTransform.localPosition));

                item.transform.SetParent(gameObject.transform);
                Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; index: " + allIndex[0] + "; localPosition: " + slotTransform.localPosition);
                item.transform.localPosition = slotTransform.localPosition + new Vector3(0, itemHeightOffset, 0); // TEST

                if (IsBoxFull())
                    m_tower.AddBoxToTower();

                return;
            }
        }
    }

    private void TakeMediumItem(GameObject item)
    {
        foreach (MultiSlots multiSlot in m_doubleSlots)
        {
            bool firstIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[0]].m_isAvailable;
            bool secondIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[1]].m_isAvailable;

            if (firstIndexIsAvailable && secondIndexIsAvailable)
            {
                m_availableSlotsLeft -= 2;
                int index1 = multiSlot.m_slotIndexes[0];
                int index2 = multiSlot.m_slotIndexes[1];
                Transform slotTransform1 = m_slotsInfo[index1].m_slotTransform;
                Transform slotTransform2 = m_slotsInfo[index2].m_slotTransform;
                List<Vector3> allLocalPositions = new List<Vector3>
                {
                   slotTransform1.localPosition,
                   slotTransform2.localPosition
                };
                m_slotsInfo[index1] = new SlotInfo(slotTransform1, false);
                m_slotsInfo[index2] = new SlotInfo(slotTransform2, false);
                Vector3 centralLocalPosition = CreateCentralSlotLocalPosition(allLocalPositions);
                List<int> allIndex = new List<int>
                {
                    index1,
                    index2
                };
                m_itemsInBox.Push(new ItemInBox(item, allIndex, centralLocalPosition));

                if (Mathf.Abs((index1 - index2)) != 1)
                { 
                    item.transform.localRotation = Quaternion.Euler(0,90,0);
                }
                item.transform.SetParent(gameObject.transform);

                Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; index: " + allIndex[0] + ", " + allIndex[1] + "; localPosition: " + centralLocalPosition);
                item.transform.localPosition = centralLocalPosition + new Vector3(0, itemHeightOffset, 0); // TEST

                if (IsBoxFull())
                    m_tower.AddBoxToTower();

                return;
            }
        }
    }

    private void TakeLargeItem(GameObject item)
    {
        foreach (MultiSlots multiSlot in m_fourSlots)
        {
            bool firstIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[0]].m_isAvailable;
            bool secondIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[1]].m_isAvailable;
            bool thirdIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[2]].m_isAvailable;
            bool fourthIndexIsAvailable = m_slotsInfo[multiSlot.m_slotIndexes[3]].m_isAvailable;

            if ((firstIndexIsAvailable && secondIndexIsAvailable) && (thirdIndexIsAvailable && fourthIndexIsAvailable))
            {
                m_availableSlotsLeft -= 4;
                int index1 = multiSlot.m_slotIndexes[0];
                int index2 = multiSlot.m_slotIndexes[1];
                int index3 = multiSlot.m_slotIndexes[2];
                int index4 = multiSlot.m_slotIndexes[3];
                Transform slotTransform1 = m_slotsInfo[index1].m_slotTransform;
                Transform slotTransform2 = m_slotsInfo[index2].m_slotTransform;
                Transform slotTransform3 = m_slotsInfo[index3].m_slotTransform;
                Transform slotTransform4 = m_slotsInfo[index4].m_slotTransform;
                List<Vector3> allLocalPositions = new List<Vector3>
                {
                   slotTransform1.localPosition,
                   slotTransform2.localPosition,
                   slotTransform3.localPosition,
                   slotTransform4.localPosition
                };
                m_slotsInfo[index1] = new SlotInfo(slotTransform1, false);
                m_slotsInfo[index2] = new SlotInfo(slotTransform2, false);
                m_slotsInfo[index3] = new SlotInfo(slotTransform3, false);
                m_slotsInfo[index4] = new SlotInfo(slotTransform4, false);
                Vector3 centralLocalPosition = CreateCentralSlotLocalPosition(allLocalPositions);
                List<int> allIndex = new List<int>
                {
                    index1,
                    index2,
                    index3,
                    index4
                };
                m_itemsInBox.Push(new ItemInBox(item, allIndex, centralLocalPosition));

                item.transform.SetParent(gameObject.transform);
                Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; index: " + allIndex[0] + ", " + allIndex[1] + ", " + allIndex[2] + ", " + allIndex[3] + "; localPosition: " + centralLocalPosition);
                item.transform.localPosition = centralLocalPosition + new Vector3(0, itemHeightOffset, 0) ; // TEST

                if (IsBoxFull())
                    m_tower.AddBoxToTower();

                return;
            }
        }
    }

    private Vector3 CreateCentralSlotLocalPosition(List<Vector3> localPositions)
    {
        Vector3 centralLocalPosition = Vector3.zero;
        int nbOfPositions = localPositions.Count;

        foreach (Vector3 localPosition in localPositions)
        {
            centralLocalPosition += localPosition;
        }

        return centralLocalPosition / nbOfPositions;
    }

    private bool IsBoxFull()
    {
        if (m_availableSlotsLeft == 0)
        {
            Debug.Log("Item picked up just fill the box");
            return true;
        }

        return false;
    }

    public void SetTower(Tower tower)
    {
        m_tower = tower;
    }


}
