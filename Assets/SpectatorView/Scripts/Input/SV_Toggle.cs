using Andy.IdGenerator;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    private Image _image;

    #endregion

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

    private void Start()
    {
        if (gameObject.GetComponent<IDHolder>() != null)
        {
            _buttonId = GetComponent<IDHolder>().ID;
        }

        if (Enabled)
        {
            ToggleColor();
            onClick.Invoke();
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        ToggleColor();
        onClick.Invoke();

        if (ShareClickId)
        {
            SV_Sharing.Instance.SendInt(_buttonId, "click_button");
        }
    }

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
}
