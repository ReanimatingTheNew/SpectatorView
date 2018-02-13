using HoloToolkit.Unity.InputModule;
using UnityEngine;
using Andy.IdGenerator;
using UnityEngine.Events;

public class SV_Hover : MonoBehaviour,
                        IFocusable
{
    #region Public Fields

    public bool ShareHoverId = true;

    public UnityEvent onHover;
    public UnityEvent onBlur;

    #endregion


    #region Private Fields

    private int _buttonId;

    #endregion

    #region Main Methods

    private void Start()
    {
        if (gameObject.GetComponent<IDHolder>() != null)
        {
            _buttonId = gameObject.GetComponent<IDHolder>().ID;
        }
    }

    #endregion

    #region Event Methods

    public void OnFocusEnter()
    {
        onHover.Invoke();

        if (ShareHoverId)
        {
            SV_Sharing.Instance.SendInt(_buttonId, "hover_button");
        }
    }

    public void OnFocusExit()
    {
        onBlur.Invoke();

        if (ShareHoverId)
        {
            SV_Sharing.Instance.SendInt(_buttonId, "blur_button");
        }
    }

    #endregion
}
