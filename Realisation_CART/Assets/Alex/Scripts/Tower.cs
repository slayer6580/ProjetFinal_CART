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
        AddBoxeToTower();
    }

    public void AddBoxeToTower()
    {
        m_boxCount++;
        GameObject instant = Instantiate(boxPrefab);
        instant.transform.SetParent(transform);
        instant.name = "Boxe " + m_boxCount;
        Box instantBox = instant.GetComponent<Box>();
        float height = (m_boxCount - 1) * boxHeight;
        instant.transform.position = new Vector3(transform.position.x, height, transform.position.z);
        m_boxesInCart.Push(instantBox);
        //Debug.Log(m_boxesInCart.Count);
    }

    public void RemoveBoxeToTower()
    {
        if (m_boxCount == 1)
        {
            Debug.LogError("¨On peut pas enlever toute les boites du panier");
            return;
        }
           
        m_boxCount--;
        Box boxToRemove = m_boxesInCart.Pop();
        Destroy(boxToRemove.gameObject);
        //Debug.Log(m_boxesInCart.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddBoxeToTower();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            RemoveBoxeToTower();
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
            Debug.Log("TODO");
            return false;
        }
    }

    public void TakeObjectInActualBoxe(GameObject item, ItemData.ESize size)
    {
        GetTopBox().TakeItem(item, size);
    }

    private Box GetTopBox()
    {
        return m_boxesInCart.Peek();
    }
}
