using UnityEngine;
using System.Collections;

public class OpenURL : MonoBehaviour
{
    [SerializeField] string url = default;
    
    public void Open()
    {
        Application.OpenURL(url);
    }
}