using SpectatorView.Sharing;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SpectatorView.Other
{
    public class SV_Record : SV_Singleton<SV_Record>
    {
#if UNITY_EDITOR
        [DllImport("UnityCompositorInterface")]
        private static extern void TakePicture();

        [DllImport("UnityCompositorInterface")]
        private static extern void StartRecording();

        [DllImport("UnityCompositorInterface")]
        private static extern void StopRecording();

        [DllImport("UnityCompositorInterface")]
        private static extern bool IsRecording();
#endif

        #region Public Fields

        public GameObject RecText;
        public GameObject StopText;

        #endregion

        #region Public Methods

        // Start Recording
        public void StartRec()
        {
            ShowRec();

#if UNITY_EDITOR
            StartRecording();
#endif
            SV_Sharing.Instance.SendBool(true, "start_rec");
        }

        // Stop Recording
        public void StopRec()
        {
            ShowStop();

#if UNITY_EDITOR
            StopRecording();
#endif
            SV_Sharing.Instance.SendBool(true, "stop_rec");
        }

        // Take Recording
        public void TakePic()
        {
#if UNITY_EDITOR
            TakePicture();
#endif
            SV_Sharing.Instance.SendBool(true, "take_pic");
        }

        #endregion

        #region Private Methods

        private void ShowRec()
        {
            if (StopText != null) { StopText.SetActive(false); }
            if (RecText != null) { RecText.SetActive(true); }
        }

        private void ShowStop()
        {
            if (RecText != null) { RecText.SetActive(false); }
            if (StopText != null) { StopText.SetActive(true); }
        }

        #endregion

    }
}
