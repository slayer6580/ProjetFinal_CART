using UnityEngine;

namespace BoxSystem
{
    public class MovingCartDebug : MonoBehaviour
    {
        [Header("Translation")]
        [SerializeField][Range(0.01f, 0.05f)] private float m_translationSpeed;
        [SerializeField][Range(0.0f, 5.00f)] private float m_translationLenght;

        [Header("Rotation")]
        [SerializeField][Range(0.00f, 1.00f)] private float m_rotationSpeed;

        bool m_positiveDirection = true;

        void Update()
        {
            LinearTranslation();

            //rotation
            transform.Rotate(Vector3.up * m_rotationSpeed);

        }

        private void LinearTranslation()
        {
            if (transform.position.x > m_translationLenght && m_positiveDirection)
                m_positiveDirection = false;
            else if (transform.position.x < -m_translationLenght && !m_positiveDirection)
                m_positiveDirection = true;


            if (m_positiveDirection)
                transform.Translate(Vector3.right * m_translationSpeed, Space.World);
            else
                transform.Translate(Vector3.left * m_translationSpeed, Space.World);
        }
    }
}
