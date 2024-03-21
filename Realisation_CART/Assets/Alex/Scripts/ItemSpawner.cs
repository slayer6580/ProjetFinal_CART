using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_smallPrefab;
    [SerializeField] private GameObject m_mediumPrefab;
    [SerializeField] private GameObject m_largePrefab;

    [SerializeField] private Tower m_tower;
    [SerializeField] private ItemData.ESize m_sizeOfObjects;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))        
            TakeItem(ItemData.ESize.small);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            TakeItem(ItemData.ESize.medium);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            TakeItem(ItemData.ESize.large);
    }


    private void TakeItem(ItemData.ESize size)
    {
     
    }
}
