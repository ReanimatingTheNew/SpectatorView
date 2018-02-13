using HoloToolkit.Sharing;
using System;
using UnityEngine;
using UnityEngine.Events;
using ID = SpectatorView.Sharing.SV_Sharing.TestMessageID;

namespace SpectatorView.Sharing
{
    /// <summary>
    /// Subsribes to SV_Sharing data events and call user provided events
    /// </summary>
    public class SV_HandlerBank : SV_Singleton<SV_HandlerBank>
    {
        #region Public Fields

        // frequently used
        public MsgEvent OnTransform;

        public DataEvent OnJson;

        public DataEvent OnString;
        public DataEvent OnInt;
        public DataEvent OnFloat;

        public DataEvent OnByte;
        public DataEvent OnLong;
        public DataEvent OnDouble;
        public DataEvent OnBool;

        [HideInInspector]
        public bool ModelLoaded = false;

        #endregion

        #region Main Methods

        // Setup callbacks for ALL Data Types
        private void Start()
        {
            var handlers = SV_Sharing.Instance.MessageHandlers;

            handlers[ID.Transform] = OnGetTransform;

            handlers[ID.Byte] = OnGetByte;
            handlers[ID.Int] = OnGetInt;
            handlers[ID.Long] = OnGetLong;
            handlers[ID.Float] = OnGetFloat;
            handlers[ID.Double] = OnGetDouble;
            handlers[ID.Bool] = OnGetBool;
            handlers[ID.String] = OnGetString;

            handlers[ID.Json] = OnGetJson;
        }

        #endregion

        #region Callbacks Methods

        // transform
        void OnGetTransform(NetworkInMessage msg)
        {
            OnTransform.Invoke(msg);
        }
        // byte
        void OnGetByte(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadByte(msg);

            OnByte.Invoke(obj);
        }
        // int
        void OnGetInt(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadInt(msg);

            OnInt.Invoke(obj);
        }
        // long
        void OnGetLong(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadLong(msg);

            OnLong.Invoke(obj);
        }
        // float
        void OnGetFloat(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadFloat(msg);

            OnFloat.Invoke(obj);
        }
        // double
        void OnGetDouble(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadDouble(msg);

            OnDouble.Invoke(obj);
        }
        // bool
        void OnGetBool(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadInt(msg);

            OnBool.Invoke(obj);
        }
        // string
        void OnGetString(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadString(msg);

            OnString.Invoke(obj);
        }
        // json
        void OnGetJson(NetworkInMessage msg)
        {
            var obj = SV_Sharing.Instance.ReadString(msg);

            OnJson.Invoke(obj);
        }

        #endregion

        #region Custom Events

        [Serializable]
        public class MsgEvent : UnityEvent<NetworkInMessage> { }

        [Serializable]
        public class DataEvent : UnityEvent<SV_Sharing.SharingData> { }

        #endregion
    }
}
