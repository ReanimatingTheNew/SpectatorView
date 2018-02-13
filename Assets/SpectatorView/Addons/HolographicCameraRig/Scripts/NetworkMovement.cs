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

        // компонент присоединён к камере?
        private bool attachToCam = false;

        private const int maxNetworkTransforms = 3;

        private Queue<NetworkTransform> networkTransforms = new Queue<NetworkTransform>(maxNetworkTransforms);

        #endregion

        #region Main Methods

        private void Start()
        {
            // объект содержит компонент SV_Camera?
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

                // компонент прикреплён к камере и синхронизация SV_Camera включена
                if (attachToCam
                    && SV_RemoteControl.Instance.SV_CameraSync)
                {
                    // синхронизирует позицию головы и объекта
                    gameObject.transform.localPosition = networkTransform.Position;
                    gameObject.transform.localRotation = networkTransform.Rotation;

                    // добавляет оффсет позиции объекта
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
