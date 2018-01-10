using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ScreenShoot))]
public class ScreenShootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScreenShoot myTarget = (ScreenShoot)target;

        DrawDefaultInspector();
        
        if (GUILayout.Button("Save Screen Shoot"))
        {
            myTarget.TakeScreenShoot();
        }
    }
}