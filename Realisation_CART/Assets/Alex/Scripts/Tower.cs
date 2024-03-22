using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float boxHeight;
    private int m_boxCount = 0;
    private Stack<Box> m_boxesInCart = new Stack<Box>();

    void Start()
    {
        AddBoxToTower();
    }

    public void AddBoxToTower()
    {
        m_boxCount++;
        GameObject instant = Instantiate(boxPrefab);
        instant.transform.SetParent(transform);
        instant.name = "Boxe " + m_boxCount;
        Box instantBox = instant.GetComponent<Box>();
        instantBox.SetTower(this);
        float height = (m_boxCount - 1) * boxHeight;
        instant.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        m_boxesInCart.Push(instantBox);
       // Debug.Log("boxes in tower: " + m_boxesInCart.Count);
    }

    public void RemoveBoxToTower()
    {
        if (m_boxCount == 1)
        {
            Debug.LogError("¨On peut pas enlever toute les boites du panier");
            return;
        }

        m_boxCount--;
        Box boxToRemove = m_boxesInCart.Pop();
        Destroy(boxToRemove.gameObject);
       // Debug.Log("boxes in tower: " + m_boxesInCart.Count);
    }

    // TEST
    private void Update()
    {     
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddBoxToTower();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            RemoveBoxToTower();
        }
    }

    public bool CanTakeObjectInTheActualBox(ItemData.ESize size)
    {
        if (size == ItemData.ESize.small)
        {
            return GetTopBox().CanTakeItem(size);
        }
        else
        {
            Debug.Log("TODO, NOT SUPPOSED TO HAPPEN");
            return false;
        }
    }

    public void PutObjectInTopBox(GameObject item, ItemData.ESize size)
    {
        GetTopBox().TakeItem(item, size);
    }

    private Box GetTopBox()
    {
        return m_boxesInCart.Peek();
    }
}
