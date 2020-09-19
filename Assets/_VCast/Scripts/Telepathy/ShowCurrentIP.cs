using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using TMPro;

public class ShowCurrentIP : MonoBehaviour
{
    [SerializeField] TMP_Text text = default;

    public string GetLocalIPv4()
     {
         return Dns.GetHostEntry(Dns.GetHostName())
             .AddressList.First(
                 f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
             .ToString();
     }    

    void Start()
    {
        text.text = GetLocalIPv4();
    }
}
