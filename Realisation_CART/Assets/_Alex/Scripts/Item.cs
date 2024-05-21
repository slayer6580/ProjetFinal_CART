using UnityEngine;

namespace BoxSystem
{
    public class Item : MonoBehaviour
    {

        public ItemData m_data;

        private Transform m_towerTransform;
        private Vector3 m_startPosition;

        private float m_timer = 0;
        private float m_distanceSnap;
        private float m_slerpTime;
        private bool m_isMoving = false;
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
        public void StartSlerpTowardTower(GameObject tower, float snapDistance, float slerptime)
        {
            m_towerTransform = tower.transform;
            m_startPosition = transform.position;
            m_initialScale = transform.localScale;
            Transform model = transform.GetChild(0);
            model.transform.localScale = Vector3.one;

            m_slerpTime = slerptime;
            m_isMoving = true;
            m_distanceSnap = snapDistance;
            enabled = true;
        }

        /// <summary> Pour désactiver l'update de la classe </summary>
        private void EndSlerp()
        {
            //TODO demander a la tour de placer l'objet dans une boite
            m_towerTransform.gameObject.GetComponent<TowerBoxSystem>().AddItemToTower(this.gameObject);
            enabled = false;
        }

        /// <summary> Le slerp entre le point de départ de l'objet et sa place dans la boite </summary>
        private void Slerping()
        {
            if (!m_isMoving)
                return;

            float slerpTime = m_timer / m_slerpTime;
            m_timer += Time.deltaTime;

            transform.position = Vector3.Slerp(m_startPosition, m_towerTransform.position, slerpTime);
            transform.localScale = Vector3.Slerp(m_initialScale, m_targetedScale, slerpTime);
            float targetDistance = Mathf.Abs(Vector3.Distance(transform.position, m_towerTransform.position));

            if (targetDistance < m_distanceSnap)
            {
                m_isMoving = false;
                m_timer = 0;
                EndSlerp();
                return;
            }

        }

    }
}
