// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Sharing;

namespace SpectatorView
{
    public class SpectatorViewManager : SV_Singleton<SpectatorViewManager>
    {
        // Use IP to disambiguate scenarios where multiple Unity clients could be connected to the same sharing service.
        // This could happen if you have multiple Spectator View rigs in the same experience.
        [HideInInspector]
        [Tooltip("Set this to the IP of the machine running Unity you wish to connect this spectator view rig to.  You can leave this field blank if you are only using 1 spectator view rig.")]
        public string LocalComputerIP = string.Empty;

        [HideInInspector]
        [Tooltip("Comma separated IPs for other client HoloLens devices")]
        public string ClientHololensCSV = string.Empty;

        public GameObject GetAnchor
        {
            get
            {
                return SV_Settings.Instance.Anchor;
            }
        }

        protected override void Awake()
        {
            if (SV_Settings.Instance.SV_IP.Trim() == string.Empty)
            {
                Debug.LogWarning("If SV_Settings.SV_IP is not populated, it must be updated at runtime.");
            }

            if (SV_Settings.Instance.localIP.Trim() == string.Empty)
            {
                Debug.LogError("Sharing Service IP must be populated.");
            }

            if (SV_Settings.Instance.HolographicCameraManager == null)
            {
                Debug.LogError("HolographicCameraManager must be populated.");
            }

            if (SV_Settings.Instance.Anchor == null)
            {
                Debug.LogError("Anchor must be populated.");
            }

            if (SV_Settings.Instance.Sharing == null)
            {
                Debug.LogError("Sharing must be populated.");
            }

            InstantiateSharing();
        }

        public void InstantiateSharing()
        {
            SharingStage stage = SharingStage.Instance;

            if (stage == null)
            {
                GameObject sharingObj = GameObject.Find(SV_Settings.Instance.Sharing.name);

                if (sharingObj != null)
                {
                    stage = sharingObj.GetComponent<SharingStage>();
                }
            }

            // Instantiate Sharing.
            if (stage == null)
            {
                CreateSharingStage(null);
            }

            SV_Settings.Instance.Sharing.SetActive(true);
        }

        public void UpdateSpectatorViewIP()
        {
            SV_CustomMessages.Instance.SendUpdatedIPs(SV_Settings.Instance.SV_IP);

            if (SV_Settings.Instance.SV_IP != HolographicCameraManager.Instance.HolographicCameraIP)
            {
                HolographicCameraManager.Instance.HolographicCameraIP = SV_Settings.Instance.SV_IP;
            }

#if UNITY_EDITOR
            HolographicCameraManager.Instance.ResetHolographicCamera();
#endif
        }

        public void OnEnable()
        {
            EnableSpectatorView();
        }

        private void CreateSharingStage(Transform parent)
        {
            GameObject sharing = (GameObject)GameObject.Instantiate(SV_Settings.Instance.Sharing, Vector3.zero, Quaternion.identity);
            sharing.transform.parent = parent;

            SharingStage stage = sharing.GetComponent<SharingStage>();
            if (stage == null)
            {
                stage = sharing.AddComponent<SharingStage>();
            }

            stage.ConnectToServer(SV_Settings.Instance.localIP, stage.ServerPort);
        }

        private void CreateSpectatorViewRig(Transform parent)
        {
            HolographicCameraManager hcm = SV_Settings.Instance.HolographicCameraManager.GetComponent<HolographicCameraManager>();

            if (hcm == null)
            {
                hcm = SV_Settings.Instance.HolographicCameraManager.AddComponent<HolographicCameraManager>();
            }

            hcm.HolographicCameraIP = SV_Settings.Instance.SV_IP;
            hcm.LocalComputerIP = LocalComputerIP;

            SV_Settings.Instance.HolographicCameraManager = (GameObject)GameObject.Instantiate(SV_Settings.Instance.HolographicCameraManager, Vector3.zero, Quaternion.identity);
            SV_Settings.Instance.HolographicCameraManager.transform.parent = parent;
        }

        public void EnableSpectatorView()
        {
            // Instantiate Anchor.
            if (!SV_Settings.Instance.Anchor.activeInHierarchy)
            {
                SV_Settings.Instance.Anchor = (GameObject)GameObject.Instantiate(SV_Settings.Instance.Anchor, Vector3.zero, Quaternion.identity);
            }

            SV_Settings.Instance.Anchor.SetActive(true);

            // Instantiate HolographicCameraManager.
            if (SpectatorView.HolographicCameraManager.Instance == null)
            {
                CreateSpectatorViewRig(null);
            }
            else
            {
                HolographicCameraManager hcm = SV_Settings.Instance.HolographicCameraManager.GetComponent<HolographicCameraManager>();
                Transform parent = HolographicCameraManager.Instance.transform.parent;

                if (hcm == null)
                {
                    Debug.LogWarning("Recreating HolographicCameraManager prefab since HolographicCameraManager script did not exist on original.");

                    GameObject.DestroyImmediate(SpectatorView.HolographicCameraManager.Instance);
                    CreateSpectatorViewRig(parent);
                }
                else
                {
                    if (hcm.HolographicCameraIP != SV_Settings.Instance.SV_IP ||
                        hcm.LocalComputerIP != LocalComputerIP)
                    {
                        Debug.LogWarning("Recreating HolographicCameraManager prefab since IP's were incorrect on original.");

                        // IP's are wrong, recreate rig.
                        GameObject.DestroyImmediate(HolographicCameraManager.Instance);
                        CreateSpectatorViewRig(parent);
                    }
                }
            }

            SV_Settings.Instance.HolographicCameraManager.SetActive(true);
        }
    }
}
