using UnityEngine;
using BoxSystem;
using DiscountDelirium;

public class ItemSpawner1 : MonoBehaviour
{
    [SerializeField] private Item1 m_itemPrefab;
    [SerializeField] private Tower1 m_tower; // Warning : TowerPhysics bellow depends on this prefab
    private TowerPhysics m_towerPhysics;

    private void Start()
    {
        gameObject.name = "Item Spawner: " + m_itemPrefab.m_data.m_name;
        m_towerPhysics = m_tower.GetComponent<TowerPhysics>();
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
