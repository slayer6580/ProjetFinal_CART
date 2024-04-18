using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class BoxManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_slotPrefab;

        [field: Header("Longeur, largeur et épaisseur de la boite")]
        [field: SerializeField] public float BoxWidth { get; private set; }
        [SerializeField] private float m_boxLength;
        [field: SerializeField] public float BoxThickness { get; private set; }

        [field: Header("Hauteur de la boite")]
        [field: SerializeField] public float BoxHeight { get; private set; }

        [Header("Nombre de slot par longeur et largeur")]
        [SerializeField] private int m_nbSlotWidth;
        [SerializeField] private int m_nbSlotLength;

        [field: Header("ReadOnly")]
        [field: SerializeField] public float SlotHeight { get; private set; }
        [field: SerializeField] public float SlotWidth { get; private set; }
        [field: SerializeField] public float SlotLenght { get; private set; }

        private List<Vector3> m_slotsLocalTransform = new List<Vector3>();
        private List<List<int>> m_doubleSlotsList = new List<List<int>>();
        private List<List<int>> m_quadrupleSlotsList = new List<List<int>>();

        private float m_halfLength;
        private float m_halfWidth;

        private static BoxManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            CalculateBoxHalfDimension();
            CalculateSlotDimension();
            CreateSlots();
        }

        public static BoxManager GetInstance()
        {
            return Instance;
        }

        #region (--- AjustBoxGraphics ---)



        /// <summary> Ajust box size </summary>
        public void AjustBoxGraphics(GameObject box)
        {
            float lenght = m_boxLength;
            float width = BoxWidth;
            float thickness = BoxThickness;
            float height = BoxHeight;
            float halfHeight = height / 2;
            float halfThickness = thickness / 2;
            float halfLength = lenght / 2;
            float halfWidth = width / 2;
            float halfWidthAndHalfThickness = halfWidth + halfThickness;
            float doubleThickness = thickness * 2;
            float lenghtAndDoubleThickness = lenght + doubleThickness;
            float HeightAndThickness = height + thickness;
            float halfHeightMinusHalfThickness = halfHeight - halfThickness;
            float HalfLenghtAndHalfThickness = halfLength + halfThickness;

            Transform boxGraphics = box.transform.GetChild(1);


            // bottom
            Transform boxBottom = boxGraphics.transform.GetChild(0);       
            boxBottom.localScale = new Vector3(lenght, thickness, width);
            boxBottom.localPosition = new Vector3(0, -halfThickness, 0);

            // side left
            Transform boxLeft = boxGraphics.transform.GetChild(1);
            boxLeft.localScale = new Vector3(lenghtAndDoubleThickness, thickness, HeightAndThickness);
            boxLeft.localPosition = new Vector3(0, halfHeightMinusHalfThickness, -halfWidthAndHalfThickness);

            // side right
            Transform boxRight = boxGraphics.transform.GetChild(2);
            boxRight.localScale = new Vector3(lenghtAndDoubleThickness, thickness, HeightAndThickness);
            boxRight.localPosition = new Vector3(0, halfHeightMinusHalfThickness, halfWidthAndHalfThickness);

            // back
            Transform boxBack = boxGraphics.transform.GetChild(3);
            boxBack.localScale = new Vector3(width, thickness, HeightAndThickness);
            boxBack.localPosition = new Vector3(HalfLenghtAndHalfThickness, halfHeightMinusHalfThickness, 0);

            // front
            Transform boxFront = boxGraphics.transform.GetChild(4);
            boxFront.localScale = new Vector3(width, thickness, HeightAndThickness);
            boxFront.localPosition = new Vector3(-HalfLenghtAndHalfThickness, halfHeightMinusHalfThickness, 0);

        }
        #endregion


        #region (--- CreateSlots ---)

        /// <summary> Calculates all slots infos </summary>
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

        /// <summary> Find all length position </summary>
        private void FindLengthPositions(List<float> m_slotsLengthPosition)
        {
            float halfLengthSpacing = SlotLenght / 2;
            float lengthPosition = -m_halfLength;

            for (int i = 0; i < m_nbSlotWidth; i++)
            {
                lengthPosition += i == 0 ? halfLengthSpacing : SlotLenght;
                m_slotsLengthPosition.Add(lengthPosition);
            }
        }

        /// <summary> Find all width position </summary>
        private void FindWidthPositions(List<float> slotsWidthPosition)
        {
            float halfWidthSpacing = SlotWidth / 2;
            float widthPosition = m_halfWidth;

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                widthPosition -= i == 0 ? halfWidthSpacing : SlotWidth;
                slotsWidthPosition.Add(widthPosition);
            }
        }

        /// <summary> Add all slots to the box </summary>
        private void PlaceSlotsInBox(List<float> slotsLengthPosition, List<float> slotsWidthPosition)
        {
            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    Vector3 slotLocalPosition = new Vector3(slotsLengthPosition[j], 0, slotsWidthPosition[i]);
                    m_slotsLocalTransform.Add(slotLocalPosition);
                }
            }
        }

        /// <summary> Find all double slots posibilities </summary>
        private void FindAllDoubleSlots()
        {
            int index = 0; int index2 = 0;

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    if (i == 0) // first row
                    {
                        if (index != (m_nbSlotWidth - 1)) // no right collumn
                        {
                            index2 = index + 1; // right
                            SendDoubleSlotToBox(index, index2);
                        }
                    }
                    else // les autres rangées
                    {
                        if ((index + 1) % m_nbSlotWidth == 0) // right collumn
                        {
                            index2 = index - m_nbSlotWidth; //up
                            SendDoubleSlotToBox(index, index2);
                        }
                        else // colonne dans le milieu
                        {
                            index2 = index - m_nbSlotWidth; //up
                            SendDoubleSlotToBox(index, index2);

                            index2 = index + 1; //right
                            SendDoubleSlotToBox(index, index2);
                        }
                    }
                    index++;
                }
            }
        }

        /// <summary> Find all quadruple slots posibilities </summary>
        private void FindAllFourSlots()
        {
            int index = 0; int index2 = 0; int index3 = 0; int index4 = 0;

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    if (i + 1 < m_nbSlotLength) // not last row
                    {
                        if ((index + 1) % m_nbSlotWidth != 0) // no right collumn
                        {
                            index2 = index + 1; // right
                            index3 = index + m_nbSlotWidth; // bottom
                            index4 = index + m_nbSlotWidth + 1; // bottom right
                            SendFourSlotToBox(index, index2, index3, index4);
                        }
                    }
                    index++;
                }
            }
        }

        /// <summary> Send all double slots possibilities to box </summary>
        private void SendDoubleSlotToBox(int index, int index2)
        {
            List<int> doubleSlots = new List<int>()
            {
              index,
              index2
            };

            m_doubleSlotsList.Add(doubleSlots);
        }

        /// <summary> Send all quadruple slots possibilities to box </summary>
        private void SendFourSlotToBox(int index, int index2, int index3, int index4)
        {
            List<int> quadrupleSlots = new List<int>()
            {
              index,
              index2,
              index3,
              index4
            };

            m_quadrupleSlotsList.Add(quadrupleSlots);
        }

        #endregion

        #region (--- CalculateDimension ---)

        /// <summary> Calculate box half dimension </summary>
        private void CalculateBoxHalfDimension()
        {
            m_halfLength = BoxWidth / 2;
            m_halfWidth = m_boxLength / 2;
        }

        /// <summary> Calculate slot dimension </summary>
        private void CalculateSlotDimension()
        {
            SlotLenght = BoxWidth / m_nbSlotWidth;
            SlotWidth = m_boxLength / m_nbSlotLength;
            SlotHeight = BoxHeight;
        }

        #endregion

        #region (--- Getter ---)

        public List<List<int>> GetDoubleSlotsIndexes()
        {
            return m_doubleSlotsList;
        }

        public List<List<int>> GetQuadrupleSlotsIndexes()
        {
            return m_quadrupleSlotsList;
        }

        public List<Vector3> GetSlotsLocalVector()
        {
            return m_slotsLocalTransform;
        }

        public Vector3 GetLocalScale()
        {
            return new Vector3(SlotLenght, SlotHeight, SlotWidth);
        }

        public int GetTotalSLots()
        {
            return m_nbSlotLength * m_nbSlotWidth;
        }

        public float GetBoxHeightDifference()
        {
            return SlotHeight + BoxThickness;
        }

        #endregion

    }
}

