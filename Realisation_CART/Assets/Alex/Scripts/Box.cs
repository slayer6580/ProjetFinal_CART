using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(BoxSetup))]

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
            public SlotInfo(Transform slotPosition, bool isAvailable)
            {
                m_slotTransform = slotPosition;
                m_isAvailable = isAvailable;
            }

            public Transform m_slotTransform;
            public bool m_isAvailable;
        }
        #endregion


        #region (--- Variables ---)

        private List<SlotInfo> m_slotsList = new List<SlotInfo>();
        private List<ItemInBox> m_itemsList = new List<ItemInBox>();
        private List<MultiSlots> m_doubleSlots = new List<MultiSlots>(); // Coordonnés de tout les connections de slot double
        private List<MultiSlots> m_fourSlots = new List<MultiSlots>(); // Coordonnés de tout les connections de slot a quatre (2 x 2)
        private int m_totalSlots;
        private int m_availableSlotsLeft;
        private int m_itemCount = 0;
        private BoxSetup m_boxSetup;
        private const int MEDIUM_SIZE = 2;
        private const int LARGE_SIZE = 4;
        private Tower m_tower;
        #endregion



        private void Awake()
        {
            m_boxSetup = GetComponent<BoxSetup>();
        }



        #region (--- InitFunctions ---)
        /// <summary> BoxSetup va nous donner les infos des slots </summary>
        public void AddSlotInList(Transform slotTransform)
        {
            m_slotsList.Add(new SlotInfo(slotTransform, true));
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

        /// <summary> La boite se connecte a la tour lors de sa propre création </summary>
        public void SetTower(Tower tower)
        {
            m_tower = tower;    
        }
        #endregion 


        #region (--- Bool Verification ---)
        /// <summary> Regarde si on peut prendre l'objet selon sa taille </summary>
        public bool CanPutItemInsideBox(ItemData.ESize size)
        {
            return size == ItemData.ESize.small ? CanPutSmallItemInBox() : CanPutMultiSlotItemInBox(size);
        }

        /// <summary> Regarde si on peut prendre un petit objet </summary>
        private bool CanPutSmallItemInBox()
        {
            return m_availableSlotsLeft > 0 ? true : false;
        }

        /// <summary> Regarde si on peut prendre un objet multi slot </summary>
        private bool CanPutMultiSlotItemInBox(ItemData.ESize size)
        {
            int sizeInt = size == ItemData.ESize.medium ? MEDIUM_SIZE : LARGE_SIZE;
            return m_availableSlotsLeft < sizeInt ? false : true;
        }
        #endregion



        #region (--- PutItemInsideBox ---)
        /// <summary> Pour mettre l'objet dans la boite selon sa taille </summary>
        public void PutItemInBox(GameObject GO)
        {
            Item item = GO.GetComponent<Item>();

            if (item.m_data.m_size == ItemData.ESize.small)
                PutSmallItemInBox(GO);
            else
                PutInBoxOrReorganize(GO);
        }

        /// <summary> Pour mettre un petit objet dans la boite </summary>
        private void PutSmallItemInBox(GameObject GO)
        {
            for (int i = 0; i < m_slotsList.Count; i++)
            {
                if (m_slotsList[i].m_isAvailable)
                {
                    m_availableSlotsLeft--;
                    m_itemCount++;
                    GO.name = "Item " + m_itemCount;

                    // Pour empecher de spawner sous la boite lorsque la boite se trouve plus haut que sa position initial
                    Transform slotTransform = m_slotsList[i].m_slotTransform;
                    Vector3 destination = slotTransform.localPosition + transform.parent.parent.position + transform.parent.localPosition;
                    Vector3 slotHeight = new Vector3(0, m_boxSetup.SlotHeight / 2 + 0.11f, 0);
                    destination += slotHeight;
                    GO.transform.position = destination;

                    m_slotsList[i] = new SlotInfo(slotTransform, false);
                    List<int> allIndex = new List<int>();
                    allIndex.Add(i);
                    m_itemsList.Add(new ItemInBox(GO, allIndex, slotTransform.localPosition));
                    SetItemForSlerpAndSnap(GO, slotTransform.localPosition, false);
                    return;
                }
            }
        }

        /// <summary> Regarde si on peut placer le multi slot item tout de suite ou réorganizer </summary>
        private void PutInBoxOrReorganize(GameObject GO)
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
                    slotsAvailable.Add(m_slotsList[multiSlot.m_slotIndexes[i]].m_isAvailable);
                }

                if (AllSlotIsAvailable(slotsAvailable))
                {
                    PutMultiSlotItemInBox(GO, multiSlot);
                    return;
                }
            }
            ReorganizeBox(GO);
        }

        /// <summary> Pour réorganiser la boite et placer l'item à l'interieur </summary>
        private void ReorganizeBox(GameObject GO)
        {
            // Faire une nouvelle liste temporaire et ajouter le nouvel item dedans
            List<GameObject> newList = new List<GameObject>();
            newList.Add(GO);

            // Faire une copie de chaque item de la boite en supprimant les anciens dans notre nouvelle liste
            int nbOfItemInBox = m_itemsList.Count;
            for (int i = 0; i < nbOfItemInBox; i++)
            {
                GameObject objectToDelete = m_itemsList[i].GetItem();
                GameObject instant = Instantiate(objectToDelete);
                newList.Add(instant);
                Destroy(objectToDelete);
            }
            m_itemsList.Clear();

            // réorganiser le liste du plus grand object au plus petit
            newList = newList.OrderByDescending(unit => (int)unit.GetComponent<Item>().m_data.m_size).ToList(); // précis comme ca

            // remettre la boite a zero
            m_availableSlotsLeft = m_totalSlots;
            for (int i = 0; i < m_totalSlots; i++)
            {
                Transform lastTransform = m_slotsList[i].m_slotTransform;
                m_slotsList[i] = new SlotInfo(lastTransform, true);
            }

            // replacer les items du plus grand au plus petit, 100% accurate
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
            List<Vector3> allLocalPositions = new List<Vector3>();

            m_availableSlotsLeft -= sizeInt;

            for (int i = 0; i < sizeInt; i++)
            {
                allLocalPositions.Add(m_slotsList[multiSlot.m_slotIndexes[i]].m_slotTransform.localPosition);
                m_slotsList[multiSlot.m_slotIndexes[i]] = new SlotInfo(m_slotsList[multiSlot.m_slotIndexes[i]].m_slotTransform, false);
            }

            Vector3 localPosition = CreateLocalPositionForItem(allLocalPositions);
            m_itemsList.Add(new ItemInBox(GO, multiSlot.m_slotIndexes, localPosition));


            bool turn90Degree = false;
            // TODO va devoir transferer l'information du if ci dessous dans la fonction SetItemSlerpAndSnap()
            if (sizeInt == MEDIUM_SIZE && Mathf.Abs((multiSlot.m_slotIndexes[0] - multiSlot.m_slotIndexes[1])) != 1)
            {
                turn90Degree = true;
            }

            SetItemForSlerpAndSnap(GO, localPosition, turn90Degree);
        }
        #endregion



        #region (--- RemoveItemFromBox ---)

        public void RemoveItemImpulse()
        {
            // get the top item
            if (m_itemsList.Count <= 0)
            {
                Debug.LogWarning("No item in the box");
                return;
            }

            ItemInBox itemInBox = m_itemsList[m_itemsList.Count - 1];
            if (itemInBox.m_item == null)
            {
                Debug.LogWarning("Item is null");
                return;
            }

            Debug.Log("Item to remove: " + itemInBox.m_item.name);
            itemInBox.m_item.AddComponent<Rigidbody>().AddForce(Vector3.left + Vector3.up * 10, ForceMode.Impulse);
            m_itemsList.RemoveAt(m_itemsList.Count - 1);
        }

        #endregion



        #region (--- HelpFunctions ---)
        /// <summary> Place l'objet dans la hierarchie enfant de la boite </summary>
        private void SetItemForSlerpAndSnap(GameObject GO, Vector3 localPosition, bool turn90Degree)
        {
            GO.transform.SetParent(gameObject.transform);
            GO.GetComponent<Item>().StartSlerpAndSnap(this, localPosition + new Vector3(0, m_boxSetup.SlotHeight / 2, 0), m_tower.Cart.transform, turn90Degree);
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
        #endregion
    }
}