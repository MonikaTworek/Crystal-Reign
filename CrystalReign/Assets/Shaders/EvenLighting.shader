// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/EvenLighting" {
	Properties {
		materialColor("Material Color", Color) = (1, 1, 1, 1)
		ambientColor("Ambient Color", Color) = (1, 1, 1, 1)
		darkening("Darkening Ratio", Float) = 1
		darkeningEnd("Darkening End Distance", Float) = 280
	}
	Subshader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Cull Off
			//Blend DstAlpha DstAlpha
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#if UNITY_EDITOR
				#pragma enable_d3d11_debug_symbols
			#endif
			#include "UnityCG.cginc"

			float4 ambientColor;
			float4 materialColor;
			float darkening;
			float darkeningEnd;

			struct fragIn {
				float4 pos : SV_POSITION;
				float slope : TEXCOORD3;
				float4 color : TEXCOORD1;
				//position local to drone camera
				float4 droneSpacePos : TEXCOORD0;
				float2 uv : TEXCOORD2;
			};

			fragIn vert(appdata_base i)
			{
				fragIn o;
				o.pos = UnityObjectToClipPos(i.vertex);

				float3 lookDir = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, i.vertex);
				float dist = min(distance(_WorldSpaceCameraPos.xyz, mul(unity_ObjectToWorld, i.vertex)), darkeningEnd);
				dist*=dist;
				float3 normal = mul(unity_ObjectToWorld, i.normal);

				fixed NdotL = max(dot(normalize(normal), normalize(lookDir.xyz)), 0);
				NdotL = NdotL*pow(cos(NdotL), 0.7) + 0.3;

				o.color.xyz = saturate(ambientColor.xyz + (materialColor.xyz*materialColor.w - ambientColor.xyz) * NdotL)*(1-dist*darkening*0.00001);
				o.color.w = materialColor.w;

				o.uv = i.texcoord.xy;
				return o;
			}

			fixed4 frag(fragIn i) : SV_Target
			{
				/*if (isSectionHighlighted == 1)
				{
					i.color = alphaBlend(i.color, sectionHighlightColor);
				}
				if (isSectionSelected == 1)
				{
					i.color = alphaBlend(i.color, sectionSelectionColor);
				}
				calcHighlightAndFootprintColor(i.uv, i.droneSpacePos, i.slope, i.color);*/
				return i.color;
			}
			ENDCG
		}
	}
}