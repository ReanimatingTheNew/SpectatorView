using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SV_RemoteControl))]
public class SV_RemoteControlEdotor : Editor
{
    #region Main Methods

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Terminate App"))
        {
            SV_Sharing.Instance.SendValue(true, "terminate_app", true);
        }

        //if (GUILayout.Button("Test"))
        //{
        //    SV_Sharing.Instance.SendValue(true, "test_request", true);
        //}
    }

    #endregion
}