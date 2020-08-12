using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class ScreenAspectRatio : MonoBehaviour
{
    AspectRatioFitter aspectRatioFitter;
    void Awake()
    {
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectRatio = (float)Screen.width / Screen.height;
    }
}