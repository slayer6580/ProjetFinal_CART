using UnityEngine;

namespace BackstoreScale
{
    public class FloorTriggerCaller : MonoBehaviour
    {
        public BackstoreDoorSystem listener;
        GameObjectData gameObjectData = new GameObjectData();
        [SerializeField] private bool m_isInsideBackRoom = false;

        private void Awake()
        {
            listener = GetComponentInParent<BackstoreDoorSystem>();
            if (listener == null)
            {
                Debug.LogError("FloorTriggerCaller: No BackstoreDoorSystem found in parent");
            }

            gameObjectData.IsInsideBackRoom = m_isInsideBackRoom;
        }

        private void OnTriggerEnter(Collider other)
        {
            listener.onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            listener.onTriggerExit.Invoke(other);
        }
    }
}
