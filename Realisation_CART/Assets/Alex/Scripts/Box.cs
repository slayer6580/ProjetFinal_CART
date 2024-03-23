using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BoxSystem.Box;

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

            public GameObject GetItem()
            {
                return m_item;
            }
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
        private int m_totalSlots;
        private int m_availableSlotsLeft;
        private Tower m_tower;
        private BoxSetup m_boxSetup;
        private const int MEDIUM_SIZE = 2;
        private const int LARGE_SIZE = 4;

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
            m_totalSlots = numberOfSlots;
            m_availableSlotsLeft = m_totalSlots;
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

        private bool CanPutMultiSlotItemInBox(ItemData.ESize size)
        {
            int sizeInt = size == ItemData.ESize.medium ? MEDIUM_SIZE : LARGE_SIZE;

            if (m_availableSlotsLeft < sizeInt)
                return false;

            return true;
        }

        /// <summary> Pour mettre l'objet dans la boite selon sa taille </summary>
        public void PutItemInBox(GameObject GO)
        {
            Item item = GO.GetComponent<Item>();
            switch (item.m_data.m_size)
            {
                case ItemData.ESize.small:
                    PutSmallItemInBox(GO);
                    break;
                case ItemData.ESize.medium:
                    CheckForReorganization(GO);
                    break;
                case ItemData.ESize.large:
                    CheckForReorganization(GO);
                    break;
                default:
                    break;
            }

            //if (IsBoxFull())
            //    m_tower.AddBoxToTower();

        }

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

        /// <summary> Regarde si on peut prendre un objet multi slot </summary>
        private void CheckForReorganization(GameObject GO)
        {
            Item item = GO.GetComponent<Item>();
            List<MultiSlots> multiSlotList = new List<MultiSlots>();

            multiSlotList = item.m_data.m_size == ItemData.ESize.medium ? m_doubleSlots : m_fourSlots;
            int sizeInt = item.m_data.m_size == ItemData.ESize.medium ? MEDIUM_SIZE : LARGE_SIZE;

            foreach (MultiSlots multiSlot in multiSlotList)
            {
                List<bool> slotsAvailable = new List<bool>();
                for (int i = 0; i < sizeInt; i++)
                {
                    slotsAvailable.Add(m_slotsInfo[multiSlot.m_slotIndexes[i]].m_isAvailable);
                }

                if (AllSlotIsAvailable(slotsAvailable))
                {
                    PutMultiSlotItemInBox(GO, multiSlot);
                    return;
                }
            }

            ReorganizeBox(GO);

        }

        /// <summary> Pour réorganiser la boite </summary>
        private void ReorganizeBox(GameObject GO)
        {
            // créer une liste de copie avec le nouvel object
            List<GameObject> newList = new List<GameObject>();
            int nbOfItemInBox = m_itemsInBox.Count;
            newList.Add(GO);

            for (int i = 0; i < nbOfItemInBox; i++)
            {
                GameObject objectToDelete = m_itemsInBox.Pop().GetItem();
                GameObject instant = Instantiate(objectToDelete);
                newList.Add(instant);
                Destroy(objectToDelete);
            }
            m_itemsInBox.Clear();

            // réorganiser le liste du plus grand object au plus petit
            newList.OrderByDescending(unit => (int)unit.GetComponent<Item>().m_data.m_size);

            // vider la boite
            m_availableSlotsLeft = m_totalSlots;
            for (int i = 0; i < m_totalSlots; i++)
            {
                Transform lastTransform = m_slotsInfo[i].m_slotTransform;
                m_slotsInfo[i] = new SlotInfo(lastTransform, true);
            }

            // replacer les items
            foreach (GameObject item in newList)
            {
                PutItemInBox(item);
            }

        }

        /// <summary> Pour mettre un objet multi slot dans la boite </summary>
        private void PutMultiSlotItemInBox(GameObject GO, MultiSlots multiSlot)
        {
            Item item = GO.GetComponent<Item>();
            int sizeInt = item.m_data.m_size == ItemData.ESize.medium ? MEDIUM_SIZE : LARGE_SIZE;

            m_availableSlotsLeft -= sizeInt;
            List<int> indexes = new List<int>();
            List<Transform> transforms = new List<Transform>();
            List<Vector3> allLocalPositions = new List<Vector3>();
            for (int i = 0; i < sizeInt; i++)
            {
                indexes.Add(multiSlot.m_slotIndexes[i]);
                transforms.Add(m_slotsInfo[indexes[i]].m_slotTransform);
                allLocalPositions.Add(transforms[i].localPosition);
                m_slotsInfo[indexes[i]] = new SlotInfo(transforms[i], false);
            }

            Vector3 localPosition = CreateLocalPositionForItem(allLocalPositions);
            m_itemsInBox.Push(new ItemInBox(GO, indexes, localPosition));

            if (sizeInt == MEDIUM_SIZE && Mathf.Abs((indexes[0] - indexes[1])) != 1)
            {
                GO.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            ItemSetParentInBox(GO, localPosition);

        }

        /// <summary> Place l'objet dans la hierarchie enfant de la boite </summary>
        private void ItemSetParentInBox(GameObject GO, Vector3 localPosition)
        {
            GO.transform.SetParent(gameObject.transform);
            GO.transform.localPosition = localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0); // TEST
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
