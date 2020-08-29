using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Telepathy;

public class BasicServer : MonoBehaviour
{
    [SerializeField] int connectionId = 0;
    [SerializeField] int port = 1337;

    [SerializeField] TMP_Text outputTMP;

    Server server = new Server();

    public void StartServer()
    {
        server.Start(port);
    }

    public void Stop()
    {
        if (server.Active)
            server.Stop();
    }

    public void SendMessage()
    {
        if (server.Active)
            server.Send(connectionId, new byte[]{0x42, 0x13, 0x37});
    }
    
    private void Awake() 
    {
        Application.runInBackground = true;
        // use Debug.Log functions for Telepathy so we can see it in the console
        Telepathy.Logger.Log = Debug.Log;
        Telepathy.Logger.LogWarning = Debug.LogWarning;
        Telepathy.Logger.LogError = Debug.LogError;
    }

    private void OnApplicationQuit() 
    {
        server.Stop();
    }

    private void Update()
    {
        // Server
        if (server.Active)
        {            
            // Show all new messages
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
}
