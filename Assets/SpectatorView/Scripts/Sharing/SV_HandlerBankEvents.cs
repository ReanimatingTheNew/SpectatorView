using Andy.IdGenerator;
using HoloToolkit.Sharing;
using SpectatorView.InputModule;
using SpectatorView.Other;
#if UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;
using SV_Data = SpectatorView.Sharing.SV_Sharing.SharingData;

namespace SpectatorView.Sharing
{
    public class SV_HandlerBankEvents : MonoBehaviour
    {
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

        #region Main Methods

        private void Start()
        {
            
        }

        #endregion

        #region Event Methods

        public void OnTransform(NetworkInMessage msg)
        {
            msg.ReadInt64(); // get user_id but not use

            string tag = SV_Sharing.Instance.ReadString(msg, true).stringValue; // get message_tag

            Target = null;

            switch (tag)
            {
#if UNITY_EDITOR
                case "root":
                    Target = SV_Root.Instance.Anchor;
                    break;
#endif
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
                case "object_transform":

                    var obj = JsonUtility.FromJson<SV_TransformSync.TransformData>(data.stringValue);
                    var _target = GetObjById.Instance.GetObject(obj.id);

                    if (_target != null)
                    {
                        Target = _target.transform;
                        ApplyTransform(obj);
                        Debug.Log("[GET Transform]: { id: " + obj.id + ", name: \"" + _target.name + "\" }");
                    }

                    break;

                case "recognize_keyword":
                    var obj1 = JsonUtility.FromJson<SV_KeywordsSync.Keyword>(data.stringValue);
                    var _target1 = GetObjById.Instance.GetObject(obj1.id);

                    if (_target1 != null)
                    {
                        var comp = _target1.GetComponent<SV_KeywordsSync>();
                        comp.InvokeKeywordResponse(obj1.keyword);
                    }

                    break;

                case "test":
                    //var obj = JsonUtility.FromJson<SomeClass>(data.stringValue);
                    break;
            }

            Debug.Log("[SV_HandlerBankEvents]: OnJson " + data.tag);
        }

        public void OnString(SV_Data data)
        {
            switch (data.tag)
            {
                case "terminate_app":
                    Application.Quit();
                    break;

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
                        Debug.Log("[GET ButtonClick]: { id: " + data.intValue + ", name: \"" + obj.name + "\" }");
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

                case "click_toggle":
                    var obj3 = GetObjById.Instance.GetObject(data.intValue); // try to get button with this id

                    if (obj3
                        && obj3.GetComponent<SV_Toggle>() != null)
                    {
                        obj3.GetComponent<SV_Toggle>().onClick.Invoke(); // invoke button event
                        Debug.Log("[GET ButtonClick]: { id: " + data.intValue + ", name: \"" + obj3.name + "\" }");
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
#if UNITY_EDITOR
                case "reset_global_transform":
                    SV_DragCube.Instance.ResetPosition();
                    break;

                case "start_rec":
                    SV_Record.Instance.StartRec();
                    break;

                case "stop_rec":
                    SV_Record.Instance.StopRec();
                    break;

                case "take_pic":
                    SV_Record.Instance.TakePic();
                    break;
#endif

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

        private void ApplyTransform(SV_TransformSync.TransformData data)
        {
            Target.position = data.position;
            Target.rotation = data.rotation;
            Target.localScale = data.localScale;
        }

        #endregion
    }
}
