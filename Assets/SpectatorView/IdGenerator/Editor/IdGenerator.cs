using UnityEngine;
using UnityEditor;

namespace Andy.IdGenerator
{
    public class IdGenerator
    {
        [MenuItem("Andy/IdGenerator")]
        public static void GenerateHashes()
        {
            IDHolder[] holders = Object.FindObjectsOfType<IDHolder>();

            for (int i = 0; i < holders.Length; i++)
            {
                Undo.RecordObject(holders[i], "blaBlaBla");
                holders[i].ID = i + 1;
                EditorUtility.SetDirty(holders[i]);
            }

            Debug.Log("ID Generated for " + holders.Length + " objects");
        }
    }
}
