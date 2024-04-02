using UnityEngine;

namespace BoxSystem
{
    public class Item : MonoBehaviour
    {

        public ItemData m_data;
        private Box m_box;
        private Transform m_playerTransform;
        private Vector3 m_targetLocalPosition;
        [SerializeField] private Vector3 m_startPosition; // TEST SERIALIZEFIELD

        private float m_timer = 0;
        private float m_distanceSnap;
        private bool m_isMoving = false;
        private bool m_turn90Degree = false;
        private float m_slerpTime;

        private Transform m_boxTransform; // NEW

        private void Awake()
        {
            gameObject.name = m_data.name;
        }

        private void Start()
        {
            //GameObject instant = Instantiate(m_data.m_object);
            //instant.transform.SetParent(transform);
            //instant.transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            Slerping();
        }

        /// <summary> Pour configurer le slerp et le snap </summary>
        public void StartSlerpAndSnap(Box box, Vector3 localPosition, GameObject player, bool turn90Degree, float snapDistance, bool autoSnap)
        {
            Debug.Log("StartSlep");
            m_turn90Degree = turn90Degree;
            m_playerTransform = player.transform;
            m_targetLocalPosition = localPosition;
            m_startPosition = transform.position;
            m_box = box;
            m_boxTransform = box.transform;
            m_slerpTime = player.GetComponent<GrabItemTrigger>().ItemSlerpTime;
            if (autoSnap) // apres un reorganize
            {
                SnapToBox();
                return;
            }
            m_isMoving = true;
            m_distanceSnap = snapDistance;
            enabled = true;
        }

        /// <summary> Pour que l'objet Snap dans la boite avec la rotation du joueur et la rotation locale de la boite </summary>
        private void SnapToBox()
        {
            Debug.Log("Snaping!");
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

        /// <summary> Pour désactiver l'update de la classe </summary>
        private void EndSlerp()
        {
            Debug.Log("Slerp Ended");
            enabled = false;
        }

        /// <summary> Le slerp entre le point de départ de l'objet et sa place dans la boite </summary>
        private void Slerping()
        {
            Debug.Log("Sleping...");
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
