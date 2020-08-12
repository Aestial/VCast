using UnityEngine;
using UnityEngine.UI;

namespace VCast.UI
{
    [RequireComponent(typeof(AspectRatioFitter))]
    public class FullscreenAspectRatio : MonoBehaviour
    {
        AspectRatioFitter aspectRatioFitter;
        void Awake()
        {
            aspectRatioFitter = GetComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectRatio = (float)Screen.width / Screen.height;
        }
    }
}