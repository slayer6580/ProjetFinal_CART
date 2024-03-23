using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class DebugControls : MonoBehaviour
    {
        public Rigidbody RB { get; set; }
        [field: SerializeField]private Camera Camera { get; set; }
        private float m_speed = 500.0f;


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (RB == null) return;

            Vector3 newDirection = Vector3.zero;
            Vector3 cameraForward = Vector3.ProjectOnPlane(Camera.transform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.ProjectOnPlane(Camera.transform.right, Vector3.up).normalized;

            //if (!Input.anyKey)
            //{
            //    RB.velocity = Vector3.zero;
            //}

            if (Input.GetKeyDown(KeyCode.W))
            {
                //Debug.Log("Pressed W");
                newDirection += cameraForward * m_speed;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //Debug.Log("Pressed S");
                newDirection -= cameraForward * m_speed;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //Debug.Log("Pressed A");
                newDirection -= cameraRight * m_speed;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                //Debug.Log("Pressed D");
                newDirection += cameraRight * m_speed;
            }

            RB.AddForce(newDirection, ForceMode.Acceleration);
        }
    }
}
