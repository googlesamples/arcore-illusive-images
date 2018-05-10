//-----------------------------------------------------------------------
// <copyright file="FirstFrameTransparency.cs" company="Novel">
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
/// Gradually turn on GameObject transparency on first frame takeover.
/// </summary>
public class FirstFrameTransparency : MonoBehaviour 
{
    /// <summary>
    /// Input alpha value.
    /// </summary>
    public float Alpha = 1.0f;
    private Renderer m_Renderer;

    /// <summary>
    /// The Unity Awake method.
    /// </summary>
    private void Awake() 
    {
        m_Renderer = this.gameObject.GetComponent<Renderer>();
        StartCoroutine(FadeOn(1.5f));
    }

    /// <summary>
    /// Fade GameObject on.
    /// </summary>
    /// <param name="duration">
    /// Duration of fading.
    /// </param>  
    /// <returns> 
    /// Returns null.
    /// </returns> 
    private IEnumerator FadeOn(float duration)
    {
        float time = 0f;
        while (time < duration) 
        {
            time += Time.deltaTime;
            float lerpValue = Mathf.Lerp(0f, Alpha, time / duration);
            m_Renderer.material.SetFloat("Opacity", lerpValue);
            yield return null;
        }

        yield break;
    }
}
