using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;

    public class SyncBlendShape : MonoBehaviour
    {
        [SerializeField]
        SkinnedMeshRenderer m_SkinnedMeshRenderer;
        public SkinnedMeshRenderer skinnedMeshRenderer
        {
            get { return m_SkinnedMeshRenderer; }
            set { m_SkinnedMeshRenderer = value; }
        }        
        [SerializeField] int port = 1337;

        Server server = new Server();

        public void StartServer()
        {
            server.Start(port);
        }

        public void StopServer()
        {
            server.Stop();
        }

        void Awake()
        {
            // update even if window isn't focused, otherwise we don't receive.
            Application.runInBackground = true;

            // use Debug.Log functions for Telepathy so we can see it in the console
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;
        }
        
        void Update()
        {
            // Server
            if (server.Active)
            {
                // show all new messages
                Telepathy.Message msg;
                while (server.GetNextMessage(out msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            Debug.Log("Connected");
                            break;
                        case Telepathy.EventType.Data:
                            var data = msg.data;
                            var coeffs = Utils.ObjectSerializationExtension.Deserialize<FaceBlendShapeCoefficients>(data);
                            Debug.Log(msg.connectionId + "as ID. Coeffs: " + coeffs.ToString());
                            UpdateFaceFeatures(coeffs);
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
            server.Stop();         
        }

        void UpdateFaceFeatures(FaceBlendShapeCoefficients face)
        {
            if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }            
            foreach (var c in face.blendShapeCoefficients)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(c.Key, c.Value);                
            }
        }
    }
}

