using TMPro;
using UnityEngine;

namespace StatsSystem
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("starting cart token to give at player")]
        [SerializeField] private int m_startingCartToken;

        [Header("nb of token cost depending on stat level")]
        [SerializeField] private int[] m_upgradeCostByLevel;

        [Header("Mettre les TextMeshProUGUI du panel upgrade ci dessous")]
        [SerializeField] private TextMeshProUGUI m_nbOfCartTokenText;
        [SerializeField] private TextMeshProUGUI m_speedText;
        [SerializeField] private TextMeshProUGUI m_turningText;
        [SerializeField] private TextMeshProUGUI m_defenseText;
        [SerializeField] private TextMeshProUGUI m_turboText;

        private int m_nbCartToken;
        private static UpgradeManager Instance;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this.gameObject);
                return;
            }     

            Instance = this;

            m_nbCartToken = m_startingCartToken;
        }

        public UpgradeManager GetInstance()
        {
            return Instance;
        }

    }
}
