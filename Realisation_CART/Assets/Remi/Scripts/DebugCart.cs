using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace DiscountDelirium
{
    public class DebugCart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            GetComponent<Rigidbody>().AddForce(transform.forward
                                            * GameConstants.BASE_ADD_FORCE
                                            * Time.fixedDeltaTime
                                        );
        }
    }
}
