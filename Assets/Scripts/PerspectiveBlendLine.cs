//-----------------------------------------------------------------------
// <copyright file="PerspectiveBlendLine.cs" company="Novel">
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Find user's position along an axis between two defined points.
/// </summary>
public class PerspectiveBlendLine : MonoBehaviour 
{
    /// <summary>
    /// Two positions to blend between.
    /// </summary>
    public Transform BlendPosition1, BlendPosition2;

    /// <summary>
    /// Variable that is used to drive position in animation.
    /// </summary>
    public float T;

    /// <summary>
    /// Determine axis.
    /// </summary>
    public bool X, Z;

    /// <summary>
    /// The camera used for the first person view.
    /// </summary>
    private GameObject m_FirstPersonCam;
    private float m_CamAxisPos = 0f;
    private float m_BlendAxisPos1 = 0f;
    private float m_BlendAxisPos2 = 0f;

    /// <summary>
    /// The Unity Awake method.
    /// </summary>
    private void Awake() 
    {
        T = 0f;
        m_FirstPersonCam = GameObject.Find("First Person Camera");
    }

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    private void Update() 
    {
        // Get Camera in Local Space.
        Vector3 cameraRelative = transform.InverseTransformPoint(m_FirstPersonCam.transform.position);

        if (X == true) 
        {
            m_CamAxisPos = cameraRelative.x;
            m_BlendAxisPos1 = BlendPosition1.transform.localPosition.x;
            m_BlendAxisPos2 = BlendPosition2.transform.localPosition.x;
            if (m_CamAxisPos < m_BlendAxisPos1) 
            {
                T = 0f;
            } 
            else if (m_CamAxisPos > m_BlendAxisPos1 && m_CamAxisPos < m_BlendAxisPos2) 
            {
                T = (m_CamAxisPos - m_BlendAxisPos1) / (m_BlendAxisPos2 - m_BlendAxisPos1);
            }
            else 
            {
                T = 1f;
            }
        }
        else if (Z == true) 
        {
            m_CamAxisPos = cameraRelative.z;
            m_BlendAxisPos1 = BlendPosition1.transform.localPosition.z;
            m_BlendAxisPos2 = BlendPosition2.transform.localPosition.z;
            if (m_CamAxisPos < m_BlendAxisPos1) 
            {
                T = 0f;
            } 
            else if (m_CamAxisPos > m_BlendAxisPos1 && m_CamAxisPos < m_BlendAxisPos2) 
            {
                T = (m_CamAxisPos - m_BlendAxisPos1) / (m_BlendAxisPos2 - m_BlendAxisPos1);
            }  
            else 
            {
                T = 1f;
            }
        }
    }
}
