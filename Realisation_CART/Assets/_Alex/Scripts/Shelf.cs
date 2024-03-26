using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace BoxSystem
{
    public class Shelf : MonoBehaviour
    {
        [SerializeField] private GameObject m_itemPrefab;
        [SerializeField] private int m_itemQuantity;
        [SerializeField] private TextMeshPro m_quantityText;
        private int m_remainingItems;
        private Color m_initialColor;

        private void Awake()
        {

            m_remainingItems = m_itemQuantity;
            m_quantityText.text = m_itemQuantity.ToString();
            ColorShelf();
            m_initialColor = GetComponent<MeshRenderer>().material.color;
            
        }

        public bool CanTakeItem()
        {
            return m_remainingItems > 0 ? true : false;

        }

        public GameObject GetItemFromShelf()
        {
            m_remainingItems--;
            GameObject instant = Instantiate(m_itemPrefab);
            instant.transform.position = transform.position;
            m_quantityText.text = m_remainingItems.ToString();
            return instant;
        }


        private void OnValidate()
        {
            m_quantityText.text = m_itemQuantity.ToString();
            ColorShelf();
        }

   

        private void ColorShelf()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            switch (m_itemPrefab.GetComponent<Item>().m_data.m_size)
            {
                case ItemData.ESize.small:
                    meshRenderer.sharedMaterial.color = Color.green;
                    break;
                case ItemData.ESize.medium:
                    meshRenderer.sharedMaterial.color = Color.yellow;
                    break;
                case ItemData.ESize.large:
                    meshRenderer.sharedMaterial.color = Color.red;
                    break;
                default:
                    meshRenderer.sharedMaterial.color = Color.white;
                    break;
            }
        }



        public void SelectedShelf()
        {
            GetComponent<MeshRenderer>().material.color = Color.black;
        }

        public void UnSelectedShelf()
        {
            GetComponent<MeshRenderer>().material.color = m_initialColor;
        }
    }

  

}
        