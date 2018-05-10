//-----------------------------------------------------------------------
// <copyright file="TransmoController.cs" company="Novel">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for Transmo.
    /// </summary>
    public class TransmoController : MonoBehaviour
    {
        /// <summary>
        /// A prefab for transmogrification demo.
        /// </summary>
        public TransmoObject TransmoPrefab;

        /// <summary>
        /// The camera used for the first person view of the example.
        /// </summary>
        public Camera FirstPersonCamera;

        private List<AugmentedImage> m_ImagesInCameraFrustrum = new List<AugmentedImage>();

        private List<TransmoObject> m_Geo = new List<TransmoObject>();

        private List<AugmentedImage> m_TempTrackedImages = new List<AugmentedImage>();

        private bool m_InstantiateBool;

        /// <summary>
        /// The Unity Start method.
        /// </summary>
        public void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            m_InstantiateBool = false;
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Get all tracked images that have been detected this frame.
            Session.GetTrackables<AugmentedImage>(m_TempTrackedImages, TrackableQueryFilter.Updated);

            // Create objects for the new tracked images.
            foreach (var image in m_TempTrackedImages)
            {
                if (image.TrackingState == TrackingState.Tracking) 
                {
                    if (m_InstantiateBool == false) 
                    {
                        GameObject augmentedGO = Instantiate(TransmoPrefab.gameObject);
                        var geo = augmentedGO.GetComponent<TransmoObject>();
                        geo.Target = image;
                        m_Geo.Add(geo);
                        m_InstantiateBool = true;
                    }
                }
            }

            // Cull out any visualisers for tracked images that have stopped tracking.
            int removedCount = m_Geo.RemoveAll(x => x.Target.TrackingState == TrackingState.Stopped);
            if (removedCount > 0)
            {
                Debug.LogFormat("Culled {0} visualizer(s).", removedCount);
            }

            // Find images within the camera frustrum.
            bool changeToImagesInFrustrumSet = false;
            m_TempTrackedImages.Clear();
            foreach (var geo in m_Geo)
            {
                if (geo.Target.TrackingState == TrackingState.Tracking &&
                    geo.IsAnyCornerInFrustrum(FirstPersonCamera))
                {
                    m_TempTrackedImages.Add(geo.Target);

                    // Track if this image was added to the frustrum (avoid per-frame string allocation).
                    changeToImagesInFrustrumSet = changeToImagesInFrustrumSet ||
                        !m_ImagesInCameraFrustrum.Contains(geo.Target);
                }
            }

            // Track if an image was removed from the frustrum (avoid per-frame string allocation).
            foreach (var image in m_ImagesInCameraFrustrum)
            {
                changeToImagesInFrustrumSet = changeToImagesInFrustrumSet ||
                    !m_TempTrackedImages.Contains(image);
            }

            m_ImagesInCameraFrustrum.Clear();
            m_ImagesInCameraFrustrum.AddRange(m_TempTrackedImages);
        }
    }
}
