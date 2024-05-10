using UnityEngine;

namespace BoxSystem
{
    public class Item : MonoBehaviour
    {

        public ItemData m_data;

        private Box m_box;
        private Transform m_playerTransform;
        private Transform m_boxTransform;
        private Vector3 m_targetLocalPosition;
        private Vector3 m_startPosition;

        private float m_timer = 0;
        private float m_distanceSnap;
        private float m_slerpTime;
        private bool m_isMoving = false;
        private bool m_turn90Degree = false;
        private Vector3 m_initialScale = Vector3.zero;
        private Vector3 m_targetedScale = Vector3.zero;

        private void Awake()
        {
            gameObject.name = m_data.name;         
        }

        private void Start()
        {
            m_targetedScale = BoxManager.GetInstance().GetLocalScale();
        }

        private void Update()
        {
            Slerping();
        }

        /// <summary> Pour configurer le slerp et le snap </summary>
        public void StartSlerpAndSnap(Box box, Vector3 localPosition, GameObject player, bool turn90Degree, float snapDistance, bool autoSnap)
        {
            m_turn90Degree = turn90Degree;
            m_playerTransform = player.transform;
            m_targetLocalPosition = localPosition;
            m_startPosition = transform.position;
            m_box = box;
            m_initialScale = transform.localScale;
            m_boxTransform = box.transform;
            m_slerpTime = player.GetComponentInChildren<GrabItemTrigger>().ItemSlerpTime;
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
            // set le parent comm étaant le graphique
            Transform allGraphics = m_boxTransform.GetChild(1);
            transform.SetParent(allGraphics);
           // transform.SetParent(m_boxTransform);

            transform.localPosition = m_targetLocalPosition;
            transform.localScale = m_targetedScale;  
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
            enabled = false;
        }

        /// <summary> Le slerp entre le point de départ de l'objet et sa place dans la boite </summary>
        private void Slerping()
        {
            if (!m_isMoving)
                return;

            float slerpTime = m_timer / m_slerpTime;
            m_timer += Time.deltaTime;

            if (m_box == null) 
            { 
                Destroy(this.gameObject);
            }

            Vector3 boxLocalPosition = m_box.transform.position + m_targetLocalPosition;

            transform.position = Vector3.Slerp(m_startPosition, boxLocalPosition, slerpTime);
            transform.localScale = Vector3.Slerp(m_initialScale, m_targetedScale, slerpTime);
            float targetDistance = Mathf.Abs(Vector3.Distance(transform.position, boxLocalPosition));

            if (targetDistance < m_distanceSnap)
            {
                m_isMoving = false;
                m_timer = 0;
                SnapToBox();
                return;
            }

        }

    }
}
