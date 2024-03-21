using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxSetup))]

public class Box : MonoBehaviour
{
    struct ItemInBox
    {  
        public ItemInBox(GameObject item, List<int> slotIndex)
        {
            m_item = item;
            m_slotIndex = slotIndex;
        }

        public GameObject m_item;
        public List<int> m_slotIndex;
    }

    struct SlotInfo
    {
        public SlotInfo(Transform slotPosition, bool isAvailable)
        {
            m_slotTransform = slotPosition;
            m_isAvailable = isAvailable;    
        }

        public Transform m_slotTransform;
        public bool m_isAvailable;
    }

    private List<SlotInfo> m_slotsInfo = new List<SlotInfo>();
    private Stack<ItemInBox> m_itemsInBox = new Stack<ItemInBox>();
    private int m_availableSlotsLeft;


    public void AddSlotInList(Transform slotTransform)
    {
        m_slotsInfo.Add(new SlotInfo(slotTransform, true));
    }

    public void InitAvailableSlots(int numberOfSlots)
    {
        m_availableSlotsLeft = numberOfSlots;
    }

    public bool CanTakeItem(ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            return CanTakeSmallItem();
        }
        else
        {
            Debug.Log("TODO");
            return false;
        }
    }

    private bool CanTakeSmallItem()
    {
        foreach (SlotInfo slotInfo in m_slotsInfo)
        {
            if (slotInfo.m_isAvailable == true)
                return true;
        }
        return false;

    }

    public void TakeItem(GameObject item, ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            TakeSmallItem(item);
        }
        else
        {
            Debug.Log("TODO");
          
        }
    }

    private void TakeSmallItem(GameObject item)
    {
        for (int i = 0; i < m_slotsInfo.Count; i++)
        {
            if (m_slotsInfo[i].m_isAvailable)
            {
                Transform slotTransform = m_slotsInfo[i].m_slotTransform;               
                m_slotsInfo[i] = new SlotInfo(slotTransform, false);
                List<int> allIndex = new List<int>();
                allIndex.Add(i);
                m_itemsInBox.Push(new ItemInBox(item, allIndex));
                Debug.Log("Added: '" + item.name + "' inside box number: " + i);
                return;
            }
        }
    }
}
