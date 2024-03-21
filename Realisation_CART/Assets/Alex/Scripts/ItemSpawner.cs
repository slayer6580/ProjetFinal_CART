using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_smallPrefab;
    [SerializeField] private GameObject m_mediumPrefab;
    [SerializeField] private GameObject m_largePrefab;

    [SerializeField] private Box m_box;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))        
            SpawnItem(ItemData.ESize.small);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnItem(ItemData.ESize.medium);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SpawnItem(ItemData.ESize.large);
    }


    private void SpawnItem(ItemData.ESize size)
    {
        GameObject itemToSpawn;

        switch (size)
        {
            case ItemData.ESize.small:
                itemToSpawn = m_smallPrefab;    
                break;
            case ItemData.ESize.medium:
                itemToSpawn = m_mediumPrefab;   
                break;
            case ItemData.ESize.large:
                itemToSpawn = m_largePrefab;    
                break;
            default:
                itemToSpawn = m_largePrefab;
                break;
        }

        GameObject instant = Instantiate(itemToSpawn);
        instant.transform.position = transform.position;

        if (m_box.CanTakeItem(size))
        {
            m_box.TakeItem(instant, size);
            //TODO, l'item doit aller de lui meme vers son slot.
        }
    }
}
