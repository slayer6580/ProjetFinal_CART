using BoxSystem;
using UnityEngine;

namespace DiscountDelirium
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private TowerBoxSystem m_towerBoxSystem;

        public TowerBoxSystem GetTower() 
        {
            return m_towerBoxSystem;
        }
    }
}
