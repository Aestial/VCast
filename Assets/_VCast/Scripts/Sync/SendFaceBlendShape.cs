using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;
    
    public class SendFaceBlendShape : MonoBehaviour
    {
        [SerializeField] string ip = "192.168.0.101";
        [SerializeField] int port = 1337;
        ARFaceBlendShape blendShape;
        Client client;

        public void Connect()
        {
            client.Connect(ip, port);
            blendShape = FindObjectOfType<ARFaceBlendShape>();
            blendShape.onUpdatedEvent.AddListener(SendData);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        void Awake()
        {
            client = new Client();
            Application.runInBackground = true;
        }

        void OnApplicationQuit()
        {
            client.Disconnect();
        }

        void SendData(FaceBlendShapeCoefficients coefficients)
        {
            if (client.Connected)
            {
                // Debug.Log("Coeffs: " + coefficients.ToString());
                Debug.Log("Coeffs lenght: " + coefficients.blendShapeCoefficients.Count.ToString());
                var data = Utils.ObjectSerializationExtension.SerializeToByteArray(coefficients);
                // Debug.Log("Data: " + BitConverter.ToString(data));
                client.Send(data);
            }            
        }

        void Update()
        {
            
        }
    }
}