using SpectatorView;
using UnityEngine;

public class SV_RemoteControl : SV_Singleton<SV_RemoteControl>
{
    #region Public Fields

    [Tooltip("SV camera sync")]
    public bool SV_camera_sync = true;

    [Tooltip("Postions offset")]
    public Vector3 SV_Camera_Position;

    [Tooltip("Rotation offset")]
    public Vector3 SV_Camera_Rotation;

    #endregion
}
