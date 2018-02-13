using UnityEngine;
using SpectatorView;

namespace Andy.IdGenerator
{
    public class IdGenerator : SV_Singleton<IdGenerator>
    {
        [HideInInspector]
        public int HoldersFound = 0;

        public void Generate()
        {
            // find all gameobjects in scene
            GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            int id = 1;
            int holderCount = 0;

            // loop thru all objects in scene
            foreach (var obj in allGameObjects)
            {
                var IdHolder = obj.GetComponent<IDHolder>();

                // if gameobject contains IDHolder
                if (IdHolder)
                {
                    IdHolder.ID = id;

                    id++;
                    holderCount++;
                }
            }

            HoldersFound = holderCount;

            Debug.Log("ID Generated for " + holderCount + " objects");
        }
    }
}
