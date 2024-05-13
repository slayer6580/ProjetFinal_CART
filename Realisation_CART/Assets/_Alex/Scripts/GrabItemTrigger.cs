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
        private Shelf shelf;


		private void OnTriggerEnter(Collider other)
        {
			shelf = other.GetComponent<Shelf>();

            if (!shelf)
                return;

            if (!m_canGrabItem || !shelf.CanTakeItem())
                return;

            for(int i = 0; i < shelf.GetItemRemaining(); i++)
            {
				TakeItemFromShelf(shelf);
			}
           
          
        }


        /// <summary> Take an item from the shelf </summary>
        public void TakeItemFromShelf(Shelf shelf)
        {
            _AudioManager.PlaySoundEffectsOneShot(ESound.GrabItem, transform.position, 0.1f);

            GameObject itemTaken = shelf.GetItemFromShelf();
            Item itemScript = itemTaken.GetComponent<Item>();
          // ItemData.ESize size = itemScript.m_data.m_size;

            // TODO commencer le slerp vers le joueur

            itemScript.StartSlerpTowardTower(TowerBoxSystem.gameObject, TowerBoxSystem.ItemSnapDistance, ItemSlerpTime);


          //  TakeItem(itemTaken, size); //OLD
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

            Item itemScript = itemTaken.GetComponent<Item>();

            BoxCollider modelCollider = itemTaken.transform.GetChild(0).GetComponent<BoxCollider>();

            if (modelCollider != false)
            {
                modelCollider.enabled = false;
            }

            itemScript.StartSlerpTowardTower(TowerBoxSystem.gameObject, TowerBoxSystem.ItemSnapDistance, ItemSlerpTime);
    
        }

        //private void TakeItem(GameObject itemTaken, ItemData.ESize size)
        //{
        //    Rigidbody rb = itemTaken.GetComponent<Rigidbody>();
        //    if (rb)
        //    {
        //        Destroy(rb);
        //    }

        //    if (!TowerBoxSystem.CanTakeObjectInTheActualBox(size))
        //    {
        //        TowerBoxSystem.AddBoxToTower();
        //    }

        //    TowerBoxSystem.PutObjectInTopBox(itemTaken);
        //}

        private void ActivateGrabItem()
        {
            m_canGrabItem = true;
        }
    }

}
