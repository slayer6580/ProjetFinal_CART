using UnityEngine;

namespace DiscountDelirium
{
    public class BoxPhysics : MonoBehaviour
    {
        int m_groundLayer = 10;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != m_groundLayer) return; // If not ground return

            //Vector3 globalPosition = transform.position;
            //Quaternion globalRotation = transform.rotation;


            //transform.parent = collision.transform;

            //transform.position = globalPosition;
            //transform.rotation = globalRotation;

            //transform.localScale = Vector3.one;

            GetComponent<AutoDestruction>().enabled = true;
        }
    }
}
