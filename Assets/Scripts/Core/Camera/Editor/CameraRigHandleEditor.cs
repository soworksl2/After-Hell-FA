using UnityEditor;
using UnityEngine;
using AfterHellFA.Camera;

[CustomEditor(typeof(RigCameraHandle))]
public class CameraRigHandleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Recalibrate camera"))
        {
            RigCameraHandle targetConverted = ((RigCameraHandle)this.target);
            Camera currentCamera = targetConverted.HandledCamera;

            if(currentCamera == null)
            {
                Debug.LogError("there isn't a camera to recalibrate");
                return;
            }

            currentCamera.transform.parent = targetConverted.transform;

            currentCamera.transform.localPosition = new Vector3(0, 0, targetConverted.CameraDistance * -1f);
            currentCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
            currentCamera.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}