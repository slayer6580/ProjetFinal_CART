using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Box1))]

    public class BoxSetup1 : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_slotPrefab;

        [Header("Longeur et largeur de la boite")]
        [SerializeField] private float m_boxLength;
        [SerializeField] private float m_boxWidth;

        [Header("Nombre de slot par longeur et largeur")]
        [SerializeField] private int m_nbSlotLength;
        [SerializeField] private int m_nbSlotWidth;

        [field: Header("Hauteur des slots")]
        [field: SerializeField] public float SlotHeight { get; private set; }

        [Header("READ ONLY")]
        [SerializeField] private float slotLengthCalculation;
        [SerializeField] private float slotWidthCalculation;

        private float m_slotLength;
        private float m_slotWidth;
        private float m_halfLength;
        private float m_halfWidth;
        private Transform m_slotsParent;
        private Box1 m_box;
        private int m_totalSlots;

        private void Awake()
        {
            m_box = GetComponent<Box1>();
            m_slotsParent = transform.GetChild(0);
            SetAvailableSlots();
            CalculateBoxHalfDimension();
            CalculateSlotDimension();
            CreateSlots();
        }

        /// <summary> Les calculs de positionement puis ajout dans la boite </summary>
        private void CreateSlots()
        {
            List<float> slotsLengthPosition = new List<float>();
            List<float> slotsWidthPosition = new List<float>();

            FindLengthPositions(slotsLengthPosition);
            FindWidthPositions(slotsWidthPosition);
            PlaceSlotsInBox(slotsLengthPosition, slotsWidthPosition);
            FindAllDoubleSlots();
            FindAllFourSlots();
        }

        /// <summary> Calcule la moitié des longeurs de la boite pour le point de départ du placement des slots </summary>
        private void CalculateBoxHalfDimension()
        {
            m_halfLength = m_boxLength / 2;
            m_halfWidth = m_boxWidth / 2;
        }

        /// <summary> Calcule la dimension des slots </summary>
        private void CalculateSlotDimension()
        {
            m_slotLength = m_boxLength / m_nbSlotLength;
            m_slotWidth = m_boxWidth / m_nbSlotWidth;
            slotLengthCalculation = m_slotLength;
            slotWidthCalculation = m_slotWidth;
        }

        private void SetAvailableSlots()
        {
            m_totalSlots = m_nbSlotWidth * m_nbSlotLength;
            m_box.InitAvailableSlots(m_totalSlots);
        }

     
        /// <summary> Trouve tout les double slots de la boite </summary>
        private void FindAllDoubleSlots()
        {
            int index = 0; int index2 = 0;

            for (int i = 0; i < m_nbSlotWidth; i++)
            {
                for (int j = 0; j < m_nbSlotLength; j++)
                {
                    if (i == 0) // premiere rangée
                    {
                        if (index != (m_nbSlotLength - 1)) // pas colonne de droite
                        {
                            index2 = index + 1; // droite
                            SendDoubleSlotToBox(index, index2);
                        }
                    }
                    else // les autres rangées
                    {
                        if ((index + 1) % m_nbSlotLength == 0) // colonne de droite
                        {
                            index2 = index - m_nbSlotLength; //haut
                            SendDoubleSlotToBox(index, index2);
                        }
                        else // colonne dans le milieu
                        {
                            index2 = index - m_nbSlotLength; //haut
                            SendDoubleSlotToBox(index, index2);

                            index2 = index + 1; //droite
                            SendDoubleSlotToBox(index, index2);
                        }
                    }
                    index++;
                }
            }
        }

        /// <summary> Trouve tout les four slots de la boite </summary>
        private void FindAllFourSlots()
        {
            int index = 0; int index2 = 0; int index3 = 0; int index4 = 0;

            for (int i = 0; i < m_nbSlotWidth; i++)
            {
                for (int j = 0; j < m_nbSlotLength; j++)
                {
                    if (i + 1 < m_nbSlotWidth) // pas la derniere rangée
                    {
                        if ((index + 1) % m_nbSlotLength != 0) // pas colonne de droite
                        {
                            index2 = index + 1; // droite
                            index3 = index + m_nbSlotLength; // bas
                            index4 = index + m_nbSlotLength + 1; // en bas a droite
                            SendFourSlotToBox(index, index2, index3, index4);
                        }
                    }
                    index++;
                }
            }
        }

        /// <summary> Donne les double slots au box script </summary>
        private void SendDoubleSlotToBox(int index, int index2)
        {
            List<int> doubleSlots = new List<int>()
            {
              index,
              index2
            };

            m_box.AddDoubleSlotInList(doubleSlots);
        }

        /// <summary> Donne les four slots au box script </summary>
        private void SendFourSlotToBox(int index, int index2, int index3, int index4)
        {
            List<int> fourSlots = new List<int>()
            {
              index,
              index2,
              index3,
              index4
            };

            m_box.AddFourSlotInList(fourSlots);
        }

        /// <summary> Ajouter les slots dans la boite selon des calculs de positionement </summary>
        private void PlaceSlotsInBox(List<float> slotsLengthPosition, List<float> slotsWidthPosition)
        {
            for (int i = 0; i < m_nbSlotWidth; i++)
            {
                for (int j = 0; j < m_nbSlotLength; j++)
                {
                    Vector3 slotPosition = new Vector3(slotsLengthPosition[j], 0, slotsWidthPosition[i]);
                    GameObject instant = Instantiate(m_slotPrefab, m_slotsParent);
                    instant.transform.localPosition = slotPosition;
                    instant.transform.localScale = new Vector3(m_slotLength, SlotHeight, m_slotWidth);
                    m_box.AddSlotInList(instant.transform);
                }
            }
        }

        /// <summary> Trouver tout les positions de largeur </summary>
        private void FindWidthPositions(List<float> slotsWidthPosition)
        {
            float halfWidthSpacing = m_slotWidth / 2;
            float widthPosition = m_halfWidth;

            for (int i = 0; i < m_nbSlotWidth; i++)
            {
                widthPosition -= i == 0 ? halfWidthSpacing : m_slotWidth;
                slotsWidthPosition.Add(widthPosition);
            }
        }

        /// <summary> Trouver tout les positions de longueur </summary>
        private void FindLengthPositions(List<float> m_slotsLengthPosition)
        {
            float halfLengthSpacing = m_slotLength / 2;
            float lengthPosition = -m_halfLength;

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                lengthPosition += i == 0 ? halfLengthSpacing : m_slotLength;
                m_slotsLengthPosition.Add(lengthPosition);
            }
        }
    }
}
