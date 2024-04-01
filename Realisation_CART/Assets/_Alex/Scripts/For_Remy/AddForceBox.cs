using UnityEngine;

namespace DiscountDelirium
{
    public class AddForceBox : MonoBehaviour
    {
        [SerializeField] private float m_force;
        [SerializeField] private GameObject m_topBox;
        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.A))
            {
                Vector3 leftPush = -transform.right * m_force;
                m_topBox.GetComponent<Rigidbody>().AddForce(leftPush);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            { 
                Vector3 rightPush = transform.right * m_force;
                m_topBox.GetComponent<Rigidbody>().AddForce(rightPush);
            }

                // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
