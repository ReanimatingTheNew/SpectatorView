using SpectatorView;

/// <summary>
/// Monitors the status of the application
/// </summary>
public class AppStateManager : SV_Singleton<AppStateManager>
{
    #region Public Fields

    /// <summary>
    /// Enum to track progress through the experience.
    /// </summary>
    public enum AppState
    {
        Starting = 0,
        WaitingForAnchor,
        WaitingForStageTransform,
        Ready
    }

    /// <summary>
    /// Tracks the current state in the experience.
    /// </summary>
    public AppState CurrentAppState { get; set; }

    #endregion

    #region Main Methods

    void Start()
    {
        CurrentAppState = AppState.WaitingForAnchor;
    }

    void Update()
    {
        switch (CurrentAppState)
        {
            case AppState.WaitingForAnchor:

                if (SV_ImportExportAnchorManager.Instance
                    && SV_ImportExportAnchorManager.Instance.AnchorEstablished)
                {
                    CurrentAppState = AppState.WaitingForStageTransform;
                }
                break;
            case AppState.WaitingForStageTransform:

                if (SV_ModelController.Instance
                    && SV_ModelController.Instance.GotTransform)
                {
                    CurrentAppState = AppState.Ready;
                }
                break;
        }
    }

    #endregion
}