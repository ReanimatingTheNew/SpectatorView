using UnityEngine;
using SpectatorView;
using System.Collections.Generic;

namespace SpectatorView
{
    /// <summary>
    /// Component that allows add new gameobjects to root at start or at runtime, 
    /// and store all models in hologram collection in list
    /// </summary>
    public class SV_Root : SV_Singleton<SV_Root>
    {
        #region Public Fields

        [Tooltip("Add Objects here to add them dynamicly on scene load")]
        public List<GameObject> ObjectsInRoot; // after adding new objects to root, this array represent All objects in root

        public List<SV_TransformSync> TransformListeners = new List<SV_TransformSync>();

        #endregion

        #region Public Hidden Fields

        [HideInInspector]
        public Transform Anchor; // public link to this object transform

        #endregion

        #region Private Fields

        private List<SV_TransformListenerState> TransformListenersStates = new List<SV_TransformListenerState>();

        #endregion

        #region Main Methods

        private void OnEnable()
        {
            // set current transfrom to Anchor field
            Anchor = transform;
        }

        private void Start()
        {
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

            // loop thru all childs of root and add them to list
            foreach (Transform child in Anchor)
            {
                ObjectsInRoot.Add(child.gameObject);
            }

            // get all transform listeners
            var arr = FindObjectsOfType<SV_TransformSync>();

            if (arr != null
                    || arr.Length > 0)
            {
                TransformListeners.AddRange(arr);

                // TransformListenersStates
                for (var i = 0; i < arr.Length; i++)
                {
                    TransformListenersStates.Add(new SV_TransformListenerState(arr[i].IsEnabled, arr[i]));
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Add game object to root
        /// </summary>
        /// <param name="obj"></param>
        public void AddToRoot(GameObject obj)
        {
            GameObject model = null;

            if (!obj.activeSelf)
            {
                model = Instantiate(obj); // instaniate model in scene and savel link to it
                model.transform.SetParent(Anchor); // make model child of hologram collection
            }
            else
            {
                model = obj;
                model.transform.SetParent(Anchor); // make model child of hologram collection
            }

            // if list of objects not contains this model, add it
            if (!ObjectsInRoot.Contains(model))
            {
                ObjectsInRoot.Add(model);
            }
        }

        /// <summary>
        /// Add game object to root with position and rotation
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void AddToRoot(GameObject obj, Vector3 position, Quaternion rotation)
        {
            GameObject model = null;

            if (!obj.activeSelf)
            {
                model = Instantiate(obj, position, rotation); // instaniate model in scene and savel link to it
                model.transform.SetParent(Anchor); // make model child of hologram collection
            }
            else
            {
                model = obj;
                model.transform.SetParent(Anchor); // make model child of hologram collection
            }

            // if list of objects not contains this model, add it
            if (!ObjectsInRoot.Contains(model))
            {
                ObjectsInRoot.Add(model);
            }
        }

        public void ChangeParent(Transform parent)
        {
            foreach (var item in ObjectsInRoot)
            {
                item.transform.parent = parent;
            }
        }

        public void ResetParent()
        {
            foreach (var item in ObjectsInRoot)
            {
                item.transform.parent = Anchor;
            }
        }

        public void DisableTransformListeners()
        {
            foreach (var item in TransformListenersStates)
            {
                item.Listener.IsEnabled = false;
            }
        }

        public void EnableTransformListeners()
        {
            foreach (var item in TransformListenersStates)
            {
                if (item.IsEnable)
                {
                    item.Listener.IsEnabled = true;
                    item.Listener.IgnoreOnce = true;
                }
            }
        }

        #endregion

        #region Nested Classes

        public class SV_TransformListenerState
        {
            public bool IsEnable;
            public SV_TransformSync Listener;

            public SV_TransformListenerState(bool IsEnable, SV_TransformSync Listener)
            {
                this.IsEnable = IsEnable;
                this.Listener = Listener;
            }
        }

        #endregion
    }
}