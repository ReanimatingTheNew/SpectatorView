using Andy.IdGenerator;
using HoloToolkit.Unity.InputModule;
using SpectatorView.Sharing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpectatorView.InputModule
{
    // Универсальный скрипт. Можно использовать как кнопку с синхронизацией через SV_Sharing.
    // Cкрипт использует IDHolder содержащий уникальный ID который генерируется скриптом IdGenerator, 
    // для передачи этого значения через SV_Sharing
    public class SV_Button : MonoBehaviour,
                             IInputClickHandler
    {
        #region Public Fields

        public bool ShareClickId = true; // синхронизировать событие через SV_Sharing?
        public bool UseAsButton = false; // использовать функционал скрипта как кнопку
        public bool OnlyShare = false; // использовать как кнопку для шеринга

        public UnityEvent onClick; // это событие вызывается из SV_HandlerBankEvents при получении события клика

        #endregion

        #region Private Fields

        private int _buttonId;
        private IDHolder _idHolder;
        private Button _btn;
        private IInputClickHandler _inputClick;

        #endregion

        #region Main Methods

        private void Start()
        {
            _idHolder = GetComponent<IDHolder>();

            if (_idHolder != null)
            {
                _buttonId = _idHolder.ID;
            }

            if (!UseAsButton) // если скрипт НЕ используется как кнопка
            {
                onClick.RemoveAllListeners(); // убираем все события добавленные вручную

                _btn = GetComponent<Button>(); // проверяем наличие Button

                if (_btn != null)
                {
                    onClick.AddListener(ButtonClick); // вешаем на onClick SV_Button событие клика кнопки (для вызова из шеринга)
                }

                _inputClick = GetComponent<IInputClickHandler>(); // проверяем наличие скрипта IInputClickHandler

                if (_inputClick != null
                    && _inputClick != this) // если скрипт не является SV_Button
                {
                    onClick.AddListener(InputClick); // вешаем на onClick SV_Button событие клика кнопки (для вызова из шеринга)
                }
            }
        }

        #endregion

        #region Event Methods

        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (UseAsButton) // если скрипт используется как кнопка
            {
                ShareClick(); // шерит клик

                if (!OnlyShare)
                {
                    onClick.Invoke(); // вызывает onClick

                    // log onclick invoke
                    if (_idHolder) { Debug.Log("SV_Button.onClick.Invoke() { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
                    else { Debug.Log("SV_Button.onClick.Invoke() { id: 0, name: \"" + gameObject.name + "\" }"); }
                }
            }
            // если не используется то только шеринг
            else
            {
                ShareClick(); // шерит клик
            }
        }

        #endregion

        // метод для вызова onClick на кнопке
        public void ButtonClick()
        {
            _btn.onClick.Invoke();
            ShareClick();
        }

        // метод для вызова onClick на IInputClickHandler
        public void InputClick()
        {
            _inputClick.OnInputClicked(null);
            ShareClick();
        }

        public void ShareClick()
        {
            if (ShareClickId)
            {
                SV_Sharing.Instance.SendInt(_buttonId, "click_button"); // отсылаем id кнопки в SV_Sharing

                LogClick();
            }
        }

        private void LogClick()
        {
            if (_idHolder) { Debug.Log("[SEND ButtonClick] { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
            else { Debug.Log("[SEND ButtonClick] { id: 0, name: \"" + gameObject.name + "\" }"); }
        }
    }
}