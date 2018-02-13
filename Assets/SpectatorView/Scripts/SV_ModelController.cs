using HoloToolkit.Sharing;

public class SV_ModelController : SpectatorView.SV_Singleton<SV_ModelController>
{
    #region Public Fields

    public string SV_SharingTag = "model";

    /// <summary>
    /// Tracks if we have been sent a transform for the model.
    /// The model is rendered relative to the actual anchor.
    /// </summary>
    public bool GotTransform { get; set; }

    #endregion

    #region Private Fields

    // flag if sharing callbacks is set (UserJoined)
    private bool registeredSharingStageCallbacks = false;

    #endregion

    #region Main Methods

    void Update()
    {
        if (!registeredSharingStageCallbacks // if callbacks is NOT registered 
            && SharingStage.Instance != null // SharingStage is exist
            && SharingStage.Instance.SessionUsersTracker != null) // SessionUsersTracker is exist
        {
            registeredSharingStageCallbacks = true; // set it to true, and we never back here

            SharingStage.Instance.SessionUsersTracker.UserJoined += SessionUsersTracker_UserJoined;
        }

        // share transform once
        if (!GotTransform)
        {
            GotTransform = true;

            ShareTransform();
        }
    }

    #endregion

    #region Utility Methods

    public void ShareTransform()
    {
#if NETFX_CORE
        SV_Sharing.Instance.SendTransform(transform.localPosition,
                transform.localRotation,
                transform.localScale,
                SV_SharingTag);
#endif
    }

    /// <summary>
    /// When a new user joins we want to send them the relative transform for the model if we have it.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SessionUsersTracker_UserJoined(User user)
    {
        ShareTransform();
    }

#endregion
}
