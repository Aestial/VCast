using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace VCast.XR.ARFoundation
{
    [RequireComponent(typeof(ARCameraBackground))]
    public class ARCameraTexture : MonoBehaviour
    {
        [SerializeField] RenderTexture renderTexture = default;
        [SerializeField] float textureScale;
        [SerializeField] int width = 280;
        ARCameraBackground m_ARCameraBackground;

        // Start is called before the first frame update
        void Awake() 
        {            
            m_ARCameraBackground = GetComponent<ARCameraBackground>();
            renderTexture.width = this.width;
            // renderTexture.width =  Mathf.CeilToInt(textureScale * Screen.width);
            // renderTexture.height =  Mathf.CeilToInt(textureScale * Screen.height);
        }

        // Update is called once per frame
        void Update()
        {
            // Copy the camera background to a RenderTexture
            Graphics.Blit(null, renderTexture, m_ARCameraBackground.material);            
            // // Copy the RenderTexture from GPU to CPU
            // var activeRenderTexture = RenderTexture.active;
            // RenderTexture.active = renderTexture;
            // if (m_LastCameraTexture == null)
            //     m_LastCameraTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, true);
            // m_LastCameraTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            // m_LastCameraTexture.Apply();
            // RenderTexture.active = activeRenderTexture;
            
            // // Write to file
            // var bytes = m_LastCameraTexture.EncodeToPNG();
            // var path = Application.persistentDataPath + "/camera_texture.png";
            // File.WriteAllBytes(path, bytes);
        }
    }
}
