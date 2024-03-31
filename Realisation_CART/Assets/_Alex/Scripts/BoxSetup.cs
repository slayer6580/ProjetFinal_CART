using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Box))]

    public class BoxSetup : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_slotPrefab;

        [Header("Longeur et épaisseur de la boite")]
        [SerializeField] private float m_boxThickness;

        [field: Header("Hauteur et largeur de la boite")]
        [field: SerializeField] public float BoxHeight { get; private set; }
        [field: SerializeField] public float BoxWidth { get; private set; }
        [field: SerializeField] public float BoxLength { get; private set; }

        [Header("Nombre de slot par longeur et largeur")]
        [SerializeField] private int m_nbSlotWidth;
        [SerializeField] private int m_nbSlotLength;

        public float SlotHeight { get; private set; }
        public float SlotWidth { get; private set; }
        public float SlotLenght { get; private set; }


        [Header("Pour placer les parties de la boite")]
        [SerializeField] private bool m_showBoxParts;
        [Space]
        [ShowIf("m_showBoxParts", true)][SerializeField] private GameObject m_boxBottom;
        [ShowIf("m_showBoxParts", true)][SerializeField] private GameObject m_boxSideLeft;
        [ShowIf("m_showBoxParts", true)][SerializeField] private GameObject m_boxSideRight;
        [ShowIf("m_showBoxParts", true)][SerializeField] private GameObject m_boxFront;
        [ShowIf("m_showBoxParts", true)][SerializeField] private GameObject m_boxBack;

        private float m_halfLength;
        private float m_halfWidth;
        private Transform m_slotsParent;
        private Box m_box;
        private int m_totalSlots;

        private void Awake()
        {
            m_box = GetComponent<Box>();
            m_slotsParent = transform.GetChild(0);

            AjustBoxGraphics();
            SetAvailableSlots();
            CalculateBoxHalfDimension();
            CalculateSlotDimension();
            CreateSlots();
        }

        private void OnValidate()
        {
            AjustBoxGraphics();
        }

        #region (--- AjustBoxGraphics ---)

        /// <summary> Ajuster les dimensions de la boite selon les grandeurs donnés </summary>
        private void AjustBoxGraphics()
        {
            float lenght = BoxLength;
            float width = BoxWidth;
            float thickness = m_boxThickness;
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

            // bottom
            SetUpParts(m_boxBottom,
                new Vector3(lenght, thickness, width),
                new Vector3(0, -halfThickness, 0));

            // side left
            SetUpParts(m_boxSideLeft,
                new Vector3(lenghtAndDoubleThickness, thickness, HeightAndThickness),
                new Vector3(0, halfHeightMinusHalfThickness, -halfWidthAndHalfThickness));

            // side right
            SetUpParts(m_boxSideRight,
                   new Vector3(lenghtAndDoubleThickness, thickness, HeightAndThickness),
              new Vector3(0, halfHeightMinusHalfThickness, halfWidthAndHalfThickness));

            // back
            SetUpParts(m_boxBack,
                new Vector3(width, thickness, HeightAndThickness),
                new Vector3(HalfLenghtAndHalfThickness, halfHeightMinusHalfThickness, 0));


            // front
            SetUpParts(m_boxFront,
                new Vector3(width, thickness, HeightAndThickness),
                new Vector3(-HalfLenghtAndHalfThickness, halfHeightMinusHalfThickness, 0));
        }

        private void SetUpParts(GameObject part, Vector3 boxScale, Vector3 boxlocalPosition)
        {
            part.transform.localScale = boxScale;
            part.transform.localPosition = boxlocalPosition;
        }

        #endregion


        #region (--- CreateSlots ---)

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

        /// <summary> Trouver tout les positions de longueur </summary>
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

        /// <summary> Trouver tout les positions de largeur </summary>
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

        /// <summary> Ajouter les slots dans la boite selon des calculs de positionement </summary>
        private void PlaceSlotsInBox(List<float> slotsLengthPosition, List<float> slotsWidthPosition)
        {
            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    Vector3 slotPosition = new Vector3(slotsLengthPosition[j], 0, slotsWidthPosition[i]);
                    GameObject instant = Instantiate(m_slotPrefab, m_slotsParent);
                    instant.transform.localPosition = slotPosition;
                    instant.transform.localScale = new Vector3(SlotLenght, SlotHeight, SlotWidth);
                    m_box.AddSlotInList(instant.transform);
                }
            }
        }

        /// <summary> Trouve tout les double slots de la boite </summary>
        private void FindAllDoubleSlots()
        {
            int index = 0; int index2 = 0;

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    if (i == 0) // premiere rangée
                    {
                        if (index != (m_nbSlotWidth - 1)) // pas colonne de droite
                        {
                            index2 = index + 1; // droite
                            SendDoubleSlotToBox(index, index2);
                        }
                    }
                    else // les autres rangées
                    {
                        if ((index + 1) % m_nbSlotWidth == 0) // colonne de droite
                        {
                            index2 = index - m_nbSlotWidth; //haut
                            SendDoubleSlotToBox(index, index2);
                        }
                        else // colonne dans le milieu
                        {
                            index2 = index - m_nbSlotWidth; //haut
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

            for (int i = 0; i < m_nbSlotLength; i++)
            {
                for (int j = 0; j < m_nbSlotWidth; j++)
                {
                    if (i + 1 < m_nbSlotLength) // pas la derniere rangée
                    {
                        if ((index + 1) % m_nbSlotWidth != 0) // pas colonne de droite
                        {
                            index2 = index + 1; // droite
                            index3 = index + m_nbSlotWidth; // bas
                            index4 = index + m_nbSlotWidth + 1; // en bas a droite
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

        #endregion


        #region (--- CalculateDimension ---)

        /// <summary> Calcule la moitié des longeurs de la boite pour le point de départ du placement des slots </summary>
        private void CalculateBoxHalfDimension()
        {
            m_halfLength = BoxWidth / 2;
            m_halfWidth = BoxLength / 2;
        }

        /// <summary> Calcule la dimension des slots </summary>
        private void CalculateSlotDimension()
        {
            SlotLenght = BoxWidth / m_nbSlotWidth;
            SlotWidth = BoxLength / m_nbSlotLength;
            SlotHeight = BoxHeight;
        }

        /// <summary> Donne le nombre total de slot au Box </summary>
        private void SetAvailableSlots()
        {
            m_totalSlots = m_nbSlotLength * m_nbSlotWidth;
            m_box.InitAvailableSlots(m_totalSlots);
        }

        #endregion

    }
}
