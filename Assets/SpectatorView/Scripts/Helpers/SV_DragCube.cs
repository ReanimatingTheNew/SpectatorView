using System;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using SpectatorView.Sharing;
using UnityEngine.UI;

namespace SpectatorView
{
    public class SV_DragCube : Singleton<SV_DragCube>
    {
        #region Public Fields

        public bool IsEnabled = true;

        public enum Scope { Local, Global }
        public Scope currentScope;
        public GameObject Cube;
        public Text ModeText;
        public Transform DragCollection;

        #endregion

        #region Private Fields

        private Vector3 _startPosition;
        private Quaternion _startRotation;

        #endregion

        #region Main Methods

        private void Start()
        {
            _startPosition = DragCollection.position;
            _startRotation = DragCollection.rotation;

            if (IsEnabled)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        #endregion

        #region Utility Methods

        public void Show()
        {
            IsEnabled = true;
            Cube.SetActive(true);

            SV_Root.Instance.ChangeParent(DragCollection); // make all root objects childs of dragging collection
            SV_Root.Instance.DisableTransformListeners(); // disable all transform listeners to prevent sharing leaks
        }

        public void Hide()
        {
            IsEnabled = false;
            Cube.SetActive(false);

            SV_Root.Instance.ResetParent(); // make all root objects childs of root again
            SV_Root.Instance.EnableTransformListeners(); // enable all enabled listeners
        }

        public void SetScope(int newScope)
        {
            currentScope = (Scope)newScope;

            // if SV_TransformSync component exist destroy it
            var tSync = DragCollection.GetComponent<SV_TransformSync>();
            if (tSync) { DestroyImmediate(tSync); }

            switch (currentScope)
            {
                case Scope.Local:
                    ModeText.text = "Mode: Local";
                    break;

                case Scope.Global:
                    ModeText.text = "Mode: Global";

                    DragCollection.gameObject.AddComponent<SV_TransformSync>();
                    break;
            }
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            Hide();
        }

        public void ResetPosition()
        {
            SV_Root.Instance.ChangeParent(DragCollection);
            SV_Root.Instance.DisableTransformListeners();

            DragCollection.position = _startPosition;
            DragCollection.rotation = _startRotation;

            SV_Root.Instance.ResetParent(); // make all root objects childs of root again
            SV_Root.Instance.EnableTransformListeners(); // enable all enabled listeners
        }

        public void ResetGlobalPosition()
        {
            ResetPosition();
            SV_Sharing.Instance.SendBool(true, "reset_global_transform");
        }

        #endregion
    }
}
