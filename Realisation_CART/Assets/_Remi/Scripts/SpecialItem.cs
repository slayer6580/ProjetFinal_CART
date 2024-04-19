using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class SpecialItem : MonoBehaviour
    {
        [field: SerializeField] private GameObject SpecialItemPrefab { get; set; } = null;
        [SerializeField] private float m_rotationSpeed = 0.25f;
        [SerializeField] private float m_oscillationSpan = 0.2f;
        [SerializeField] private float m_oscillationHeight = 1.5f;
        [SerializeField] private float m_scale = 1.5f;

        private const int PLAYER_BODY_LAYER = 3;

        private Vector3 m_oscillationPosition = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            if (SpecialItemPrefab == null)
            {
                Debug.LogWarning("SpecialItem: SpecialItemPrefab is not set");
                return;
            }

            SpecialItemPrefab = Instantiate(SpecialItemPrefab, transform);
            SpecialItemPrefab.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
        }

        // Update is called once per frame
        void Update()
        {
            if (SpecialItemPrefab == null) return;

            SpecialItemPrefab.transform.Rotate(Vector3.up, m_rotationSpeed);

            m_oscillationPosition = SpecialItemPrefab.transform.position;
            m_oscillationPosition.y = Mathf.Sin(Time.time) * m_oscillationSpan + m_oscillationHeight;
            SpecialItemPrefab.transform.position = m_oscillationPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != PLAYER_BODY_LAYER) return;
            // TODO: Pick up item
            Destroy(SpecialItemPrefab);
        }
    }
}
