using UnityEngine;

namespace BoxSystem
{
    public class TowerPhysicsGenerator : MonoBehaviour
    {
        [field: SerializeField] private GameObject TowerPhysicsPrefab { get;  set; } = null;

        void Start()
        {

        }

        public GameObject GenerateTowerPhysics()
        {
            if (TowerPhysicsPrefab == null)
            {
                Debug.LogError("TowerPhysicsPrefab is null");
                return null;
            }

            GameObject towerPhysics = Instantiate(TowerPhysicsPrefab, transform);
            return towerPhysics;
        }
    }
}
