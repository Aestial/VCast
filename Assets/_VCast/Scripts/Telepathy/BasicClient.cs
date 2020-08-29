using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;

public class BasicClient : MonoBehaviour
{
    [SerializeField] string ip = "192.168.0.101";
    [SerializeField] int port = 1337;

    Client client = new Client();

    public void Connect()
    {
        client.Connect(ip, port);
    }

    public void Disconnect()
    {
        if (client.Connected)
            client.Disconnect();
    }

    public void SendMessage()
    {
        if (client.Connected)
            client.Send(new byte[]{0x2, 0x1, 0x37});
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
        client.Disconnect();
    }

    private void Update()
    {
        // Client
        if (client.Connected)
        {            
            // Show all new messages
            Telepathy.Message msg;
            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:
                        Debug.Log("Connected");
                        break;
                    case Telepathy.EventType.Data:
                        Debug.Log("Data: " + BitConverter.ToString(msg.data));
                        break;
                    case Telepathy.EventType.Disconnected:
                        Debug.Log("Disconnected");
                        break;
                }
            }
        }
    }
}
