using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Telepathy;

namespace VCast
{
    using XR.ARFoundation;

    public class SyncFaceBlendShape : AbstractSync
    {
        [SerializeField] string m_StrPrefix = "Face.M_F00_000_00_Fcl_";
        public string strPrefix
        {
            get { return m_StrPrefix; }
            set { m_StrPrefix = value; }
        }
        [SerializeField] SkinnedMeshRenderer m_SkinnedMeshRenderer;
        public SkinnedMeshRenderer skinnedMeshRenderer
        {
            get { return m_SkinnedMeshRenderer; }
            set { m_SkinnedMeshRenderer = value; }
        }
        
        Dictionary<string, int> m_FaceBlendShapeIndexMap;

        public override void Awake()
        {
            base.Awake();
            onMessageReceived = OnMessageReceivedDelegate;
            CreateFeatureBlendMapping();
        }
        
        void CreateFeatureBlendMapping()
        {
            if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }
            m_FaceBlendShapeIndexMap = new Dictionary<string, int>();
            // Brows
            m_FaceBlendShapeIndexMap["BrowDownLeft"        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Angry");
            // m_FaceBlendShapeIndexMap["BrowDownRight"       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Angry");
            m_FaceBlendShapeIndexMap["BrowInnerUp"         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Surprise");
            m_FaceBlendShapeIndexMap["BrowOuterUpLeft"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Fun");
            // m_FaceBlendShapeIndexMap["BrowOuterUpRight"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "BRW_Fun");
            // Cheeks
            // m_FaceBlendShapeIndexMap["CheekSquintLeft"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "ALL_Fun");
            // m_FaceBlendShapeIndexMap["CheekSquintRight"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "ALL_Fun");
            // Eyes
            m_FaceBlendShapeIndexMap["EyeBlinkLeft"        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_L");
            m_FaceBlendShapeIndexMap["EyeBlinkRight"       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_R");
            // m_FaceBlendShapeIndexMap["EyeLookDownLeft"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_L");
            // m_FaceBlendShapeIndexMap["EyeLookDownRight"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Close_R");
            // m_FaceBlendShapeIndexMap["EyeSquintLeft"       ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Joy_L"); // CLAMP? EXCLUSIVE?  
            // m_FaceBlendShapeIndexMap["EyeSquintRight"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Joy_R"); // CLAMP? EXCLUSIVE?  
            // m_FaceBlendShapeIndexMap["EyeWideLeft"         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Surprise"); // EXCLUSIVE?
            // m_FaceBlendShapeIndexMap["EyeWideRight"        ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "EYE_Surprise"); // EXCLUSIVE?
            // Jaw
            m_FaceBlendShapeIndexMap["JawOpen"             ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_A"); // CLAMP? EXCLUSIVE?  
            // m_FaceBlendShapeIndexMap["JawForward"          ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_O");
            // Mouth            
            m_FaceBlendShapeIndexMap["MouthClose"          ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Neutral"); // EXCLUSIVE?
            // m_FaceBlendShapeIndexMap["MouthDimpleLeft"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            // m_FaceBlendShapeIndexMap["MouthDimpleRight"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            // m_FaceBlendShapeIndexMap["MouthFrownLeft"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Angry");
            // m_FaceBlendShapeIndexMap["MouthFrownRight"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Angry");
            m_FaceBlendShapeIndexMap["MouthFunnel"         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Sorrow");
            m_FaceBlendShapeIndexMap["MouthLowerDownLeft"  ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "HA_Fung2_Low");
            m_FaceBlendShapeIndexMap["MouthLowerDownRight" ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
            m_FaceBlendShapeIndexMap["MouthPressLeft"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Up");
            // m_FaceBlendShapeIndexMap["MouthPressRight"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Up");
            m_FaceBlendShapeIndexMap["MouthPucker"         ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_U");
            m_FaceBlendShapeIndexMap["MouthRollLower"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Down");
            // m_FaceBlendShapeIndexMap["MouthRollUpper"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Down");
            // m_FaceBlendShapeIndexMap["MouthSmileLeft"      ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Fun");
            // m_FaceBlendShapeIndexMap["MouthSmileRight"     ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_Fun");
            m_FaceBlendShapeIndexMap["MouthStretchLeft"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_I");
            // m_FaceBlendShapeIndexMap["MouthStretchRight"   ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_I");
            m_FaceBlendShapeIndexMap["MouthUpperUpLeft"    ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "HA_Fung2_Up");
            // m_FaceBlendShapeIndexMap["MouthUpperUpRight"   ]   = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(strPrefix + "MTH_E");
        }

        void OnMessageReceivedDelegate(Telepathy.Message msg)
        {
            var data = msg.data;
            var coeffs = Utils.ObjectSerializationExtension.Deserialize<FaceBlendShapeCoefficients>(data);
            UpdateFaceFeatures(coeffs);
        }

        void UpdateFaceFeatures(FaceBlendShapeCoefficients face)
        {
            if (skinnedMeshRenderer == null || !skinnedMeshRenderer.enabled || skinnedMeshRenderer.sharedMesh == null)
            {
                return;
            }            
            foreach (var featureCoefficient in face.blendShapeCoefficients)
            {
                int mappedBlendShapeIndex;
                if (m_FaceBlendShapeIndexMap.TryGetValue(featureCoefficient.Key, out mappedBlendShapeIndex))
                {
                    if (mappedBlendShapeIndex >= 0)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(mappedBlendShapeIndex, featureCoefficient.Value);                        
                    }
                }                
            }
        }
    }
}

