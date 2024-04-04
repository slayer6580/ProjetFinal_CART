using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class SpringHinge : MonoBehaviour
    {
        enum Eside
        {
            left,
            right
        }

        [SerializeField] private float m_angle = 0; // GARDER A 0
        [SerializeField][Range(0.0f, 1000.0f)] private float m_springForce = 100.0f;
        [SerializeField] private float m_angleOffset = 0.1f;
        [SerializeField] private GameObject m_firstBox;
        [SerializeField] private List<GameObject> m_boxesWithHinge = new List<GameObject>(); // SEULEMENT GAMEOBJECTS AVEC HINGE

        private List<HingeJoint> m_hinges = new List<HingeJoint>();
        private List<Vector3> m_boxesInitialPosition = new List<Vector3>();

        private Eside m_side;
        private const float BOX_HALF_WIDTH = 0.5f;
        private const float BOX_HALF_HEIGHT = 0.5f;

        private void Awake()
        {

            foreach (GameObject go in m_boxesWithHinge)
            {
                m_hinges.Add(go.GetComponent<HingeJoint>());
                //PAS BESOIN DE PRENDRE LIGNE EN BAS CAR TU AS DEJA CETTE DONN� UTILIS� DANS GetTopBox.ReplaceBoxToOrigin()
                m_boxesInitialPosition.Add(go.transform.localPosition);
            }

            // PEUT IMPORTE LE BORD
            m_side = Eside.left;
        }

        private void Start()
        {
            foreach (HingeJoint joint in m_hinges)
            {
                ChangeAllAnchors(m_side);
            }
        }

        private void ReplaceAllBoxesToOrigin()
        {
            for (int i = 0; i < m_boxesWithHinge.Count; i++)
            {
                m_boxesWithHinge[i].transform.localPosition = m_boxesInitialPosition[i];
            }
        }

        private void Update()
        {

            // DOIT REGARDER ANGLE DE LA BOITE AVEC LE PREMIER HINGE
            if (m_hinges[0].angle > m_angleOffset && m_side == Eside.right)
            {

                ReplaceAllBoxesToOrigin(); // METTRE LE CODE DU TOWERBOXSYSTEM QUI FAIT PAREIL
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.left);

            }
            else if (m_hinges[0].angle < -m_angleOffset && m_side == Eside.left)
            {

                ReplaceAllBoxesToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.right);
            }
            else if (m_hinges[0].angle > -m_angleOffset && m_hinges[0].angle < m_angleOffset)
            {

                EnabledColliderOnBoxes(false);
            }

            UpdateSpringsForce();

        }

        private void LateUpdate()
        {

            foreach (GameObject box in m_boxesWithHinge)
            {
                Vector3 lockedPosition = new Vector3(box.transform.localPosition.x, box.transform.localPosition.y, m_firstBox.transform.localPosition.z);
                box.transform.localPosition = lockedPosition;
            }

        }


        private void EnabledColliderOnBoxes(bool value)
        {
            foreach (GameObject box in m_boxesWithHinge)
            {
                box.GetComponent<BoxCollider>().enabled = value;
            }
        }

        private void UpdateSpringsForce()
        {
            for (int i = 0; i < m_hinges.Count; i++)
            {
                JointSpring spring = m_hinges[i].spring;
                spring.targetPosition = m_angle;
                spring.spring = m_springForce / (i + 1); // POUR DIMINUER LA FORCE DU SPRING DES BOITES AU DESSUS
                m_hinges[i].spring = spring;
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;
            // BOX_HALF_WIDTH = GetTopBox.GetAnchorWidth()
            // BOX_HALF_HEIGHT = GetTopBox.GetAnchorHeight()
            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0) : new Vector3(BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0);

            foreach (HingeJoint hingeJoint in m_hinges)
            {
                hingeJoint.anchor = anchorPosition;
            }

        }


    }
}