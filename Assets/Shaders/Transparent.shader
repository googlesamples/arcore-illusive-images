// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Transparent"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TransparentTex ("Transparent", 2D) = "white" {}

		_SecondaryTex ("SecondaryTexture", 2D) = "white" {}
		_SecondaryTransparentTex ("SecondaryTransparent", 2D) = "white" {}

		_ReflCubeTex ("ReflectionCube", Cube) = "defaulttexture" {}
		_ReflColor ("ReflColor", Color) = (1,1,1,1)
		_Grazing ("Grazing", Float) = 1

		Mixer ("Mix", Float) = 1
		Opacity ("Opacity", Float) = 1
		Interp ("Interp", Float) = 0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha

		Cull Off
		Pass	
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float oa : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float3 normalDir : TEXCOORD2;
	            float3 viewDir : TEXCOORD3;
//	            float3 misc : TEXCOORD4;
			};

			sampler2D _MainTex;
			sampler2D _TransparentTex;

			sampler2D _SecondaryTex;
			sampler2D _SecondaryTransparentTex;

			samplerCUBE _ReflCubeTex;
			float4 _ReflColor;
			float _Grazing;

			float4 _MainTex_ST;
			float Mixer;
			float Interp;
			float Opacity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				 float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
            	float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
            	o.normalDir.r = 1.0 - pow(-dot(-worldViewDir, worldNormal),_Grazing);
            	o.viewDir = reflect(-worldViewDir, worldNormal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 c0 = tex2D(_MainTex, i.uv);
				fixed4 a0 = tex2D(_TransparentTex, i.uv);

				fixed4 c1 = tex2D(_SecondaryTex, i.uv);
				fixed4 a1 = tex2D(_SecondaryTransparentTex, i.uv);
				fixed4 col;
				float mix = Interp*Mixer;
				col.a = a0.r*(1.0 - mix) + a1.r*(mix);
				float ii = pow(mix,2.0);
				col.rgb = c0.rgb*(1.0 - ii) + c1.rgb*( ii);
				col.a *= Opacity;

            	float3 reflColor = texCUBE(_ReflCubeTex, i.viewDir)*_ReflColor*i.normalDir.r;
            	col.rgb += reflColor;

				return col;
			}
			ENDCG
		}
	}
}
