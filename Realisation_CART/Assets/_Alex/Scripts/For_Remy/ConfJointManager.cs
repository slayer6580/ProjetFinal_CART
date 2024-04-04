using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiscountDelirium
{
    public class ConfJointManager : MonoBehaviour
    {
        enum Eside
        {
            left,
            right
        }

        [SerializeField][Range(0.0f, 1000.0f)] private float m_springForce = 100.0f;
        [SerializeField] private GameObject m_firstBoxKinematic;
        [SerializeField] private Transform m_lastBoxTransform;
        [SerializeField] private List<GameObject> m_boxesWithConf = new List<GameObject>(); // SEULEMENT GAMEOBJECTS AVEC HINGE

        private List<ConfigurableJoint> m_confJoint = new List<ConfigurableJoint>();
        private List<Vector3> m_boxesInitialPosition = new List<Vector3>();

        private Eside m_side;
        private const float BOX_HALF_WIDTH = 0.5f;
        private const float BOX_HALF_HEIGHT = 0.5f;

        private void Awake()
        {

            foreach (GameObject go in m_boxesWithConf)
            {
                m_confJoint.Add(go.GetComponent<ConfigurableJoint>());
                //PAS BESOIN DE PRENDRE LIGNE EN BAS CAR TU AS DEJA CETTE DONN� UTILIS� DANS GetTopBox.ReplaceBoxToOrigin()
                m_boxesInitialPosition.Add(go.transform.localPosition);
            }

            // PEUT IMPORTE LE BORD
            m_side = Eside.right;
        }

        private void Start()
        {
            ChangeAllAnchors(m_side);
        }

        private void ReplaceAllBoxesToOrigin()
        {
            for (int i = 0; i < m_boxesWithConf.Count; i++)
            {
                m_boxesWithConf[i].transform.localPosition = m_boxesInitialPosition[i];
            }
        }

        public Quaternion getJointRotation(ConfigurableJoint joint)
        {
            return (Quaternion.FromToRotation(joint.axis, joint.connectedBody.transform.rotation.eulerAngles));
        }

        private void Update()
        {

            // DOIT REGARDER ANGLE DE LA BOITE AVEC LE PREMIER HINGE
            if (m_lastBoxTransform.localPosition.x > 0 && m_side == Eside.right)
            {
                ReplaceAllBoxesToOrigin(); // METTRE LE CODE DU TOWERBOXSYSTEM QUI FAIT PAREIL
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.left);
                Debug.Log("left anchor");

            }
            else if (m_lastBoxTransform.localPosition.x < 0 && m_side == Eside.left)
            {
                ReplaceAllBoxesToOrigin();
                EnabledColliderOnBoxes(true);
                ChangeAllAnchors(Eside.right);
                Debug.Log("right anchor");
            }


            UpdateSpringsForce();

        }

        private void LateUpdate()
        {

            foreach (GameObject box in m_boxesWithConf)
            {
                Vector3 lockedPosition = new Vector3(box.transform.localPosition.x, box.transform.localPosition.y, m_firstBoxKinematic.transform.localPosition.z);
                box.transform.localPosition = lockedPosition;
            }

        }

        private void EnabledColliderOnBoxes(bool value)
        {
            foreach (GameObject box in m_boxesWithConf)
            {
                box.GetComponent<BoxCollider>().enabled = value;
            }
        }

        // TEST ONLY
        private void UpdateSpringsForce()
        {
            for (int i = 0; i < m_confJoint.Count; i++)
            {
                JointDrive spring = m_confJoint[i].angularYZDrive;
                spring.positionSpring = m_springForce;
                m_confJoint[i].angularYZDrive = spring;
            }
        }

        private void ChangeAllAnchors(Eside wantedSide)
        {
            m_side = wantedSide;

            Vector3 anchorPosition = m_side == Eside.left ? new Vector3(-BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0) : new Vector3(BOX_HALF_WIDTH, -BOX_HALF_HEIGHT, 0);

            foreach (ConfigurableJoint hingeJoint in m_confJoint)
            {
                hingeJoint.anchor = anchorPosition;
            }

        }
    }
}
