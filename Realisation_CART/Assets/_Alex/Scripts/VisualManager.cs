using UnityEngine;

namespace StatsSystem
{
    public class VisualManager : MonoBehaviour
    {
        [Header("All Materials")]
        [SerializeField] private Material[] m_cartMaterials;
        [SerializeField] private Material[] m_wheelsMaterials;
        [SerializeField] private Material[] m_shoesMaterials;

        [Header("All Parts")]
        [SerializeField] MeshRenderer[] m_cartParts;
        [SerializeField] MeshRenderer[] m_wheelParts;
        [SerializeField] MeshRenderer[] m_shoesParts;

        private static VisualManager Instance;

        private void Awake()
        {
            if (Instance == null)            
                Instance = this;            
        }

        public static VisualManager GetInstance()
        {
            return Instance;
        }

        private void Start()
        {
            UpdateAll();
        }

        private void UpdateWheels()
        {
            int wheelUpgrade = PlayerPrefs.GetInt("Handling", 0);
        
            foreach (var wheelPart in m_wheelParts)
                wheelPart.material = m_wheelsMaterials[wheelUpgrade];

        }

        private void UpdateCart()
        {
            int cartUpgrade = PlayerPrefs.GetInt("Balance", 0);

            foreach (var cartPart in m_cartParts)
                cartPart.material = m_cartMaterials[cartUpgrade];

        }

        private void UpdateShoes()
        {
            int shoesUpgrade = PlayerPrefs.GetInt("Acceleration", 0);

            foreach (var shoePart in m_shoesParts)
                shoePart.material = m_shoesMaterials[shoesUpgrade];
        }

        public void UpdateAll()
        {
            UpdateCart();
            UpdateShoes();
            UpdateWheels();
        }

    }
}
