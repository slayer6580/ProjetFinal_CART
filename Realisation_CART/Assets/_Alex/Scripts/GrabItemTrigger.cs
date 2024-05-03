using System.Collections;
using UnityEngine;
using static Manager.AudioManager;

namespace BoxSystem
{
    public class GrabItemTrigger : MonoBehaviour
    {
        [field: Header("Maximal Second of slerp time before snapping")]
        [field: SerializeField] public float ItemSlerpTime { get; private set; }

        [field: Header("Put the tower Box System here")]
        [field: SerializeField] public TowerBoxSystem TowerBoxSystem { get; private set; }

        [Header("Stole animation time")]
        [SerializeField] public float m_animationTime;

        [Header("Shelf grab delay")]
        [SerializeField] private float m_grabDelay;

        private bool m_canGrabItem = true;

        private void OnTriggerStay(Collider other)
        {
            Shelf shelf = other.GetComponent<Shelf>();

            if (!shelf)
                return;

            if (!m_canGrabItem || !shelf.CanTakeItem())
                return;

            TakeItemFromShelf(shelf);

            m_canGrabItem = false;
            Invoke("ActivateGrabItem", m_grabDelay);

        }

        /// <summary> Take an item from the shelf </summary>
        public void TakeItemFromShelf(Shelf shelf)
        {
            _AudioManager.PlaySoundEffectsOneShot(ESound.GrabItem, transform.position, 0.1f);

            GameObject itemTaken = shelf.GetItemFromShelf();
            ItemData.ESize size = itemTaken.GetComponent<Item>().m_data.m_size;

            TakeItem(itemTaken, size);
        }

        public void StealItemFromOtherTower(TowerBoxSystem towerToSteal)
        {
            if (towerToSteal.GetBoxCount() == 0)
                return;

            GameObject itemTaken = towerToSteal.GetStolenItem();

            if (itemTaken == null)
                return;

            ItemData.ESize size = itemTaken.GetComponent<Item>().m_data.m_size;

            StartCoroutine(WaitForAnimation(itemTaken, size));
        }

        IEnumerator WaitForAnimation(GameObject itemTaken, ItemData.ESize size)
        {
            yield return new WaitForSeconds(m_animationTime);
            TakeItem(itemTaken, size);
        }

        private void TakeItem(GameObject itemTaken, ItemData.ESize size)
        {
            Rigidbody rb = itemTaken.GetComponent<Rigidbody>();
            if (rb)
            {
                Destroy(rb);
            }

            if (!TowerBoxSystem.CanTakeObjectInTheActualBox(size))
            {
                TowerBoxSystem.AddBoxToTower();
            }

            TowerBoxSystem.PutObjectInTopBox(itemTaken);
        }

        private void ActivateGrabItem()
        {
            m_canGrabItem = true;
        }
    }

}
