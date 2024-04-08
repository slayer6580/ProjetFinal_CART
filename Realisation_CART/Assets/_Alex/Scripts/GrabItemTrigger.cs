using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class GrabItemTrigger : MonoBehaviour
    {
        [field: Header("le temps maximal que prend un item pour venir dans le panier")]
        [field: SerializeField] public float ItemSlerpTime { get; private set; }

        [field: Header("mettre la tour du player ici")]
        [field: SerializeField] public TowerBoxSystem TowerBoxSystem { get; private set; }

        [Header("Choose shelf calcul weight")]
        [SerializeField] [Range(0.0f, 30.0f)] private float m_sideDotProductImpact;
        [SerializeField] [Range(0.0f, 10.0f)] private float m_distanceImpact;
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
            Debug.LogWarning("Detect Shelf");
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

            float bestScore = float.NegativeInfinity;
            Shelf closestShelf = null;

            foreach (Shelf shelf in m_takableShelves)
            {
             
                if (!shelf.CanTakeItem())
                    continue;

                float closestDotProduct = -2;         

                Vector3 vectorToShelf = (shelf.transform.position - transform.position).normalized;
                float leftDotProduct = Vector3.Dot(vectorToShelf, -transform.right);
                float rightDotProduct = Vector3.Dot(vectorToShelf, transform.right);
                closestDotProduct = Mathf.Max(leftDotProduct, rightDotProduct);
                float shelfDistance = Vector3.Distance(transform.position, shelf.transform.position);
                float score = (closestDotProduct * m_sideDotProductImpact) + (-shelfDistance * m_distanceImpact);

                if (score > bestScore)
                {
                    bestScore = score;
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

            if (!TowerBoxSystem.CanTakeObjectInTheActualBox(size))
            {
                Debug.Log("Need a new box to put item");
                TowerBoxSystem.AddBoxToTower();
            }
        
            TowerBoxSystem.PutObjectInTopBox(itemTaken);

        }
    }


}
