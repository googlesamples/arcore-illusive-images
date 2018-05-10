//-----------------------------------------------------------------------
// <copyright file="TransmoObject.cs" company="Novel">
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

    /// <summary>
    /// Uses a line renderer to visualize a TrackedImage.
    /// </summary>
    public class TransmoObject : MonoBehaviour
    {
        /// <summary>
        /// The TrackedImage to visualize.
        /// </summary>
        public AugmentedImage Target;

        /// <summary>
        /// The user's position along an axis.
        /// </summary>
        public PerspectiveBlendLine Blend;

        /// <summary>
        /// First frame scaling.
        /// </summary>
        public FirstFrameScale IntroScale;
        
        /// <summary>
        /// Animation GameObject.
        /// </summary>
        public GameObject BlendGO;

        /// <summary>
        /// Animation on/off.
        /// </summary>
        public bool AnimationBool;
        
        private Animation m_Anim;
        private float m_LastT;
        private GameObject m_FirstPersonCam;

        private float m_HalfWidth;
        private float m_HalfHeight;

        /// <summary>
        /// Gets the top-left corner of the image in local space.
        /// </summary>
        public Vector3 LocalTopLeftCorner
        {
            get
            {
                return (m_HalfWidth * Vector3.left) + (m_HalfHeight * Vector3.forward);
            }
        }

        /// <summary>
        /// Gets the top-right corner of the image in local space.
        /// </summary>
        public Vector3 LocalTopRightCorner
        {
            get
            {
                return (m_HalfWidth * Vector3.right) + (m_HalfHeight * Vector3.forward);
            }
        }

        /// <summary>
        /// Gets the bottom-left corner of the image in local space.
        /// </summary>
        public Vector3 LocalBottomLeftCorner
        {
            get
            {
                return (m_HalfWidth * Vector3.left) + (m_HalfHeight * Vector3.back);
            }
        }

        /// <summary>
        /// Gets the bottom-right corner of the image in local space.
        /// </summary>
        public Vector3 LocalBottomRightCorner
        {
            get
            {
                return (m_HalfWidth * Vector3.right) + (m_HalfHeight * Vector3.back);
            }
        }

        /// <summary>
        /// The Unity Awake method.
        /// </summary>
        public void Awake()
        {
            if (AnimationBool == true) 
            {
                Blend = gameObject.GetComponent<PerspectiveBlendLine>();
                m_Anim = BlendGO.GetComponent<Animation>();
                m_Anim.Play("CINEMA_4D_Main");
                m_Anim["CINEMA_4D_Main"].speed = 0f;
            }

            m_FirstPersonCam = GameObject.Find("First Person Camera");
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            transform.position = Target.CenterPose.position;
            transform.rotation = Target.CenterPose.rotation;
            transform.localScale = new Vector3(Target.ExtentX, Target.ExtentX, Target.ExtentX);
            m_HalfWidth = Target.ExtentX / 2;
            m_HalfHeight = Target.ExtentZ / 2;
            if (IntroScale.LoopingBool == true) 
            {
                TweenAnimation(0.5f);
                m_LastT = 0.5f;
            }

            if (IntroScale.LoopingBool == false && AnimationBool == true) 
            {
                TweenAnimation(Blend.T);
            }
        }

        /// <summary>
        /// Checks if any corner of the target image is within a camera's frustrum.
        /// </summary>
        /// <param name="camera">The camera to check against.</param>
        /// <returns><c>true</c> if the atleast one corner is in the frustrum, otherwise <c>false</c>.</returns>
        public bool IsAnyCornerInFrustrum(Camera camera)
        {
            return _IsPointInFrustrum(transform.TransformPoint(LocalTopLeftCorner), camera) ||
                _IsPointInFrustrum(transform.TransformPoint(LocalTopRightCorner), camera) ||
                _IsPointInFrustrum(transform.TransformPoint(LocalBottomLeftCorner), camera) ||
                _IsPointInFrustrum(transform.TransformPoint(LocalBottomRightCorner), camera);
        }

        private bool _IsPointInFrustrum(Vector3 point, Camera camera)
        {
            var viewportPoint = camera.WorldToViewportPoint(point);
            return viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f &&
                viewportPoint.z > 0f;
        }

        /// <summary>
        /// Checks if any corner of the target image is within a camera's frustrum.
        /// </summary>
        /// <param name="tween">The look up frame.</param>
        private void TweenAnimation(float tween)
        {
            float smoothTime = 0.1F;
            float velocity = 0.0F;
            float smoothT = Mathf.SmoothDamp(m_LastT, tween, ref velocity, smoothTime);
            m_Anim["CINEMA_4D_Main"].normalizedTime = smoothT;
            m_LastT = smoothT;
        }
    }
}
