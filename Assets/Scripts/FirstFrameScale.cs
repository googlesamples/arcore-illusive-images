//-----------------------------------------------------------------------
// <copyright file="FirstFrameScale.cs" company="Novel">
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
/// Scale GameObject on first frame takeover.
/// </summary>
public class FirstFrameScale : MonoBehaviour 
{
    /// <summary>
    /// Looping State.
    /// </summary>
    public bool LoopingBool;

    /// <summary>
    /// Scaling Duration.
    /// </summary>
    public float ScalingDuration = 1.5f;

    /// <summary>
    /// The Unity Awake method.
    /// </summary>
    private void Awake() 
    {
        LoopingBool = false;
        StartCoroutine(ScaleY(ScalingDuration));
    }

    /// <summary>
    /// Scale GameObject in Y direction.
    /// </summary>
    /// <param name="duration">
    /// Duration of scaling transformation.
    /// </param>  
    /// <returns> 
    /// Returns null.
    /// </returns> 
    private IEnumerator ScaleY(float duration)
    {
        LoopingBool = true;
        yield return new WaitForSeconds(1.5f);
        float time = 0f;
        while (time < duration) 
        {
            time += Time.deltaTime;
            float lerpValue = Mathf.Lerp(0.001f, 1f, time / duration);
            this.transform.localScale = new Vector3(this.transform.localScale.x, lerpValue, this.transform.localScale.z);
            yield return null;
        }

        LoopingBool = false;
        yield break;
    }
}
