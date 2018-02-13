using Andy.IdGenerator;
using SpectatorView.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpectatorView
{
    public class SV_TransformOneShot : MonoBehaviour
    {
        #region Public Fields

        public bool AutoStart = true;

        #endregion

        #region Private Fields

        private int _objectId;

        private IDHolder _idHolder;

        #endregion

        #region Main Methods

        void Start()
        {
            _idHolder = GetComponent<IDHolder>();

            if (_idHolder)
            {
                _objectId = _idHolder.ID;
            }

            if (AutoStart)
            {
                ShareTransform();
            }
        }

        #endregion

        #region Utility Methods

        public void ShareTransform()
        {
            SV_Sharing.Instance.SendJson(new SV_TransformSync.TransformData(_objectId,
                                            transform.position,
                                            transform.rotation,
                                            transform.localScale), "object_transform");

            Debug.Log("SV_TransformOneShot.ShareTransform()");
        }
    
        #endregion
    }
}