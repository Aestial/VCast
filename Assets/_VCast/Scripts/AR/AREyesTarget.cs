using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace VCast.XR.ARFoundation
{
    [System.Serializable]
    public class Vector3Serializable 
    {
        public float x;
        public float y;
        public float z;
        public Vector3Serializable()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }
        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    [System.Serializable]
    public class EyesTargetPosition
    {
        public Vector3Serializable position;
        public Vector3Serializable eyeForward;
        public EyesTargetPosition()
        {
            position = new Vector3Serializable();
            eyeForward = new Vector3Serializable();
        }
    }

    [System.Serializable]
    public class EyesTargetEvent : UnityEvent<EyesTargetPosition>
    {
    }

    [RequireComponent(typeof(ARFace))]
    public class AREyesTarget : MonoBehaviour
    {
        public EyesTargetEvent onUpdatedEvent;

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
            EyesTargetPosition eyesTarget = new EyesTargetPosition();
            eyesTarget.eyeForward.Set(m_Face.rightEye.forward.x, m_Face.rightEye.forward.y, m_Face.rightEye.forward.z);
            // target.position = Eye.position + m_Face.rightEye.forward * targetDistance;
            target.position = Eye.position + m_Face.rightEye.forward * targetDistance;
            float x  = target.localPosition.x * (mirror ? -1.0f : 1.0f);
            float y  = -target.localPosition.y;
            float z  = target.localPosition.z * focalDistance;
            target.localPosition = new Vector3(x, y, z);
            eyesTarget.position.Set(target.position.x, target.position.y, target.position.z);
            onUpdatedEvent.Invoke(eyesTarget);
            // Debug.LogFormat("Eye target local position: {0}", target.localPosition);
            // Debug.LogFormat("Eye target position: {0}", target.position);            
        }
    }
}