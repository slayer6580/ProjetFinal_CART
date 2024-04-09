using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class Cursor : MonoBehaviour
    {
        public void Move(Vector2 direction)
        {
            //Debug.Log("Direction: " + direction);
            Vector3 vec3 = new Vector3(direction.x, direction.y, 0);
            transform.position += vec3 * 10.0f;
        }
    }
}
