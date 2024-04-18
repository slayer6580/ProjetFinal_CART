using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    [RequireComponent(typeof(Shelf))]
    public class ShelfSetup : MonoBehaviour
    {

        [Tooltip("limited based on transform list size")]
        [SerializeField] private float m_nbOfItems;

        [Header("Items transforms, Don't touch")]
        [SerializeField] private List<Transform> m_smallItemTransform;
        [SerializeField] private List<Transform> m_mediumItemTransform;
        [SerializeField] private List<Transform> m_largeItemTransform;

        private Shelf m_shelf;

        private void Awake()
        {
            m_shelf = GetComponent<Shelf>();
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
