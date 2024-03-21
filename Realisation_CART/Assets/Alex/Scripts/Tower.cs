using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private float boxHeight;
    private int m_boxCount = 0;
    private List<Box> m_boxesInCart = new List<Box>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
