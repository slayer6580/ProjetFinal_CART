using UnityEngine;

namespace BoxSystem
{
    public class AddForceToBox : MonoBehaviour
    {
        [SerializeField] private float m_force;
        private TowerBoxSystem Tower;


        void Awake()
        {
            Tower = GetComponent<TowerBoxSystem>();
        }

        private void Update()
        {

            if (Input.GetKeyUp(KeyCode.G))
            {
                Vector3 leftPush = -transform.right * m_force;

                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(leftPush);
            }
            else if (Input.GetKeyUp(KeyCode.H))
            {
                Vector3 rightPush = transform.right * m_force;
                Tower.GetTopBox().GetComponent<Rigidbody>().AddForce(rightPush);
            }

            // DOIT AJOUTER DE LA FORCE SUR LA DERNIERE BOITE
        }
    }
}
