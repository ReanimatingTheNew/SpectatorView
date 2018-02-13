using Andy.IdGenerator;
using SpectatorView.Sharing;
using System;
using UnityEngine;

namespace SpectatorView
{
    public class SV_TransformSync : MonoBehaviour
    {
        #region Public Fields

        public bool IsEnabled = true;

        public bool IgnoreOnce = false;

        public bool Log = true;

        #endregion

        #region Private Fields

        private int _objectId;
        private IDHolder _idHolder;

        #endregion

        #region Main Methods

        private void Start()
        {
            _idHolder = GetComponent<IDHolder>();

            if (_idHolder != null)
            {
                _objectId = _idHolder.ID;
            }
        }

        void Update()
        {
            if (IgnoreOnce)
            {
                transform.hasChanged = false;
                IgnoreOnce = false;
            }

            if (IsEnabled
                && transform.hasChanged)
            {
                SV_Sharing.Instance.SendJson(new TransformData(_objectId,
                                            transform.position,
                                            transform.rotation,
                                            transform.localScale), "object_transform");

                transform.hasChanged = false;

                if (Log)
                {
                    if (_idHolder) { Debug.Log("[SEND Transfrorm] { id: " + _idHolder.ID + ", name: \"" + gameObject.name + "\" }"); }
                    else { Debug.Log("[SEND Transfrorm] { id: 0, name: \"" + gameObject.name + "\" }"); }
                }
            }
        }

        #endregion

        #region Nested Classes

        [Serializable]
        public class TransformData
        {
            public int id;

            public Vector3 position;
            public Quaternion rotation;
            public Vector3 localScale;

            public TransformData(int id, Vector3 position, Quaternion rotation, Vector3 localScale)
            {
                this.id = id;

                this.position = position;
                this.rotation = rotation;
                this.localScale = localScale;
            }
        }

        #endregion
    }
}
