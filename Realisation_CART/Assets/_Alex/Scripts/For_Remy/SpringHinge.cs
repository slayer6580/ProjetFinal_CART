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

        [SerializeField] private float m_angle = 0;
        [SerializeField] [Range(100.0f, 1000.0f)] private float m_springForce = 0;
        [SerializeField] private List<HingeJoint> m_hinges;

        private Eside m_side;
        private const float BOX_HALF_WIDTH = 0.5f; // a aller chercher apres test pour 'fitter' sur nos boites
        private const float BOX_HALF_HEIGHT = 0.5f; // a aller chercher apres test pour 'fitter' sur nos boites

        private void Awake()
        {
            if (m_angle >= 0)            
                m_side = Eside.left;            
            else
                m_side = Eside.right;
        }

        private void Start()
        {
            foreach (HingeJoint joint in m_hinges)
            {
                ChangeAllAnchors(m_side);
            }
        }

        private void Update()
        {
            // Change anchor side depending on angle desired
            if (m_angle > 0 && m_side == Eside.right)
            {
                // TODO un repositionnent des boites a l'origine juste avant
                ChangeAllAnchors(Eside.left);
            }
            else if (m_angle < 0 && m_side == Eside.left)
            {
                // TODO un repositionnent des boites a l'origine juste avant
                ChangeAllAnchors(Eside.right);
            }

            UpdateSprings();

        }

        private void UpdateSprings()
        {
            foreach (HingeJoint hingeJoint in m_hinges)
            {
                JointSpring spring = hingeJoint.spring;
                spring.targetPosition = m_angle; // angle désirée
                spring.spring = m_springForce; // la force du spring
                hingeJoint.spring = spring;
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;
            Vector3 anchorPosition = wantedSide == Eside.left ? new Vector3(-BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0) : new Vector3(0.5f, -0.5f, 0);

            foreach (HingeJoint hingeJoint in m_hinges)
            {
                hingeJoint.anchor = anchorPosition;
            }

        }


    }
}
