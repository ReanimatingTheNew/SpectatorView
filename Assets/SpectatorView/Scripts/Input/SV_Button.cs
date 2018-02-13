using Andy.IdGenerator;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Events;

namespace SpectatorView.InputModule
{
    public class SV_Button : MonoBehaviour, IInputClickHandler
    {
        #region Public Fields

        public bool ShareClickId = true;

        public UnityEvent onClick;

        #endregion

        #region Private Fields

        private int _buttonId;

        #endregion

        private void Start()
        {
            if (gameObject.GetComponent<IDHolder>() != null)
            {
                _buttonId = gameObject.GetComponent<IDHolder>().ID;
            }
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            onClick.Invoke();

            if (ShareClickId)
            {
                SV_Sharing.Instance.SendInt(_buttonId, "click_button");
            }
        }
    }
}