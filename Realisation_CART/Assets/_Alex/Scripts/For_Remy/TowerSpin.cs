using UnityEngine;

namespace DiscountDelirium
{
    public class TowerSpin : MonoBehaviour
    {
        [SerializeField] private float m_turnSpeed;


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Vector3.up * m_turnSpeed);
        }
    }
}
