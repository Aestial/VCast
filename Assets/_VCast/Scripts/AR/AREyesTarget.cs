using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace VCast.XR.ARFoundation
{
    [RequireComponent(typeof(ARFace))]
    public class AREyesTarget : MonoBehaviour
    {
        [SerializeField] Transform Eye = default;
        [SerializeField] Transform target = default;
        [SerializeField] float focalDistance = 0.25f;
        [SerializeField] float targetDistance = 10.0f;
        [SerializeField] bool mirror = true;
        ARFace m_Face;
        XRFaceSubsystem m_FaceSubsystem;

        void Awake()
        {
            m_Face = GetComponent<ARFace>();
        }

        void OnEnable()
        {
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
            {
                m_FaceSubsystem = (XRFaceSubsystem)faceManager.subsystem;
                m_Face.updated += OnUpdated;
            }
            else
            {
                enabled = false;
            }
        }        
        void OnDisable()
        {
            m_Face.updated -= OnUpdated;
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {   
            target.position = Eye.position + m_Face.rightEye.forward * targetDistance;
            float x  = target.localPosition.x * (mirror ? -1.0f : 1.0f);
            float y  = -target.localPosition.y;
            float z  = target.localPosition.z * focalDistance;
            target.localPosition = new Vector3(x, y, z);
            // Debug.LogFormat("Eye target local position: {0}", target.localPosition);
            // Debug.LogFormat("Eye target position: {0}", target.position);            
        }
    }
}