using UnityEngine;

namespace DiscountDelirium
{
    public class AddForceBox : MonoBehaviour
    {
        [SerializeField] private float m_force;
        [SerializeField] private Rigidbody m_rb;
        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.A))
            {
                Vector3 leftPush = -transform.right * m_force;
                m_rb.GetComponent<Rigidbody>().AddForce(leftPush);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            { 
                Vector3 rightPush = transform.right * m_force;
                m_rb.GetComponent<Rigidbody>().AddForce(rightPush);
            }

                // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
