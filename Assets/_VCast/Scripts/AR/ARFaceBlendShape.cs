using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace VCast.XR.ARFoundation
{
    [System.Serializable]
    public class FaceBlendShapeCoefficients
    {
        public Dictionary<int, float> blendShapeCoefficients;
        public FaceBlendShapeCoefficients()
        {
            blendShapeCoefficients = new Dictionary<int, float>();
        }
        public void Clear()
        {
            blendShapeCoefficients.Clear();
        }
        public void Add(int index, float coefficient)
        {
            try
            {
                blendShapeCoefficients.Add(index, coefficient);    
            }
            catch (System.Exception)
            {                
                throw;
            }            
        }
    }

    [System.Serializable]
    public class BlendShapeEvent : UnityEvent<FaceBlendShapeCoefficients>
    {
    }

    [RequireComponent(typeof(ARFace))]
    public class ARFaceBlendShape : MonoBehaviour
    {
        [SerializeField]
        string m_StrPrefix = "Face.M_F00_000_00_Fcl_";
        public string strPrefix
        {
            get { return m_StrPrefix; }
            set { m_StrPrefix = value; }
        }

        [SerializeField]
        float m_CoefficientScale = 100.0f;
        public float coefficientScale
        {
            get { return m_CoefficientScale; }
            set { m_CoefficientScale = value; }
        }

        [SerializeField]
        SkinnedMeshRenderer m_SkinnedMeshRenderer;
        public SkinnedMeshRenderer skinnedMeshRenderer
        {
            get
            {
                return m_SkinnedMeshRenderer;
            }
            set
            {
                m_SkinnedMeshRenderer = value;
                CreateFeatureBlendMapping();
            }
        }
        
        public BlendShapeEvent onUpdatedEvent;
        
    #if UNITY_IOS && !UNITY_EDITOR
        ARKitFaceSubsystem m_ARKitFaceSubsystem;
        Dictionary<ARKitBlendShapeLocation, int> m_FaceArkitBlendShapeIndexMap;
    #endif
        ARFace m_Face;
        FaceBlendShapeCoefficients coefficients;

        void Awake()
        {
            coefficients = new FaceBlendShapeCoefficients();
            // update even if window isn't focused, otherwise we don't receive.
            Application.runInBackground = true;

            // use Debug.Log functions for Telepathy so we can see it in the console
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;

            m_Face = GetComponent<ARFace>();
            CreateFeatureBlendMapping();
        }

        void CreateFeatureBlendMapping()
        {
            if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }
        #if UNITY_IOS && !UNITY_EDITOR
            m_FaceArkitBlendShapeIndexMap = new Dictionary<ARKitBlendShapeLocation, int>();
            // Brows
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownLeft        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Angry");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowDownRight       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Angry");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowInnerUp         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Surprise");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpLeft     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Fun");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.BrowOuterUpRight    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Fun");
            // Cheeks
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintLeft     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "ALL_Fun");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.CheekSquintRight    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "ALL_Fun");
            // Eyes
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeBlinkLeft        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_L");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeBlinkRight       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_R");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookDownLeft     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_L");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeLookDownRight    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_R");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeSquintLeft       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Joy_L"); // CLAMP? EXCLUSIVE?  
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeSquintRight      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Joy_R"); // CLAMP? EXCLUSIVE?  
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeWideLeft         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Surprise"); // EXCLUSIVE?
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.EyeWideRight        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Surprise"); // EXCLUSIVE?
            // Jaw
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawOpen             ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_A"); // CLAMP? EXCLUSIVE?  
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.JawForward          ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_O");
            // Mouth            
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthClose          ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Neutral"); // EXCLUSIVE?
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthDimpleLeft     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthDimpleRight    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFrownLeft      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Angry");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFrownRight     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Angry");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthFunnel         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Sorrow");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthLowerDownLeft  ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "HA_Fung2_Low");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthLowerDownRight ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPressLeft      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Up");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPressRight     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Up");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthPucker         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_U");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthRollLower      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Down");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthRollUpper      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Down");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthSmileLeft      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Fun");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthSmileRight     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Fun");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthStretchLeft    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_I");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthStretchRight   ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_I");
            m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthUpperUpLeft    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "HA_Fung2_Up");
            // m_FaceArkitBlendShapeIndexMap[ARKitBlendShapeLocation.MouthUpperUpRight   ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            
        #endif
        }

        void SetVisible(bool visible)
        {
            if (skinnedMeshRenderer == null) return;
            skinnedMeshRenderer.enabled = visible;
        }

        void UpdateVisibility()
        {
            var visible =
                enabled &&
                (m_Face.trackingState == TrackingState.Tracking) &&
                (ARSession.state > ARSessionState.Ready);
                
            SetVisible(visible);
        }

        void OnEnable()
        {
        #if UNITY_IOS && !UNITY_EDITOR
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null)
            {
                m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
            }
        #endif
            UpdateVisibility();
            m_Face.updated += OnUpdated;
            ARSession.stateChanged += OnSystemStateChanged;            
        }
        
        void OnDisable()
        {
            m_Face.updated -= OnUpdated;
            ARSession.stateChanged -= OnSystemStateChanged;
        }
                
        void OnSystemStateChanged(ARSessionStateChangedEventArgs eventArgs)
        {
            UpdateVisibility();
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {   
            UpdateVisibility();
            UpdateFaceFeatures();
            onUpdatedEvent.Invoke(coefficients);
        }

        void UpdateFaceFeatures()
        {
            if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }            
            coefficients.Clear();

        #if UNITY_IOS && !UNITY_EDITOR
            using (var blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
            {
                foreach (var featureCoefficient in blendShapes)
                {
                    int mappedBlendShapeIndex;
                    if (m_FaceArkitBlendShapeIndexMap.TryGetValue(featureCoefficient.blendShapeLocation, out mappedBlendShapeIndex))
                    {
                        if (mappedBlendShapeIndex >= 0)
                        {
                            skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, featureCoefficient.coefficient * coefficientScale);
                            coefficients.Add(mappedBlendShapeIndex, featureCoefficient.coefficient * coefficientScale);                            
                        }
                    }
                    else 
                    {
                        // Debug.Log("Cannot find coefficient with: " + featureCoefficient.blendShapeLocation.ToString());
                    }
                }
                // Debug.Log(coefficients.blendShapeCoefficients.Count);
            }
        #endif
        }

    }
}