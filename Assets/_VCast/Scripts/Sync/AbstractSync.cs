using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Telepathy;

public abstract class AbstractSync : MonoBehaviour
{
    [SerializeField] string m_IP = "192.168.0.101";
    public string ip
    {
        get { return m_IP; }
        set { m_IP = value; }
    }
    [SerializeField] int m_Port = 1337;
    public int port
    {
        get { return m_Port; }
        set { m_Port = value; }
    }

    internal Client client;

    public delegate void OnMessageReceived(Telepathy.Message msg);
    internal OnMessageReceived onMessageReceived;

    public void Connect()
    {
        client.Connect(ip, port);
    }

    public void Disconnect()
    {
        client.Disconnect();
    }

    public virtual void Awake()
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
        // Client
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
                        Debug.Log("Message");
                        // CALL Delegate(msg); 
                        onMessageReceived(msg);
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
}
