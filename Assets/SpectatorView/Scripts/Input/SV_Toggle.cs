using Andy.IdGenerator;
using HoloToolkit.Unity.InputModule;
using SpectatorView.Sharing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpectatorView.InputModule
{
    public class SV_Toggle : MonoBehaviour, IInputClickHandler
    {
        #region Public Fields

        public bool Enabled = false;

        public bool ShareClickId = true;

        public Color enabledColor;
        public Color disabledColor;

        public UnityEvent onClick;

        #endregion

        #region Private Fields

        private int _buttonId;
        private IDHolder _idHolder;

        private Image _image;

        #endregion

        #region Private Properties

        private Image Image
        {
            get
            {
                if (!_image)
                {
                    _image = GetComponent<Image>();
                }

                return _image;
            }
        }

        #endregion

        #region Main Methods

        private void Start()
        {
            _idHolder = GetComponent<IDHolder>();

            if (_idHolder != null)
            {
                _buttonId = _idHolder.ID;
            }

            if (Enabled)
            {
                ToggleColor();
                onClick.Invoke();
            }
        }

        #endregion

        #region Event Methods

        public void OnInputClicked(InputClickedEventData eventData)
        {
            ToggleColor();
            onClick.Invoke();

            if (_idHolder) { Debug.Log("SV_Toggle.onClick.Invoke() { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
            else { Debug.Log("SV_Toggle.onClick.Invoke() { id: 0, name: \"" + gameObject.name + "\" }"); }

            if (ShareClickId)
            {
                SV_Sharing.Instance.SendInt(_buttonId, "click_toggle");
                Log();
            }
        }

        #endregion

        #region Utility Methods

        public void EnabledColor()
        {
            if (Image)
            {
                Image.color = enabledColor;
            }
        }

        public void DisabledColor()
        {
            if (Image)
            {
                Image.color = disabledColor;
            }
        }

        public void ToggleColor()
        {
            if (Image)
            {
                // find all SV_Toggle
                var arr = FindObjectsOfType<SV_Toggle>();
                // disable all toggles
                foreach (var item in arr)
                {
                    item.DisabledColor();
                }

                Image.color = Image.color == enabledColor
                                ? disabledColor
                                : enabledColor;
            }
        }

        private void Log()
        {
            if (_idHolder) { Debug.Log("[SEND ToggleClick] { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
            else { Debug.Log("[SEND ToggleClick] { id: 0, name: \"" + gameObject.name + "\" }"); }
        }

        #endregion
    }
}
