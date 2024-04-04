using UnityEngine;

namespace DiscountDelirium
{
    public class LocalPositionConstraint : MonoBehaviour
    {

        private void LateUpdate()
        {
            Vector3 lockedPosition = transform.localPosition;
            lockedPosition.x = transform.localPosition.x;
            lockedPosition.y = transform.localPosition.y;
            lockedPosition.z = 0;
            transform.localPosition = lockedPosition;
        }


    }
}
