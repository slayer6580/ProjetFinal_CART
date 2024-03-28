using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class GrabItemTrigger : MonoBehaviour
    {
        [field: Header("le temps maximal que prend un item pour venir dans le panier")]
        [field: SerializeField] public float ItemSlerpTime { get; private set; }

        [field: Header("mettre la tour du player ici")]
        [field: SerializeField] public TowerBoxSystem Tower { get; private set; }

        private List<Shelf> m_takableShelves = new List<Shelf>();
        private Shelf m_choosenShelf = null;

        private void Update()
        {
            UpdateBestShelfOption();
        }

        private void OnTriggerEnter(Collider other)
        {
            Shelf shelf = other.GetComponent<Shelf>();

            if (!shelf)
                return;
            m_takableShelves.Add(shelf);
        }

        private void OnTriggerExit(Collider other)
        {
            Shelf shelf = other.GetComponent<Shelf>();

            if (!shelf)
                return;
         
            m_takableShelves.Remove(shelf);

            if (m_choosenShelf == shelf)
            {
                m_choosenShelf.UnSelectedShelf();
                m_choosenShelf = null;               
            }
        }

        /// <summary>  Regarde qu'elle Shelf es la plus proche du Dot Product du vector vers l'avant avant du cart  </summary>
        private void UpdateBestShelfOption()
        {
            if (m_takableShelves.Count == 0)
                return;

            float closestDotProduct = -2;

            Shelf closestShelf = null;

            foreach (Shelf shelf in m_takableShelves)
            {
                if (!shelf.CanTakeItem())
                    continue;

                Vector3 vectorToShelf = (shelf.transform.position - transform.position).normalized;
                float dotProduct = Vector3.Dot(vectorToShelf, transform.forward);

                if (dotProduct > closestDotProduct)
                {
                    closestDotProduct = dotProduct;
                    closestShelf = shelf;
                }
            }

            // si c'est le meme qu'on avait
            if (m_choosenShelf == closestShelf)
                return;

            // si il en avait deja un avant
            if (m_choosenShelf)
                m_choosenShelf.UnSelectedShelf();

            m_choosenShelf = closestShelf;

            // si il en a un nouveau
            if (m_choosenShelf)
                m_choosenShelf.SelectedShelf();
        }


        /// <summary> Prendre un item du Shelf Choisi </summary>
        public void TakeItemFromShelf()
        {
            if (m_choosenShelf == null)
                return;

            GameObject itemTaken = m_choosenShelf.GetItemFromShelf();
            ItemData.ESize size = itemTaken.GetComponent<Item>().m_data.m_size; 

            if (!Tower.CanTakeObjectInTheActualBox(size))
            {
                Debug.Log("Need a new box to put item");
                Tower.AddBoxToTower();
            }
        
            Tower.PutObjectInTopBox(itemTaken);

        }
    }


}
