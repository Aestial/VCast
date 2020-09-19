﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;
    
    public class SendEyesTargetPosition : MonoBehaviour
    {
        [SerializeField] int connectionId = 0;
        [SerializeField] int port = 1339;
        
        [SerializeField] TMP_Text outputTMP = default;

        AREyesTarget eyesTarget;
        
        Server server = new Server();

        int updateCount = 0;
        
        public void StartServer()
        {
            server.Start(port);
            eyesTarget = FindObjectOfType<AREyesTarget>();
            eyesTarget.onUpdatedEvent.AddListener(SendData);
        }

        public void StopServer()
        {
            if (server.Active)
                server.Stop();
        }

        void Awake()
        {            
            Application.runInBackground = true;
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;
        }

        void OnApplicationQuit()
        {
            server.Stop();
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
                            connectionId = msg.connectionId;
                            Debug.Log(msg.connectionId + " Connected");
                            outputTMP.text = "Connected: " + msg.connectionId;
                            break;
                        case Telepathy.EventType.Data:
                            string output = " Data: " + BitConverter.ToString(msg.data);
                            Debug.Log(msg.connectionId + output);
                            outputTMP.text = output;                            
                            break;
                        case Telepathy.EventType.Disconnected:
                            Debug.Log(msg.connectionId + " Disconnected");
                            outputTMP.text = "Disconnected: " + msg.connectionId;
                            break;
                    }
                }
            }
        }

        void SendData(EyesTargetPosition position)
        {
            if (server.Active)
            {
                if (updateCount % 2 == 0)
                {
                    var data = Utils.ObjectSerializationExtension.SerializeToByteArray(position);
                    // Debug.Log("Data: " + BitConverter.ToString(data));
                    server.Send(connectionId, data);
                }
                updateCount++;
            }            
        }
    }
}