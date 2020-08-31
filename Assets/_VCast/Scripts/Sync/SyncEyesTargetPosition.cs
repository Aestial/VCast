using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;

    public class SyncEyesTargetPosition : MonoBehaviour
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
        
        [SerializeField] string ip = "192.168.0.101";
        [SerializeField] int port = 1337;
        Client client;
    
        public void Connect()
        {
            client.Connect(ip, port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        void Awake()
        {
            // update even if window isn't focused, otherwise we don't receive.
            Application.runInBackground = true;
            client = new Client();
            // use Debug.Log functions for Telepathy so we can see it in the console
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;
        }
        
        void Update()
        {
            // client
            if (client.Connected)
            {
                // show all new messages
                Telepathy.Message msg;
                while (client.GetNextMessage(out msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            Debug.Log("Connected");
                            break;
                        case Telepathy.EventType.Data:
                            var data = msg.data;
                            var position = Utils.ObjectSerializationExtension.Deserialize<EyesTargetPosition>(data);
                            // Debug.Log("Coeffs: " + coeffs.ToString());
                            UpdateTargetPosition(position);
                            break;
                        case Telepathy.EventType.Disconnected:
                            Debug.Log("Disconnected");
                            break;
                    }
                }
            }
        }

        void OnApplicationQuit()
        {
            // the client/server threads won't receive the OnQuit info if we are
            // running them in the Editor. they would only quit when we press Play
            // again later. this is fine, but let's shut them down here for consistency
            client.Disconnect();            
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

