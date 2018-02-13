// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections.Generic;

namespace SpectatorView
{
    public class NetworkMovement : MonoBehaviour
    {
        #region Nested Structs

        public struct NetworkTransform
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        #endregion

        #region Private Fields

        // is component attached to cam
        private bool attachToCam = false;

        private const int maxNetworkTransforms = 3;

        private Queue<NetworkTransform> networkTransforms = new Queue<NetworkTransform>(maxNetworkTransforms);

        #endregion

        #region Main Methods

        private void Start()
        {
            // if gameobject contains SV_Camera component
            if (GetComponent<SV_Camera>() != null)
            {
                attachToCam = true;
            }
        }

        void Update()
        {
            if (networkTransforms.Count > 0)
            {
                NetworkTransform networkTransform = networkTransforms.Dequeue();

                gameObject.transform.localPosition = networkTransform.Position;
                gameObject.transform.localRotation = networkTransform.Rotation;

                // if this component attached to SV_Camera gameobject
                if (attachToCam)
                {
                    // provide SV_camera offset
                    gameObject.transform.position = gameObject.transform.position
                        - SV_RemoteControl.Instance.SV_Camera_Position;
                }
            }
        }

        #endregion

        #region Utility Methods

        public void AddTransform(NetworkTransform networkTransform)
        {
            if (networkTransforms.Count >= maxNetworkTransforms)
            {
                networkTransforms.Dequeue();
            }

            networkTransforms.Enqueue(networkTransform);
        }

        #endregion
    }
}
