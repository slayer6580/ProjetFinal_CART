using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class CharacterController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float x  = Input.GetAxis("Horizontal") * 5 * Time.deltaTime;
            float z  = Input.GetAxis("Vertical") * 5 * Time.deltaTime;
            Vector3 direction = new Vector3(x, 0, z);
            transform.Translate(direction);
        }
    }
}
