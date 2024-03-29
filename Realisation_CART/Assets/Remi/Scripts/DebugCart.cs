using UnityEngine;

namespace DiscountDelirium
{
    public class DebugCart : MonoBehaviour
    {
        private float m_speed = 3.0f;

        void Start()
        {
            Invoke("DestroyItem", 2);
        }

        /// <summary> D�truit l'objet </summary>
        private void DestroyItem()
        {
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward
                                            * GameConstants.BASE_ADD_FORCE
                                            * m_speed
                                            * Time.fixedDeltaTime
                                        );
        }

        /// <summary> D�finit la vitesse de l'objet </summary>
        public void SetSpeed(float speed)
        {
            m_speed = speed;
        }
    }
}
