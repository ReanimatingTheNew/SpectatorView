using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Andy.IdGenerator;
using UnityEngine.Events;
using SpectatorView.Sharing;

namespace SpectatorView.InputModule
{
    public class SV_Hover : MonoBehaviour,
                            IFocusable
    {
        #region Public Fields

        public bool ShareHoverId = true; // синхронизировать событие через SV_Sharing?

        public bool UseAsHover = false; // использовать функционал скрипта для действия при наведении?

        public UnityEvent onHover;
        public UnityEvent onBlur;

        #endregion

        #region Private Fields

        private int _buttonId;
        private IDHolder _idHolder;
        private IFocusable _focusable;

        #endregion

        #region Main Methods

        private void Start()
        {
            _idHolder = GetComponent<IDHolder>();

            if (_idHolder != null)
            {
                _buttonId = _idHolder.ID;
            }

            if (!UseAsHover)
            {
                onHover.RemoveAllListeners(); // убираем все события добавленные вручную
                onBlur.RemoveAllListeners(); // убираем все события добавленные вручную

                _focusable = GetComponent<IFocusable>();

                if (_focusable != null
                        && _focusable != this)
                {
                    onHover.AddListener(FocusOnObj);
                    onBlur.AddListener(BlurOnObj);
                }
            }
        }

        #endregion

        #region Event Methods

        public void OnFocusEnter()
        {
            if (UseAsHover)
            {
                onHover.Invoke();

                if (_idHolder) { Debug.Log("SV_Hover.onHover.Invoke() { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
                else { Debug.Log("SV_Hover.onHover.Invoke() { id: 0, name: \"" + gameObject.name + "\" }"); }
            }

            if (ShareHoverId)
            {
                SV_Sharing.Instance.SendInt(_buttonId, "hover_button");
                LogHover();
            }
        }

        public void OnFocusExit()
        {
            if (UseAsHover)
            {
                onBlur.Invoke();

                if (_idHolder) { Debug.Log("SV_Hover.onBlur.Invoke() { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
                else { Debug.Log("SV_Hover.onBlur.Invoke() { id: 0, name: \"" + gameObject.name + "\" }"); }
            }

            if (ShareHoverId)
            {
                SV_Sharing.Instance.SendInt(_buttonId, "blur_button");
                LogBlur();
            }
        }

        public void FocusOnObj()
        {
            _focusable.OnFocusEnter();
        }

        public void BlurOnObj()
        {
            _focusable.OnFocusExit();
        }

        #endregion

        #region Private Methods

        private void LogHover()
        {
            if (_idHolder) { Debug.Log("[SEND Hover] { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
            else { Debug.Log("[SEND Hover] { id: 0, name: \"" + gameObject.name + "\" }"); }
        }

        private void LogBlur()
        {
            if (_idHolder) { Debug.Log("[SEND Blur] { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
            else { Debug.Log("[SEND Blur] { id: 0, name: \"" + gameObject.name + "\" }"); }
        }

        #endregion
    }
}
