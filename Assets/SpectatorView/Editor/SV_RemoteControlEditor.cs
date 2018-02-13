using UnityEngine;
using UnityEditor;
using SpectatorView.Sharing;
using SpectatorView;

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
    }

    #endregion
}