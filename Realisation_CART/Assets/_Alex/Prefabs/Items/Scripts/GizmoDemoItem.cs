using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxSystem
{
    public class GizmoDemoItem : MonoBehaviour
    {

        private Vector3 m_boxSize;
        private Color m_color;
        [SerializeField] ItemData.ESize m_size;



        private void OnValidate()
        {
            switch (m_size)
            {
                case ItemData.ESize.small:
                    m_boxSize = Vector3.one;
                    m_color = new Color(0, 1, 0, 0.1f);
                    break;
                case ItemData.ESize.medium:
                    m_boxSize = new Vector3(2, 1, 1);
                    m_color = new Color(1, 1, 0, 0.1f);
                    break;
                case ItemData.ESize.large:
                    m_boxSize = new Vector3(2, 1, 2);
                    m_color = new Color(1, 0, 0, 0.1f);
                    break;
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = m_color;
            Gizmos.DrawCube(transform.position, m_boxSize);
        }
    }
}
