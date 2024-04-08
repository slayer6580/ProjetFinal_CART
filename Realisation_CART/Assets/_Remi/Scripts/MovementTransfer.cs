using Cinemachine;
using TMPro;
using UnityEngine;

namespace BoxSystem
{
    public class MovementTransfer : MonoBehaviour
    {
        [field: SerializeField] private Transform PlayerTransform { get; set; }
        [field: SerializeField] private CinemachineVirtualCamera _CinemachineVirtualCamera { get; set; }
        [field: SerializeField] private Transform TransformToMimic { get; set; }

        //[field: SerializeField] private Transform TransformToRemove { get; set; }
        private CinemachineTransposer m_cinemachineTransposer;
        private Vector3 m_cameraOffset = new Vector3(0, 0, 0);
        private Vector3 m_result = new Vector3(0, 0, 0);
        private Quaternion m_cameraRotOffset = new Quaternion(0, 0, 0, 0);

        private void Awake()
        {
            m_cinemachineTransposer = _CinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Update()
        {
            //if (TransformToMimic == null) return;

            //Vector3 camPosOffset = TransformToMimic.position - PlayerTransform.position;

            ////m_cameraOffset.x = camPosOffset.x * 0.05f;
            //m_cameraOffset.x = camPosOffset.x * 0.001f;
            //m_cameraOffset.y = transform.position.y;
            //m_cameraOffset.z = transform.position.z;

            ////Quaternion camRotOffset = TransformToMimic.rotation * Quaternion.Inverse(PlayerTransform.rotation);
            ////Quaternion camRotOffset = TransformToMimic.rotation * PlayerTransform.rotation;
            ////m_cameraRotOffset.w = camRotOffset.w;
            ////m_cameraRotOffset.x = transform.rotation.x;
            ////m_cameraRotOffset.y = transform.rotation.y;
            ////m_cameraRotOffset.z = transform.rotation.z;
            ////m_cameraRotOffset.y = camRotOffset.y * 0.5f;
            ////m_cameraRotOffset.z = camRotOffset.z * 0.5f;

            ////float positionDampingX = m_cinemachineTransposer.m_XDamping;
            ////float positionDampingY = m_cinemachineTransposer.m_YDamping;
            ////float positionDampingZ = m_cinemachineTransposer.m_ZDamping;

            ////float rotationDamping = m_cinemachineTransposer.m_YawDamping;

            ////m_result.x = Mathf.Lerp(transform.position.x, m_cameraOffset.x, Time.deltaTime * positionDampingX);
            ////m_result.y = Mathf.Lerp(transform.position.y, m_cameraOffset.y, Time.deltaTime * positionDampingY);
            ////m_result.z = Mathf.Lerp(transform.position.z, m_cameraOffset.z, Time.deltaTime * positionDampingZ);

            ////transform.position = m_result;
            //transform.position = m_cameraOffset;

            ////transform.rotation = Quaternion.Slerp(transform.rotation, m_cameraRotOffset, Time.deltaTime * rotationDamping);
            //transform.rotation = Quaternion.Inverse(TransformToMimic.rotation);
        }
    }
}
