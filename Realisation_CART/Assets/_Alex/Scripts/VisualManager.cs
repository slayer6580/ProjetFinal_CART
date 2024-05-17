using UnityEngine;

namespace StatsSystem
{
    public class VisualManager : MonoBehaviour
    {
        [Header("All Materials")]
        [SerializeField] private Material[] m_cartMaterials;
        [SerializeField] private Material[] m_wheelsMaterials;
        [SerializeField] private Material[] m_shoesMaterials;
        [SerializeField] private Material[] m_rocketMaterials;

        [Header("All Parts")]
        [SerializeField] private MeshRenderer[] m_cartParts;
        [SerializeField] private MeshRenderer[] m_wheelParts;
        [SerializeField] private MeshRenderer[] m_rocketParts;
        [SerializeField] private SkinnedMeshRenderer m_shoePart;

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

            Material[] materials = m_shoePart.materials;
            materials[2] = m_shoesMaterials[shoesUpgrade];
            m_shoePart.sharedMaterials = materials;                                           
        }

        private void UpdateRockets()
        {
            int rocketUpgrade = PlayerPrefs.GetInt("MaxSpeed", 0);

            foreach (var rocketPart in m_rocketParts)
                rocketPart.material = m_rocketMaterials[rocketUpgrade];
        }

        public void UpdateAll()
        {
            UpdateCart();
            UpdateShoes();
            UpdateWheels();
            UpdateRockets();
        }

    }
}
