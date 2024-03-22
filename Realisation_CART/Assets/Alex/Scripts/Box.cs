using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        m_boxSetup = GetComponent<BoxSetup>();
    }


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

    public bool CanPutItemInsideBox(ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
            return CanPutSmallItemInBox();

        else
            return CanPutMultiSlotItemInBox(size);
    }

    private bool CanPutSmallItemInBox()
    {
        if (m_availableSlotsLeft > 0)
            return true;

        return false;
    }

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
        // TODO After Reorganize, try CanPlaceItRightAway again

        Debug.Log("Can't place it in this box");
        return false;
    }

    private void ReorganizeBox()
    {
        // TODO
    }

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

        if (IsBoxFull())
            m_tower.AddBoxToTower();

    }

    private void PutSmallItemInBox(GameObject item)
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

                ItemSetParentInBox(item, slotTransform.localPosition);
                Debug.Log("(ItemInBox info) item: " + item.name + "; box: " + gameObject.name + "; index: " + allIndex[0] + "; localPosition: " + slotTransform.localPosition);
                return;
            }
        }
    }

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

    private void ItemSetParentInBox(GameObject item, Vector3 localPosition)
    {
        item.transform.SetParent(gameObject.transform);
        item.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0); // TEST
    }

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
