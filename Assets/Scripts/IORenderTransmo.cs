//-----------------------------------------------------------------------
// <copyright file="IORenderTransmo.cs" company="Novel">
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
/// Render control for animated material.
/// </summary>
public class IORenderTransmo : MonoBehaviour 
{
    /// <summary>
    /// Instance of Render Transmo.
    /// </summary>
    public static IORenderTransmo Instance;

    /// <summary>
    /// Material to animate.
    /// </summary>
    public Material Mat;
    
    /// <summary>
    /// Texture folder name.
    /// </summary>
    public string BakeTextureBaseName = "TRANSMO_BAKE_004";
    
    /// <summary>
    /// Number of textures to cycle through.
    /// </summary>
    public int NumTextures = 10;

    private int m_Span = 200;
    private int m_LastTexIdx = -1;
    private string m_TexbaseColor = "{0}/{0}_{1}";
    private string m_TexbaseAlpha = "{0}/A_{0}_{1}";
    private PerspectiveBlendLine m_Blend;

    /// <summary>
    /// The Unity Awake method.
    /// </summary>
    private void Awake() 
    {
        Instance = this;
        m_Blend = this.gameObject.GetComponent<PerspectiveBlendLine>();
        StartCoroutine(FadeOn(3f));
    }

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    private void Update() 
    {
        int counter = (int)(m_Blend.T * m_Span * 10);
        float driver = 0.0f;
        int nIdx = (int)Mathf.Floor(counter / m_Span);
        if (m_LastTexIdx != nIdx) 
        {
            m_LastTexIdx = nIdx;
            string c0 = string.Format(m_TexbaseColor, BakeTextureBaseName, nIdx.ToString("D4"));
            string c1 = string.Format(m_TexbaseColor, BakeTextureBaseName, (nIdx + 1).ToString("D4"));
            string a0 = string.Format(m_TexbaseAlpha, BakeTextureBaseName, nIdx.ToString("D4"));
            string a1 = string.Format(m_TexbaseAlpha, BakeTextureBaseName, (nIdx + 1).ToString("D4"));
            Texture ct0 = Resources.Load(c0) as Texture;
            Texture ct1 = Resources.Load(c1) as Texture;
            Texture at0 = Resources.Load(a0) as Texture;
            Texture at1 = Resources.Load(a1) as Texture;
            Mat.SetTexture("_MainTex", ct0);
            Mat.SetTexture("_TransparentTex", at0);
            Mat.SetTexture("_SecondaryTex", ct1);
            Mat.SetTexture("_SecondaryTransparentTex", at1);
        }

        driver = Mathf.Pow((float)(counter % m_Span) / (float)m_Span, 2.0f);
        Mat.SetFloat("Interp", driver);
    }

    private IEnumerator FadeOn(float duration)
    {
        float time = 0f;
        while (time < duration) 
        {
            time += Time.deltaTime;
            float lerpValue = Mathf.Lerp(0f, 1f, time / duration);
            Mat.SetFloat("Opacity", lerpValue);
            yield return null;
        }

        yield break;
    }
}