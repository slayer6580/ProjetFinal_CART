using DiscountDelirium;
using UnityEngine;

namespace BoxSystem
{
    public class Item : MonoBehaviour
    {
      
        public ItemData m_data;
        private Box m_box;
        private Transform m_playerTransform;
        private Vector3 m_targetLocalPosition;
        private Vector3 m_startPosition;

        private float m_timer = 0;
        private float m_distanceSnap;
        private bool m_isMoving = false;
        private bool m_turn90Degree = false;
        private float m_slerpTime;

        private Transform m_boxTransform; // NEW

        private void Awake()
        {
            gameObject.name = m_data.name;
            enabled = false;
        }

        private void Update()
        {
            Slerping();
        }

        public void StartSlerpAndSnap(Box box, Vector3 localPosition, GameObject player, bool turn90Degree, float snapDistance, bool autoSnap)
        {
            m_turn90Degree = turn90Degree;
            m_playerTransform = player.transform;           
            m_targetLocalPosition = localPosition;          
            m_box = box;
            m_boxTransform = box.transform;
            m_startPosition = transform.position;
            m_slerpTime = player.GetComponent<PlayerGrabItem>().ItemSlerpTime;
            if (autoSnap) // apres un reorganize
            {
                SnapToBox();
                return;
            }
            m_isMoving = true;

            m_distanceSnap = snapDistance;
            enabled = true;
        }

        private void SnapToBox()
        {
            transform.SetParent(m_boxTransform);
            transform.localPosition = m_targetLocalPosition;
            Vector3 eulerOfCart = m_playerTransform.rotation.eulerAngles; 
            Vector3 localEulerOfBox = m_boxTransform.transform.localRotation.eulerAngles;

            transform.eulerAngles = Vector3.zero;
            transform.eulerAngles += eulerOfCart;
            transform.eulerAngles += localEulerOfBox; 

            if (m_turn90Degree)
                transform.eulerAngles += new Vector3(0, 90, 0);

            EndSlerp();
        }

        private void EndSlerp()
        {
            Debug.Log("Slerp Ended");
            enabled = false;
        }

        private void Slerping()
        {
            if (!m_isMoving)
                return;

            

            float slerpTime = m_timer / m_slerpTime;
            m_timer += Time.deltaTime;

            Vector3 boxLocalPosition = m_box.transform.position + m_targetLocalPosition;

            transform.position = Vector3.Slerp(m_startPosition, boxLocalPosition, slerpTime);
            float targetDistance = Mathf.Abs(Vector3.Distance(transform.position, boxLocalPosition));

            if (targetDistance < m_distanceSnap)
            {
                m_isMoving = false;
                SnapToBox();
                return;
            }

        }

    }
}
