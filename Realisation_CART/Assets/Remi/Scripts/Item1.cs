using UnityEngine;

namespace BoxSystem
{
    public class Item1 : MonoBehaviour
    {
        [SerializeField] private float m_distanceSnap;

        public ItemData m_data;
        private Box1 m_box;
        private Transform m_playerTransform;
        private Vector3 m_targetLocalPosition;

        private float m_timer = 0;
        private bool m_isMoving = false;
        private bool m_turn90Degree = false;
        private const float SLERP_TIME = 30.0f;

        private void Awake()
        {
            gameObject.name = m_data.name;
            enabled = false;
        }

        private void Update()
        {
            Slerping();
        }

        public void StartSlerpAndSnap(Box1 box, Vector3 localPosition, Transform cartTransform, bool turn90Degree)
        {
            m_turn90Degree = turn90Degree;
            m_playerTransform = cartTransform;
            m_isMoving = true;
            m_targetLocalPosition = localPosition;
            m_box = box;
            enabled = true;
        }

        private void SnapToBox()
        {
            transform.localPosition = m_targetLocalPosition;
            transform.rotation = m_playerTransform.rotation;

            if (m_turn90Degree)
                transform.eulerAngles += new Vector3(0, 90, 0);

            EndSlerp();
        }

        private void EndSlerp()
        {
            //Debug.Log("Slerp Ended");
            enabled = false;
        }

        private void Slerping()
        {
            if (!m_isMoving)
                return;

            float slerpTime = m_timer / SLERP_TIME;
            m_timer += Time.deltaTime;

            Vector3 boxLocalPosition = m_box.transform.position + m_targetLocalPosition;

            transform.position = Vector3.Slerp(transform.position, boxLocalPosition, slerpTime);
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