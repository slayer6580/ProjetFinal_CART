using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [SerializeField] private float m_impulseForce;
        [SerializeField] private float m_constantForce;
        private TowerHingePhysicsAlex m_towerPhysics;

        private void Awake()
        {
            m_towerPhysics = GetComponent<TowerHingePhysicsAlex>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                Vector3 leftPush = -transform.right * m_impulseForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                Vector3 rightPush = transform.right * m_impulseForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 leftPush = -transform.right * m_constantForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Force);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 rightPush = transform.right * m_constantForce;
                m_towerPhysics.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Force);
            }

            // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
