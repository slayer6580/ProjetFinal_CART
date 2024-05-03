using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoxSystem
{

    public class Box : MonoBehaviour
    {

        #region (--- Struct ---)

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

        public struct SlotInfo
        {
            public SlotInfo(Vector3 slotPosition, bool isAvailable)
            {
                m_slotTransform = slotPosition;
                m_isAvailable = isAvailable;
            }

            public Vector3 m_slotTransform;
            public bool m_isAvailable;
        }
        #endregion

        #region (--- Variables ---)

        private List<SlotInfo> m_slotsList = new List<SlotInfo>();
        private List<ItemInBox> m_itemsList = new List<ItemInBox>();
        private List<MultiSlots> m_doubleSlots = new List<MultiSlots>(); // Coordonnés de tout les connections de slot double
        private List<MultiSlots> m_quadrupleSlots = new List<MultiSlots>(); // Coordonnés de tout les connections de slot a quatre (2 x 2)
        private int m_totalSlots;
        private int m_availableSlotsLeft;
        private TowerBoxSystem m_tower;
        private Vector3 m_initialLocalPositionInBox;
        private bool m_boxInit = false;

        #endregion


        private void Start()
        {
            InitBox();
        }

        #region (--- InitFunctions ---)

        private void InitBox()
        {
            if (m_boxInit)
                return;

            BoxManager Instance = BoxManager.GetInstance();

            GetSlotsList(Instance);
            GetDoubleSLots(Instance);    
            GetQuadrupleSlots(Instance);        
            GetTotalSlots(Instance);       
            Instance.AjustBoxGraphics(this.gameObject);

            m_boxInit = true;

        }

        private void GetTotalSlots(BoxManager Instance)
        {
            int slotTotal = Instance.GetTotalSLots();
            InitAvailableSlots(slotTotal);
        }

        private void GetQuadrupleSlots(BoxManager Instance)
        {
            List<List<int>> ListOfQuadrupleIndexes = Instance.GetQuadrupleSlotsIndexes();
            foreach (List<int> indexes in ListOfQuadrupleIndexes)
                m_quadrupleSlots.Add(new MultiSlots(indexes));
        }

        private void GetDoubleSLots(BoxManager Instance)
        {
            List<List<int>> ListOfDoubleIndexes = Instance.GetDoubleSlotsIndexes();
            foreach (List<int> indexes in ListOfDoubleIndexes)
                m_doubleSlots.Add(new MultiSlots(indexes));
        }

        private void GetSlotsList(BoxManager Instance)
        {
            List<Vector3> m_slotsLocalTransform = Instance.GetSlotsLocalVector();
            foreach (Vector3 slot in m_slotsLocalTransform)
                m_slotsList.Add(new SlotInfo(slot, true));
        }

        /// <summary> Number of slots info given by BoxSetup </summary>
        public void InitAvailableSlots(int numberOfSlots)
        {
            m_totalSlots = numberOfSlots;
            m_availableSlotsLeft = m_totalSlots;
        }

        /// <summary> Set parent reference </summary>
        public void SetTower(TowerBoxSystem tower)
        {
            m_tower = tower;
        }
        #endregion

        #region (--- Bool Verification ---)

        /// <summary> Look if box can take object, separate in two functions based on item size </summary>
        public bool CanPutItemInsideBox(ItemData.ESize size)
        {
            if (!m_boxInit)
                InitBox();

            return size == ItemData.ESize.small ? CanPutSmallItemInBox() : CanPutMultiSlotItemInBox(size);
        }

        /// <summary> Look if box can take small object based on its size </summary>
        private bool CanPutSmallItemInBox()
        {
            return m_availableSlotsLeft > 0 ? true : false;
        }

        /// <summary> Look if box can take multi object based on its size </summary>
        private bool CanPutMultiSlotItemInBox(ItemData.ESize size)
        {
            int sizeInt = size == ItemData.ESize.medium ? GameConstants.MEDIUM_SIZE : GameConstants.LARGE_SIZE;
            return m_availableSlotsLeft < sizeInt ? false : true;
        }

        /// <summary> Look if box is empty </summary>
        public bool IsEmpty()
        {
            return m_itemsList.Count == 0;
        }
        #endregion

        #region (--- PutItemInsideBox ---)

        /// <summary> Put item inside box, separate in two functions based on item size </summary>
        public void PutItemInBox(GameObject GO, bool autoSnap = false)
        {
            if (!m_boxInit)
                InitBox();

            Item item = GO.GetComponent<Item>();

            if (item.m_data.m_size == ItemData.ESize.small)
                PutSmallItemInBox(GO, autoSnap);
            else
                PutInBoxOrReorganize(GO, autoSnap);
        }


        /// <summary> Put a small item inside the box </summary>
        private void PutSmallItemInBox(GameObject GO, bool autoSnap)
        {
            for (int i = 0; i < m_slotsList.Count; i++)
            {
                if (m_slotsList[i].m_isAvailable)
                {
                    m_availableSlotsLeft--;
                    Vector3 slotTransform = m_slotsList[i].m_slotTransform;
                    m_slotsList[i] = new SlotInfo(slotTransform, false);
                    List<int> allIndex = new List<int>();
                    allIndex.Add(i);
                    m_itemsList.Add(new ItemInBox(GO, allIndex, slotTransform));
                    SlerpAndSnap(GO, slotTransform, false, autoSnap);

                    return;
                }
            }
        }

        /// <summary> Put a multi slot item inside the box or reorganization </summary>
        private void PutInBoxOrReorganize(GameObject GO, bool autoSnap)
        {
            Item item = GO.GetComponent<Item>();
            List<MultiSlots> multiSlotList = new List<MultiSlots>();

            multiSlotList = item.m_data.m_size == ItemData.ESize.medium ? m_doubleSlots : m_quadrupleSlots;
            int sizeInt = item.m_data.m_size == ItemData.ESize.medium ? GameConstants.MEDIUM_SIZE : GameConstants.LARGE_SIZE;

            // Look for box space
            foreach (MultiSlots multiSlot in multiSlotList)
            {
                List<bool> slotsAvailable = new List<bool>();
                for (int i = 0; i < sizeInt; i++)
                {
                    slotsAvailable.Add(m_slotsList[multiSlot.m_slotIndexes[i]].m_isAvailable);
                }

                if (AllSlotIsAvailable(slotsAvailable))
                {
                    PutMultiSlotItemInBox(GO, multiSlot, autoSnap);
                    return;
                }
            }

            // if cannot enter, reorganize box
            ReorganizeBox(GO);
        }

        /// <summary> For box reorganization </summary>
        private void ReorganizeBox(GameObject GO)
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(GO);

            int nbOfItemInBox = m_itemsList.Count;
            for (int i = 0; i < nbOfItemInBox; i++)
            {
                GameObject objectToDelete = m_itemsList[i].GetItem();
                GameObject instant = Instantiate(objectToDelete);
                newList.Add(instant);
                Destroy(objectToDelete);
            }
            m_itemsList.Clear();

            newList = newList.OrderByDescending(unit => (int)unit.GetComponent<Item>().m_data.m_size).ToList();

            m_availableSlotsLeft = m_totalSlots;
            for (int i = 0; i < m_totalSlots; i++)
            {
                Vector3 lastTransform = m_slotsList[i].m_slotTransform;
                m_slotsList[i] = new SlotInfo(lastTransform, true);
            }

            foreach (GameObject newItem in newList)
            {
                PutItemInBox(newItem, true);
            }
        }

        /// <summary> Put a multi slot item inside the box </summary>
        private void PutMultiSlotItemInBox(GameObject GO, MultiSlots multiSlot, bool autoSnap)
        {
            Item item = GO.GetComponent<Item>();
            int sizeInt = item.m_data.m_size == ItemData.ESize.medium ? GameConstants.MEDIUM_SIZE : GameConstants.LARGE_SIZE;
            List<Vector3> allLocalPositions = new List<Vector3>();

            m_availableSlotsLeft -= sizeInt;

            for (int i = 0; i < sizeInt; i++)
            {
                allLocalPositions.Add(m_slotsList[multiSlot.m_slotIndexes[i]].m_slotTransform);
                m_slotsList[multiSlot.m_slotIndexes[i]] = new SlotInfo(m_slotsList[multiSlot.m_slotIndexes[i]].m_slotTransform, false);
            }

            Vector3 localPosition = FindPositionInsideBox(allLocalPositions);
            m_itemsList.Add(new ItemInBox(GO, multiSlot.m_slotIndexes, localPosition));


            bool turn90Degree = false;
            if (sizeInt == GameConstants.MEDIUM_SIZE && Mathf.Abs((multiSlot.m_slotIndexes[0] - multiSlot.m_slotIndexes[1])) != 1)
            {
                turn90Degree = true;
            }

            SlerpAndSnap(GO, localPosition, turn90Degree, autoSnap);
        }
        #endregion

        #region (--- SlerpAndSnap ---)

        /// <summary> Start Slerp and Snap </summary>
        private void SlerpAndSnap(GameObject GO, Vector3 localPosition, bool turn90Degree, bool autoSnap = false)
        {
            Transform GOChild = GO.transform.GetChild(0);

            Vector3 itemScale = BoxManager.GetInstance().GetLocalScale();
            //GO.transform.localScale = itemScale; //888888888888888888888888888888888888888888888888888888888888888888888888888888888888
            GOChild.localScale = Vector3.one;
            GO.GetComponent<Item>().StartSlerpAndSnap(this, localPosition + new Vector3(0, itemScale.y / 2, 0), m_tower.Player, turn90Degree, m_tower.ItemSnapDistance, autoSnap);
        }

        #endregion

        #region (--- HelpFunctions ---)

        /// <summary> Look if bool list is all true </summary>
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

        /// <summary> Find local position of multi-slots item inside box </summary>
        private Vector3 FindPositionInsideBox(List<Vector3> slotLocalPositions)
        {
            Vector3 localposition = Vector3.zero;
            int nbOfPositions = slotLocalPositions.Count;

            foreach (Vector3 localPosition in slotLocalPositions)
            {
                localposition += localPosition;
            }

            return localposition / nbOfPositions;
        }

        /// <summary> Get last item inside box </summary>
        public ItemInBox GetLastItem()
        {
            return m_itemsList[m_itemsList.Count - 1];
        }

        /// <summary> Put back item slot as available </summary>
        public void ResetSlots(ItemInBox lastItemInBox)
        {
            foreach (int itemIndex in lastItemInBox.m_slotIndex)
            {
                Vector3 slotTransform = m_slotsList[itemIndex].m_slotTransform;
                m_slotsList[itemIndex] = new SlotInfo(slotTransform, true);
            }
        }

        /// <summary> Get list of item inside box </summary>
        public List<ItemInBox> GetItemsInBox()
        {
            return m_itemsList;
        }

        /// <summary> Set initial box local position inside tower </summary>
        public void SetBoxInitialLocalPosition(Vector3 localPosition)
        {
            m_initialLocalPositionInBox = localPosition;
        }

        /// <summary> Get parent reference </summary>
        public TowerBoxSystem GetTower()
        {
            return m_tower;
        }

        /// <summary> Get anchor height based on box size </summary>
        public float GetAnchorHeight()
        {
            return BoxManager.GetInstance().BoxThickness;
        }

        /// <summary> Get anchor width based on box size </summary>
        public float GetAnchorWidth()
        {
            return (BoxManager.GetInstance().BoxWidth / 2) + BoxManager.GetInstance().BoxThickness;
        }


        #endregion
    }
}