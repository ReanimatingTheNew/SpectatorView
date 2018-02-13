using UnityEngine;
using SpectatorView;

/// <summary>
/// Transmits the current position of the user's head to the others of the users in Update function
/// </summary>
public class LocalPlayerManager : SV_Singleton<LocalPlayerManager>
{
    #region Main Methods

    // Send the user's head position each frame.
    void Update()
    {
        // if model placed
        if (SV_ImportExportAnchorManager.Instance
            && SV_ImportExportAnchorManager.Instance.AnchorEstablished)
        {
            // Grab the current head transform and broadcast it to all the other users in the session
            Transform headTransform = Camera.main.transform;

            // Transform the head position and rotation into local space
            Vector3 headPosition = transform.InverseTransformPoint(headTransform.position);
            Quaternion headRotation = Quaternion.Inverse(transform.rotation) * headTransform.rotation;

            if (SV_Sharing.Instance)
            {
                // send head transform
                SV_Sharing.Instance.SendHeadTransform(headPosition,
                    headRotation,
                    headTransform.localScale,
                    0x1);
            }
        }
    }

    #endregion
}