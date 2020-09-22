using UnityEngine;

public class NeverSleep : MonoBehaviour
{
    void Start()
    {        
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}