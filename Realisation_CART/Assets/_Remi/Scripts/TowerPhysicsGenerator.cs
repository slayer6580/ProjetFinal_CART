using UnityEngine;

namespace BoxSystem
{
    public class TowerPhysicsGenerator : MonoBehaviour
    {
        [field: SerializeField] private GameObject TowerPhysicsPrefab { get;  set; } = null;
        // Position offset for the tower physics
        [field: SerializeField] public float TowerPhysicsSpawningPositionOffset { get; private set; } = 10;
        private static float m_towerPhysicsOffset = 0;
        private Vector3 m_newPosition = new Vector3(0, 0, 0);

        private static int m_characterIDIterator = 0;
        private int m_characterID = 0;

        public GameObject GenerateTowerPhysics()
        {
            m_characterID = m_characterIDIterator++;
            m_towerPhysicsOffset = TowerPhysicsSpawningPositionOffset * m_characterID;

            if (TowerPhysicsPrefab == null)
            {
                Debug.LogError("TowerPhysicsPrefab is null");
                return null;
            }

            GameObject towerPhysics = Instantiate(TowerPhysicsPrefab, transform);
            m_newPosition = towerPhysics.transform.position;
            m_newPosition.z += m_towerPhysicsOffset;
            towerPhysics.transform.position = m_newPosition;
            return towerPhysics;
        }
    }
}
