using UnityEngine;
using UnityEditor;

namespace Andy.IdGenerator
{
    [CustomEditor(typeof(IdGenerator))]
    public class IdGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Скрипт генерирует уникальные id для компонентов IDHolder.", MessageType.Info);

            DrawDefaultInspector();

            if (GUILayout.Button("Generate"))
            {
                IdGenerator.Instance.Generate();
            }
        }
    }
}