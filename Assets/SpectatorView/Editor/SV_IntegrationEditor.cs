using UnityEngine;
using UnityEditor;
using SpectatorView.Integration;

[CustomEditor(typeof(SV_Integration))]
public class SV_IntegrationEditor : Editor
{
    #region Main Methods

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Скрипт помогает быстро интегрировать Spectator View в проекты на HoloToolkit. "+
            "AddTooRoot: объекты которые нужно положить в HologramCollection (перемещаемые объекты). "+
            "Нажатия на кнопки синхронизируются автоматически.", MessageType.Info);

        if (GUILayout.Button("Integrate"))
        {
            SV_Integration.Instance.Integrate();
        }

        if (GUILayout.Button("Deintegrate"))
        {
            SV_Integration.Instance.Deintegration();
        }

        DrawDefaultInspector();
    }

    #endregion
}
