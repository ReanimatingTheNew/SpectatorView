using Andy.IdGenerator;
using HoloToolkit.Sharing;
using SpectatorView.InputModule;
#if UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;
using SV_Data = SV_Sharing.SharingData;

public class SV_HandlerBankEvents : MonoBehaviour
{
#if UNITY_EDITOR
    [DllImport("UnityCompositorInterface")]
    private static extern void TakePicture();

    [DllImport("UnityCompositorInterface")]
    private static extern void StartRecording();

    [DllImport("UnityCompositorInterface")]
    private static extern void StopRecording();

    [DllImport("UnityCompositorInterface")]
    private static extern bool IsRecording();
#endif

    #region Public Fields

    public GameObject target;

    #endregion

    #region Private Fields

    private Transform _target;

    #endregion

    #region Public Properties

    public Transform Target
    {
        get
        {
            return _target;
        }
        private set
        {
            _target = value;
        }
    }

    #endregion

    #region Event Methods

    public void OnTransform(NetworkInMessage msg)
    {
        msg.ReadInt64(); // get user_id but not use

        string tag = SV_Sharing.Instance.ReadString(msg, true).stringValue; // get message_tag

        switch (tag)
        {
            case "target":
                Target = target.transform;
                break;
        }

        if (Target != null)
        {
            ApplyTransform(msg);
        }

        Debug.Log("[SV_HandlerBankEvents]: OnTransfrom " + tag);
    }

    public void OnJson(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                //var obj = JsonUtility.FromJson<SomeClass>(data.stringValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnJson " + tag);
    }

    public void OnString(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.stringValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnString " + data.tag);
    }

    public void OnInt(SV_Data data)
    {
        switch (data.tag)
        {
            case "click_button":
                var obj = GetObjById.Instance.GetObject(data.intValue); // try to get button with this id

                if (obj
                    && obj.GetComponent<SV_Button>() != null)
                {
                    obj.GetComponent<SV_Button>().onClick.Invoke(); // invoke button event
                }
                break;

            case "hover_button":
                var obj1 = GetObjById.Instance.GetObject(data.intValue); // try to get button with this id

                if (obj1
                    && obj1.GetComponent<SV_Hover>() != null)
                {
                    obj1.GetComponent<SV_Hover>().onHover.Invoke(); // invoke button event
                }
                break;

            case "blur_button":
                var obj2 = GetObjById.Instance.GetObject(data.intValue); // try to get button with this id

                if (obj2
                    && obj2.GetComponent<SV_Hover>() != null)
                {
                    obj2.GetComponent<SV_Hover>().onBlur.Invoke(); // invoke button event
                }
                break;

            case "test":
                Debug.Log("Value: " + data.intValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnInt " + data.tag);
    }

    public void OnFloat(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.floatValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnFloat " + data.tag);
    }

    public void OnByte(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.byteValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnByte " + data.tag);
    }

    public void OnLong(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.longValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnLong " + data.tag);
    }

    public void OnDouble(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.doubleValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnDouble " + data.tag);
    }

    public void OnBool(SV_Data data)
    {
        switch (data.tag)
        {
            case "test":
                Debug.Log("Value: " + data.boolValue);
                break;
        }

        Debug.Log("[SV_HandlerBankEvents]: OnBool " + data.tag);
    }

    #endregion

    #region Utility Methods

    private void ApplyTransform(NetworkInMessage msg)
    {
        Target.localPosition = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
        Target.localRotation = SV_Sharing.Instance.ReadQuaternion(msg, true).quaternionValue;
        Target.localScale = SV_Sharing.Instance.ReadVector3(msg, true).vector3Value;
    }

    #endregion
}
