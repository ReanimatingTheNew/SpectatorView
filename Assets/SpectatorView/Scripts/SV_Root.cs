using UnityEngine;
using SpectatorView;
using System.Collections.Generic;

public class SV_Root : SV_Singleton<SV_Root>
{
    #region Public Fields

    [Tooltip("Add Objects here to add them dynamicly on scene load")]
    public List<GameObject> ObjectsInRoot; // after adding new objects to root, this array represent All objects in root

    #endregion

    #region Public Hidden Fields

    [HideInInspector]
    public Transform Anchor; // public link to this object transform

    #endregion

    #region Main Methods 

    private void Start()
    {
        // set current transfrom to Anchor field
        Anchor = transform;

        // if we have any objects in list add them in root
        if (ObjectsInRoot.Count > 0)
        {
            foreach (var obj in ObjectsInRoot)
            {
                // add objects in root
                AddToRoot(obj);
            }
        }
        
        // after objects added in root, clear list
        ObjectsInRoot.Clear();

        // then get all childs in root and add them to list
        foreach (Transform child in Anchor)
        {
            ObjectsInRoot.Add(child.gameObject);
        }
    }

    #endregion

    #region Utility Methods

    public void AddToRoot(GameObject obj)
    {
        var model = Instantiate(obj);

        model.transform.SetParent(Anchor);
    }

    public void AddToRoot(GameObject obj, Vector3 position, Quaternion rotation)
    {
        var model = Instantiate(obj, position, rotation);

        model.transform.SetParent(Anchor);
    }

    #endregion
}
