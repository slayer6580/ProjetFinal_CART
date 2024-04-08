using UnityEngine;

namespace BoxSystem
{
    public class ShelfManager : MonoBehaviour
    {

        [SerializeField] private Shelf[] m_shelves;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            { 
                ResetAllShelves();
            }
        }

        public void ResetAllShelves()
        {
            foreach (Shelf shelf in m_shelves)
            {
                shelf.ResetShelf();
            }
        }
    }
}
