using UnityEngine;

namespace BackstoreScale
{
    public class FloorTriggerCaller : MonoBehaviour
    {
        public BackstoreDoorSystem m_listener;
        [SerializeField] private bool m_isInsideBackroom = false;

        private void Awake()
        {
            m_listener = GetComponentInParent<BackstoreDoorSystem>();
            if (m_listener == null)
            {
                Debug.LogError("FloorTriggerCaller: No BackstoreDoorSystem found in parent");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            m_listener.m_onTriggerEnter.Invoke(other, m_isInsideBackroom);
        }

        private void OnTriggerExit(Collider other)
        {
            m_listener.m_onTriggerExit.Invoke(other, m_isInsideBackroom);
        }
    }
}
