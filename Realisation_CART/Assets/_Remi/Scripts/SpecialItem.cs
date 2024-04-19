using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class SpecialItem : MonoBehaviour
    {
        [field: SerializeField] private GameObject SpecialItemPrefab { get; set; } = null;


        // Start is called before the first frame update
        void Start()
        {
            if (SpecialItemPrefab == null)
            {
                Debug.LogWarning("SpecialItem: SpecialItemPrefab is not set");
                return;
            }

            SpecialItemPrefab = Instantiate(SpecialItemPrefab, transform);
        }

        // Update is called once per frame
        void Update()
        {
            if (SpecialItemPrefab == null) return;

            SpecialItemPrefab.transform.Rotate(Vector3.up, 0.25f);
            SpecialItemPrefab.transform.position = new Vector3(SpecialItemPrefab.transform.position.x, Mathf.Sin(Time.time) * 0.2f + 1, SpecialItemPrefab.transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO: Pick up item
        }
    }
}
