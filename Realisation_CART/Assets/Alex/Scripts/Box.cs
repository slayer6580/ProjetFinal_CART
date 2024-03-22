using System;
using System.Collections.Generic;
using UnityEngine;
using static BoxSetup;

[RequireComponent(typeof(BoxSetup))]

public class Box : MonoBehaviour
{

    [Serializable] // TEST
    public struct MultiSlots
    {
        public MultiSlots(List<int> slotIndexes)
        {
            m_slotIndexes = slotIndexes;
        }

        public List<int> m_slotIndexes;
    }

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
    public List<MultiSlots> m_doubleSlots = new List<MultiSlots>();
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

    public void InitAvailableSlots(int numberOfSlots)
    {
        m_availableSlotsLeft = numberOfSlots;
    }

    public void InitDoubleSlots(List<MultiSlots> doubleSlots)
    {
       m_doubleSlots.AddRange(doubleSlots);
    }

    public bool CanTakeItem(ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            return CanTakeSmallItem();
        }
        else
        {
            Debug.Log("TODO, NOT SUPPOSED TO HAPPEN");
            return false;
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

    

    public void TakeItem(GameObject item, ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            TakeSmallItem(item);
        }
        else
        {
            Debug.Log("TODO, NOT SUPPOSED TO HAPPEN");

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
                m_itemsInBox.Push(new ItemInBox(item, allIndex));
                Debug.Log("Added: '" + item.name + "' inside " + gameObject.name + " at slot: " + i);

                if (IsBoxFull())              
                    m_tower.AddBoxToTower();
                
                return;
            }
        }
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
