using UnityEngine;
using SpectatorView.InputModule;

namespace SpectatorView.Other
{
    public class SV_MovePanel : MonoBehaviour
    {
        #region Public Fields

        public bool IsOpen = true;

        public GameObject image;

        #endregion

        #region Public Properties

        public bool IsEnabled
        {
            get
            {
                var hD = GetComponent<SV_HandDraggable>();

                if (hD != null) { return hD.IsEnabled; }
                else { return false; }
            }
            set
            {
                var hD = GetComponent<SV_HandDraggable>();

                if (hD) { hD.IsEnabled = value; }
            }
        }

        public bool ShareTransform
        {
            get
            {
                var hD = GetComponent<SV_HandDraggable>();

                if (hD != null) { return hD.ShareTransform; }
                else { return false; }
            }
            set
            {
                var hD = GetComponent<SV_HandDraggable>();

                if (hD != null) { hD.ShareTransform = value; }
            }
        }

        #endregion

        #region Main Methods

        private void Awake()
        {
            if (IsOpen) { Open(); }
            else { Close(); }
        }

        #endregion

        #region Utility Methods

        public void Open()
        {
            image.SetActive(true);
            IsOpen = true;
        }

        public void Close()
        {
            image.SetActive(false);
            IsOpen = false;
        }

        public void Toggle()
        {
            if (IsOpen) { Close(); }
            else { Open(); }
        }

        #endregion
    }
}
