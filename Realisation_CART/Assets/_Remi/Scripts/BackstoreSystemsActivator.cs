using UnityEngine;

namespace BackstoreSystems
{
    public class BackstoreSystemsActivator : MonoBehaviour
    {
        [field: SerializeField] private GameObject BackstoreSystemsGO { get; set; } = null;
        [field: SerializeField] private ScaleSystem _BackstoreDoorSystem { get; set; } = null;

        private bool m_AreBackstoreSystemsActive = false;

        private void OnTriggerEnter(Collider other)
        {
            m_AreBackstoreSystemsActive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            m_AreBackstoreSystemsActive = false;
        }

        private void Update()
        {
            if (m_AreBackstoreSystemsActive)
            {
                BackstoreSystemsGO.SetActive(true);
            }
            else if (!m_AreBackstoreSystemsActive && !_BackstoreDoorSystem.GetIsScaleSystemActive())
            {
                BackstoreSystemsGO.SetActive(false);
            }
        }
    }
}
