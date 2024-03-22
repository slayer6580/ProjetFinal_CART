using UnityEngine;
using BoxSystem;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_smallPrefab;
    [SerializeField] private GameObject m_mediumPrefab;
    [SerializeField] private GameObject m_largePrefab;

    [SerializeField] private Tower m_tower;
    [SerializeField] private ItemData.ESize m_sizeOfObjects;
    private GameObject m_itemPrefab;

    //TEST
    private void Start()
    {
        gameObject.name = "Item Spawner: " + m_sizeOfObjects.ToString();
    }

    //TEST
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_sizeOfObjects == ItemData.ESize.small)
        {
            m_itemPrefab = m_smallPrefab;
            TakeAnItemFromStorage(ItemData.ESize.small);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && m_sizeOfObjects == ItemData.ESize.medium)
        {
            m_itemPrefab = m_mediumPrefab;
            TakeAnItemFromStorage(ItemData.ESize.medium);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && m_sizeOfObjects == ItemData.ESize.large)
        {
            m_itemPrefab = m_largePrefab;
            TakeAnItemFromStorage(ItemData.ESize.large);
        }

    }


    private void TakeAnItemFromStorage(ItemData.ESize size)
    {
        if (!m_tower.CanTakeObjectInTheActualBox(size))
        {
            Debug.Log("Need a new box to put item");
            m_tower.AddBoxToTower();
        }

        GameObject instant = Instantiate(m_itemPrefab);
        instant.transform.position = transform.position;
        m_tower.PutObjectInTopBox(instant, size);
    }

}
