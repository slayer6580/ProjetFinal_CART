using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_smallPrefab;
    [SerializeField] private GameObject m_mediumPrefab;
    [SerializeField] private GameObject m_largePrefab;

    [SerializeField] private Tower m_tower;
    [SerializeField] private ItemData.ESize m_sizeOfObjects;
    private GameObject m_itemPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_sizeOfObjects == ItemData.ESize.small)
        {
            m_itemPrefab = m_smallPrefab;
            TryToTakeItem(ItemData.ESize.small);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && m_sizeOfObjects == ItemData.ESize.medium)
        {
            m_itemPrefab = m_mediumPrefab;
            TryToTakeItem(ItemData.ESize.medium);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && m_sizeOfObjects == ItemData.ESize.large)
        {
            m_itemPrefab = m_largePrefab;
            TryToTakeItem(ItemData.ESize.large);
        }

    }

    private void TryToTakeItem(ItemData.ESize size)
    {
        if (m_tower.CanTakeObjectInTheActualBox(size))
        {
            GameObject instant = Instantiate(m_itemPrefab);
            instant.transform.SetParent(m_tower.transform);
            instant.transform.position = transform.position;
            m_tower.TakeObjectInActualBoxe(m_itemPrefab, size);
        }
    }
}
