using TMPro;
using UnityEngine;

namespace BoxSystem
{
    public class Shelf : MonoBehaviour
    {
        [Header("mettre l'item que l'étagere donne ici")]
        [SerializeField] private GameObject m_itemPrefab;
        [Header("la quantité de l'item")]
        [SerializeField] private int m_itemQuantity;
        [Header("mettre le debug visuel TMP ici")]
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

        /// <summary> Est ce qu'il reste un item dans l'étagère </summary>
        public bool CanTakeItem()
        {
            return m_remainingItems > 0 ? true : false;

        }

        /// <summary> Prendre une instance d'un item </summary>
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


        /// <summary> Change la couleur du shelf selon la grosseur de l'item </summary>
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


        /// <summary> change la couleur du shelf pour montrer que c'est la choisi </summary>
        public void SelectedShelf()
        {
            GetComponent<MeshRenderer>().material.color = Color.black;
        }

        /// <summary> change la couleur du shelf pour son originale pour montrer qu'elle n'es plu choisi </summary>
        public void UnSelectedShelf()
        {
            GetComponent<MeshRenderer>().material.color = m_initialColor;
        }
    }

  

}
        