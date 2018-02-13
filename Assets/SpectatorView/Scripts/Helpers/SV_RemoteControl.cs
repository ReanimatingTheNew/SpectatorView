using SpectatorView;
using UnityEngine;

namespace SpectatorView
{
    public class SV_RemoteControl : SV_Singleton<SV_RemoteControl>
    {
        #region Public Fields

        [Tooltip("SV camera sync")]
        public bool SV_CameraSync = true;

        [Tooltip("Postions offset")]
        public Vector3 SV_Camera_Position = new Vector3(0, -0.09f, 0);

        [Tooltip("Rotation offset")]
        public Vector3 SV_Camera_Rotation;

        #endregion
    }
}
