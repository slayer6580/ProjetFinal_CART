using UnityEngine;
using BoxSystem;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Item m_itemPrefab;
    [SerializeField] private TowerBoxSystem m_tower;

    private void Start()
    {
        gameObject.name = "Item Spawner: " + m_itemPrefab.m_data.m_name;
    }

    //TEST
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_itemPrefab.m_data.m_size == ItemData.ESize.small)
        {
            TakeAnItemFromStorage(ItemData.ESize.small);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && m_itemPrefab.m_data.m_size == ItemData.ESize.medium)
        {
            TakeAnItemFromStorage(ItemData.ESize.medium);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && m_itemPrefab.m_data.m_size == ItemData.ESize.large)
        {
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

        GameObject instant = Instantiate(m_itemPrefab.gameObject);
        instant.transform.position = transform.position;
        m_tower.PutObjectInTopBox(instant);
    }

}
