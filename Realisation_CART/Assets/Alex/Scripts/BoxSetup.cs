using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Box))]

public class BoxSetup : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject m_slotPrefab;

    [Header("Longeur et largeur de la boite")]
    [SerializeField] private float m_boxLength;
    [SerializeField] private float m_boxWidth;

    [Header("Nombre de slot par longeur et largeur")]
    [SerializeField] private int m_nbSlotLength;
    [SerializeField] private int m_nbSlotWidth;

    [Header("Hauteur des slots")]
    [SerializeField] private float m_slotHeight;

    [Header("READ ONLY")]
    [SerializeField] private float slotLengthCalculation;
    [SerializeField] private float slotWidthCalculation;

    private float m_slotLength;
    private float m_slotWidth;
    private float m_halfLength;
    private float m_halfWidth;
    private Transform m_slotsParent;
    private Box m_box;

    private void Awake()
    {
        m_box = GetComponent<Box>();
        m_slotsParent = transform.GetChild(0);
        CalculateBoxHalfDimension();
        CalculateSlotDimension();
        CreateSlots();
        SetAvailableSlots();
    }

    /// <summary> Calcule la moitié des longeurs de la boite pour le point de départ du placement des slots </summary>
    private void CalculateBoxHalfDimension()
    {
        m_halfLength = m_boxLength / 2;
        m_halfWidth = m_boxWidth / 2;
    }

    /// <summary> Calcule la dimension des slots </summary>
    void CalculateSlotDimension()
    {
        m_slotLength = m_boxLength / m_nbSlotLength;
        m_slotWidth = m_boxWidth / m_nbSlotWidth;
        slotLengthCalculation = m_slotLength;
        slotWidthCalculation = m_slotWidth;
    }

    void SetAvailableSlots()
    {
        m_box.InitAvailableSlots(m_nbSlotWidth * m_nbSlotLength);
    }

    /// <summary> Les calculs de positionement puis ajout dans la boite </summary>
    void CreateSlots()
    {
        List<float> slotsLengthPosition = new List<float>();
        List<float> slotsWidthPosition = new List<float>();

        FindLengthPositions(slotsLengthPosition);
        FindWidthPositions(slotsWidthPosition);
        PlaceSlotsInBox(slotsLengthPosition, slotsWidthPosition);
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
                instant.transform.localScale = new Vector3(m_slotLength, m_slotHeight, m_slotWidth);
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
