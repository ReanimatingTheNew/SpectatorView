using UnityEngine;
using HoloToolkit.Sharing;
using System.Collections.Generic;
using SpectatorView.Sharing;

namespace SpectatorView
{
    /// <summary>
    /// Adds and updates the head transforms of remote users.
    /// Head transforms are sent and received in the local coordinate space of the GameObject
    /// this component is on.
    /// </summary>
    public class RemotePlayerManager : SV_Singleton<RemotePlayerManager>
    {
        #region Private Fields

        // local link to custom messages
        private SV_Sharing svSharing;

        /// <summary>
        /// Keep a list of the remote heads
        /// </summary>
        private Dictionary<long, RemoteHeadInfo> remoteHeads = new Dictionary<long, RemoteHeadInfo>();

        // flag if callbacks is set (UserJoined, UserLeft)
        private bool registeredSharingStageCallbacks = false;

        #endregion

        #region Public Properties

        public IEnumerable<RemoteHeadInfo> remoteHeadInfos
        {
            get
            {
                return remoteHeads.Values;
            }
        }

        #endregion

        #region Main Methods

        void Start()
        {
            // get link to custom messages
            svSharing = SV_Sharing.Instance;

            // setup events

            // update head transform event
            svSharing.MessageHandlers[SV_Sharing.TestMessageID.HeadTransform] = UpdateHeadTransform;
            //customMessages.MessageHandlers[CustomMessages.TestMessageID.UserAvatar] = UpdateUserAvatar;
        }

        // when instances of SharingStage and SessionUsersTracker is ready, we set callbacks and set flag
        void Update()
        {
            // if sharing instances not null and events not set, we do it
            if (!registeredSharingStageCallbacks
                && SharingStage.Instance != null
                && SharingStage.Instance.SessionUsersTracker != null)
            {
                registeredSharingStageCallbacks = true;

                SharingStage.Instance.SessionUsersTracker.UserJoined += SessionUsersTracker_UserJoined;
                SharingStage.Instance.SessionUsersTracker.UserLeft += SessionUsersTracker_UserLeft;
            }
        }

        #endregion

        #region Events

        // on destroy event remove callbacks and unset flag
        protected override void OnDestroy()
        {
            registeredSharingStageCallbacks = false;

            if (SharingStage.Instance != null)
            {
                // unset events
                SharingStage.Instance.SessionUsersTracker.UserJoined -= SessionUsersTracker_UserJoined;
                SharingStage.Instance.SessionUsersTracker.UserLeft -= SessionUsersTracker_UserLeft;
            }

            base.OnDestroy();
        }

        /// <summary>
        /// Called when a user is joining.
        /// </summary>
        private void SessionUsersTracker_UserJoined(User user)
        {
            // get user info and add it to remoteHeads list
            GetRemoteHeadInfo(user.GetID());

            UpdateRemoteHeadsCount();
        }

        /// <summary>
        /// Called when a new user is leaving.
        /// </summary>
        private void SessionUsersTracker_UserLeft(User user)
        {
            // if remoteHeads still contains this user
            if (remoteHeads.ContainsKey(user.GetID()))
            {
                // destroy user object in scene
                RemoveRemoteHead(remoteHeads[user.GetID()].HeadObject);
                // remove user from list of remote heads
                remoteHeads.Remove(user.GetID());

                UpdateRemoteHeadsCount();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the data structure for the remote users' head position.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public RemoteHeadInfo GetRemoteHeadInfo(long userID)
        {
            RemoteHeadInfo headInfo;

            // Get the head info if its already in the list, otherwise add it
            if (!remoteHeads.TryGetValue(userID, out headInfo))
            {
                headInfo = new RemoteHeadInfo();
                headInfo.UserID = userID;
                //LocalPlayerManager.Instance.SendUserAvatar();

                remoteHeads.Add(userID, headInfo);
            }

            return headInfo;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when a remote user sends a head transform.
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateHeadTransform(NetworkInMessage msg)
        {
            // Parse the message
            long userID = msg.ReadInt64();
            // read position
            Vector3 headPos = svSharing.ReadVector3(msg, true).vector3Value;
            // read rotation
            Quaternion headRot = svSharing.ReadQuaternion(msg, true).quaternionValue;
            // read rotation
            RemoteHeadInfo headInfo = GetRemoteHeadInfo(userID);

            // activates users head object and set it position and rotation
            if (headInfo.HeadObject != null)
            {
                // If we don't have our anchor established, don't draw the remote head.
                headInfo.HeadObject.SetActive(headInfo.Anchored);

                headInfo.HeadObject.transform.localPosition = headPos + headRot * headInfo.headObjectPositionOffset;

                headInfo.HeadObject.transform.localRotation = headRot;
            }

            headInfo.Anchored = (msg.ReadByte() > 0);
        }

        /// <summary>
        /// When a user has left the session this will cleanup their
        /// head data.
        /// </summary>
        /// <param name="remoteHeadObject"></param>
        private void RemoveRemoteHead(GameObject remoteHeadObject)
        {
            DestroyImmediate(remoteHeadObject);
        }

        private void UpdateRemoteHeadsCount()
        {
            SV_Settings.Instance.SV_UsersCount = remoteHeads.Count;
        }

        #endregion

        #region Nested Classes

        public class RemoteHeadInfo
        {
            public long UserID;
            public GameObject HeadObject;
            public Vector3 headObjectPositionOffset;
            public int PlayerAvatarIndex;
            public int HitCount;
            public bool Active;
            public bool Anchored;
        }

        #endregion
    }
}