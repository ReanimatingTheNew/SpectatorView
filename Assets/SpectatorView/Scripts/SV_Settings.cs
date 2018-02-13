using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using SpectatorView;

public class SV_Settings : SV_Singleton<SV_Settings>
{
    #region Public Fiels

    [Header("Required")]

    [Tooltip("Hololens Spectator View IP")]
    public string SV_IP;

    [Tooltip("Local Computer IP")]
    public string localIP;

    [Tooltip("Drag the HolographicCameraRig/Addon/Prefabs/HolographicCameraManager prefab here.  ")]
    public GameObject HolographicCameraManager;

    [Tooltip("Drag the application's anchor prefab here.  If one does not exist, drag the provided Anchor prefab here from the HolographicCameraRig/Addon/Prefabs directory.")]
    public GameObject Anchor;

    [Tooltip("Drag the HoloToolkit Sharing prefab here.")]
    public GameObject Sharing;

    [Header("Optional")]
    
    public bool EnableSpatialMapping = false;
    public bool DrawVisualMeshes = false;

    #endregion

    #region Public Hidden Fields

    [HideInInspector]
    public bool SV_User_Connected = false;

    // TODO: find a way detect SV_user on device
    [HideInInspector]
    public bool Is_SV_User = false;

    #endregion

    #region Main Methods

    void Start()
    {
        // enable or disable spatial mapping
        SpatialMappingActive(EnableSpatialMapping);
    }

    #endregion

    #region Event Methods

    // Called when SV_user connected
    public void On_SV_UserConnected(SV_RemotePlayerManager.RemoteHeadInfo headInfo)
    {
        Debug.Log("SV user connected");

        SV_User_Connected = true;

        #region Commented

        // TODO: set flag to SV user
        //if (headInfo.UserID == SV_CustomMessages.Instance.localUserID
        //    && headInfo.IP == SV_IP)
        //{
        //    Is_SV_User = true;

        //    TestScript.Instance.Play();
        //}

        #endregion
    }

    // Called when SV_user disconnected
    public void On_SV_UserDisconnected()
    {
        Debug.Log("SV user disconnected");

        SV_User_Connected = false;
    }

    #endregion

    #region Utility Methods

    // Enable or disable spatial mapping
    public void SpatialMappingActive(bool active)
    {
        if (SpatialMappingManager.Instance)
        {
            // draw visual meshes or not
            SpatialMappingManager.Instance.DrawVisualMeshes = DrawVisualMeshes;

            // if ShowSpatialMapping is true
            if (active)
            {
                // activate spatial mapping gameobject
                SpatialMappingManager.Instance.gameObject.SetActive(true);
                // start observer collisions with spatial mapping
                SpatialMappingManager.Instance.StartObserver();
            }
            else
            {
                // stop observer collisions with spatial mapping
                SpatialMappingManager.Instance.StopObserver();
            }
        }
    }

    #endregion
}
