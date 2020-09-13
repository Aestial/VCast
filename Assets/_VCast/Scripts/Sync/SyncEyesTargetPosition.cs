using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;

    public class SyncEyesTargetPosition : AbstractSync
    {
        [SerializeField] Transform m_Target;
        public Transform target
        {
            get { return m_Target; }
            set { m_Target = value; }
        }
        [SerializeField] float focalDistance = 0.25f;
        [SerializeField] float targetDistance = 10.0f;
        [SerializeField] bool mirror = true;
        [SerializeField] Transform Eye = default;
        
        public override void Awake()
        {
            base.Awake();
            onMessageReceived = OnMessageReceivedDelegate;
        }

        void OnMessageReceivedDelegate(Telepathy.Message msg)
        {
            var data = msg.data;
            var position = Utils.ObjectSerializationExtension.Deserialize<EyesTargetPosition>(data);
            UpdateTargetPosition(position);
        }

        void UpdateTargetPosition(EyesTargetPosition targetPosition)
        {
            if (target == null)
            {
                return;
            }
            // target.localPosition = new Vector3(targetPosition.position.x,targetPosition.position.y, targetPosition.position.z);
            Vector3 eyeForward = new Vector3(targetPosition.eyeForward.x, targetPosition.eyeForward.y, targetPosition.eyeForward.z);
            Debug.Log(eyeForward);
            // TODO: CHECK actual local coords for eye Forward vector. Do it absolute (substract face vector)
            // Vector3 eyePosition = new Vector3(0.0f, Eye.position.y, Eye.position.z);
            target.position = Eye.position + eyeForward * targetDistance;
            float x  = target.localPosition.x * (mirror ? -1.0f : 1.0f);
            float y  = -target.localPosition.y;
            float z  = target.localPosition.z * focalDistance;
            target.localPosition = new Vector3(x, y, z);
        }
    }
}

