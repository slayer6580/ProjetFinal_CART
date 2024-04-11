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

        private void OnTriggerEnter(Collider other)
        {
            Shelf shelf = other.GetComponent<Shelf>();

            if (!shelf)
                return;

            if (!shelf.CanTakeItem())
                return;

            int nbOfItemsToGrab = shelf.GetRemainingItems();

            for (int i = 0; i < nbOfItemsToGrab; i++)            
                TakeItemFromShelf(shelf);
            
        }

        /// <summary> Take an item from the shelf </summary>
        public void TakeItemFromShelf(Shelf shelf)
        {
            _AudioManager.PlaySoundEffectsOneShot(ESound.GrabItem, transform.position, 0.1f);

            GameObject itemTaken = shelf.GetItemFromShelf();
            ItemData.ESize size = itemTaken.GetComponent<Item>().m_data.m_size; 

            if (!TowerBoxSystem.CanTakeObjectInTheActualBox(size))
            {
                Debug.Log("Need a new box to put item");
                TowerBoxSystem.AddBoxToTower();
            }
        
            TowerBoxSystem.PutObjectInTopBox(itemTaken);

        }
    }


}
