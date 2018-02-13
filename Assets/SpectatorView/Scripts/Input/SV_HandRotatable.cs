// Copyright (c) HoloGroup (http://holo.group). All rights reserved.

using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace SpectatorView.InputModule
{
    /// <summary>
    /// SV_HandRotatable is a modification of HoloTools HandRotatable script, 
    /// but it also sharing transform between all devices using SV_Sharing class
    /// </summary>
    public class SV_HandRotatable : MonoBehaviour,
        INavigationHandler
    {
        #region Public Fields

        public bool IsEnabled = true;

        public string SV_SharingTag = "";

        public bool ShareTransform = true;

        public float rotationSensitivity = 10.0f;

        [Tooltip("Transform that will be dragged. Defaults to the object of the component.")]
        public Transform HostTransform;

        #endregion

        #region Private Fields

        private float rotationFactor;

        #endregion

        #region Actions

        public Action OnStart;
        public Action OnUpdate;
        public Action OnComplete;
        public Action OnCancel;

        #endregion

        #region Main Methods

        private void Start()
        {
            if (!HostTransform)
            {
                HostTransform = transform;
            }
        }

        #endregion

        #region Events

        public void OnNavigationStarted(NavigationEventData eventData)
        {
            if (IsEnabled && OnStart != null)
            {
                OnStart();
            }
        }

        public void OnNavigationUpdated(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                // rotate model
                rotationFactor = eventData.CumulativeDelta.x * rotationSensitivity;
                HostTransform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
                // share transform
                SV_ShareTransform();

                if (OnUpdate != null)
                {
                    OnUpdate();
                }
            }
        }

        public void OnNavigationCompleted(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                SV_ShareTransform();

                if (OnComplete != null)
                {
                    OnComplete();
                }
            }
        }

        public void OnNavigationCanceled(NavigationEventData eventData)
        {
            if (IsEnabled)
            {
                SV_ShareTransform();

                if (OnCancel != null)
                {
                    OnCancel();
                }
            }
        }

        #endregion

        #region Utility Methods

        public void SV_ShareTransform()
        {
            if (ShareTransform)
            {
                SV_Sharing.Instance.SendTransform(HostTransform.localPosition,
                                                  HostTransform.localRotation,
                                                  HostTransform.localScale, SV_SharingTag);
            }
        }

#endregion
    }
}
