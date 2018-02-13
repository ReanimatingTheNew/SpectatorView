using UnityEngine;
using SpectatorView;

public class SV_Camera : SV_Singleton<SV_Camera>
{
    #region Public Fields

    public Vector3 position;

    public Quaternion rotation;

    public SV_RemotePlayerManager.RemoteHeadInfo info;

    #endregion

    #region Main Methods

    private void Start()
    {
        // make Main Camera child of Root
        Camera.main.transform.parent = SV_Root.Instance.Anchor;

        // if flag was not set
        if (!SV_Settings.Instance.SV_User_Connected)
        {
            SV_Settings.Instance.On_SV_UserConnected(info);
        }
    }

    private void OnDisable()
    {
        // if flag was not set
        if (SV_Settings.Instance.SV_User_Connected)
        {
            SV_Settings.Instance.On_SV_UserDisconnected();
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (SV_RemoteControl.Instance.SV_camera_sync
            && rotation != Quaternion.identity) // prevent bag with slow wifi
        {
            var cam = Camera.main;

            // sync Main camera position with SV_Camera
            cam.transform.position = position;
            cam.transform.rotation = rotation;

            // provide Main_Camera position offset
            cam.transform.position = cam.transform.position
                - SV_RemoteControl.Instance.SV_Camera_Position;

            // addtional rotation for main camera
            cam.transform.Rotate(SV_RemoteControl.Instance.SV_Camera_Rotation);
        }
#endif
    }

    #endregion

}
