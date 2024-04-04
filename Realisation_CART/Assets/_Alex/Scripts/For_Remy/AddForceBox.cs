using UnityEngine;

namespace DiscountDelirium
{
    public class AddForceBox : MonoBehaviour
    {
        [SerializeField] private float m_impulseForce;
        [SerializeField] private float m_constantForce;
        [SerializeField] private GameObject m_topBox;
        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.Z))
            {
                Vector3 leftPush = -transform.right * m_impulseForce;
                m_topBox.GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            { 
                Vector3 rightPush = transform.right * m_impulseForce;
                m_topBox.GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 leftPush = -transform.right * m_constantForce;
                m_topBox.GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Force);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 rightPush = transform.right * m_constantForce;
                m_topBox.GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Force);
            }

                // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
