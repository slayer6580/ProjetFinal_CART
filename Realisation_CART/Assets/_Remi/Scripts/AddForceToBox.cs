using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [SerializeField] private float m_impulseForce;
        [SerializeField] private float m_constantForce;
        private TowerBoxSystem2 Tower;


        void Awake()
        {
            Tower = GetComponent<TowerBoxSystem2>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                Vector3 leftPush = -transform.right * m_impulseForce;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Impulse);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                Vector3 rightPush = transform.right * m_impulseForce;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                Vector3 leftPush = -transform.right * m_constantForce;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush, ForceMode.Force);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                Vector3 rightPush = transform.right * m_constantForce;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush, ForceMode.Force);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log("G");
                Vector3 leftPush = -transform.right * m_impulseForce;

                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush);
            }
            else if (Input.GetKeyUp(KeyCode.H))
            {
                Debug.Log("H");
                Vector3 rightPush = transform.right * m_impulseForce;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush);
            }

            // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
