using UnityEngine;
using UnityEngine.XR;
using VRM;

public  class FaceTracking: MonoBehaviour 
{
    // Setting item of Inspector 
    [Header( "Set VRM model" )] // Item title 
    public Transform Head; // Head part you want to move 
    public VRMBlendShapeProxy proxy; // Model body

    // private UnityARSessionNativeInterface m_session;

    // // Use this for initialization 
    // void Start () {
    //      // Get native session interface
    //     m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();

    //     // don't know if it needs 
    //     // Application.targetFrameRate = 60;
        
    //     ARKitFaceTrackingConfiguration config = new ARKitFaceTrackingConfiguration();
    //     config.alignment = UnityARAlignment.UnityARAlignmentGravity;
    //     // UnityARAlignmentenum 
    //     // WorldAlignment-Options regarding how 
    //     ARKit 
    //     builds the scene coordinate system based on the actual movement of the device // UnityARAlignmentGravity-The y axis of the coordinate system is parallel to gravity and its origin is the initial position of the device ← this // UnityARAlignmentGravityAndHeading-The y-axis of the coordinate system is parallel to gravity, the x- and z-axes point the compass, the origin is the initial position of the device 
    //     // UnityARAlignmentCamera-The scene coordinate system is locked to the camera orientation 
    //     config .enableLightEstimation = true ;

    //     // Only works 
    //     if supported if (config.IsSupported)
    //     {
    //         //AR session setting 
    //         //・ARKitWorldTrackingSessionConfiguration (6DOF) 
    //         //・ARKitSessionConfiguration (3DOF) 
    //         //Configure either of the above two, but ARKitFaceTrackingConfiguration may be OK
    //         m_session.RunWithConfig(config);

    //         // face detection
    //         UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdd;
    //         UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdate;
    //         UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
    //     }
    //     else {
    //          // Processing when unavailable
    //     }
    // }

    // // Callback face detection 
    // void FaceAdd(ARFaceAnchor anchorData) {
    //      // Update head and face
    //     UpdateHead(anchorData);
    //     UpdateFace(anchorData);
    // }

    // // Callback during face detection 
    // void FaceUpdate(ARFaceAnchor anchorData) {
    //      // Update head and face
    //     UpdateHead(anchorData);
    //     UpdateFace(anchorData);
    // }

    // // Callback Face detection
    //  missing void FaceRemoved(ARFaceAnchor anchorData) {
    //      // Do nothing
    // }

    // // Update head position 
    // void UpdateHead(ARFaceAnchor anchorData) {
    //      // Change ARKit's right-hand axis to Unity's left-hand axis and change horizontal pit to mirror 
    //     float angle = 0.0f ;
    //     Vector3 axis = Vector3.zero;
    //     var rot = UnityARMatrixOps.GetRotation(anchorData.transform);
    //     rot.ToAngleAxis( out angle, out axis);
    //     axis.x = -axis.x;
    //     axis.z = -axis.z;
    //     Head.localRotation = Quaternion.AngleAxis(angle, axis);
    // }

    // // Update facial expression 
    // void UpdateFace(ARFaceAnchor anchorData) {
    //     var blendShapes = anchorData.blendShapes;
    //     if (blendShapes == null || blendShapes.Count == 0 )
    //     {
    //         return ;
    //     }

    //     // Open mouth (close 0.0←→1.0 open)
    //     proxy.SetValue(BlendShapePreset.O, blendShapes[ARBlendShapeLocation.JawOpen]);
    //     //Open eyes (close 0.0←→1.0 open)
    //     proxy.SetValue(BlendShapePreset.Blink_L, blendShapes[ARBlendShapeLocation.EyeBlinkLeft]);
    //     proxy.SetValue(BlendShapePreset.Blink_R, blendShapes[ARBlendShapeLocation.EyeBlinkRight]);
    // }
}