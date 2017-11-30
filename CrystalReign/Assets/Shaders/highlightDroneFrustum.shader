// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
Shader "Custom/highlightDroneFrustum" {
	Properties {
		sectionHighlightColor("sectionHighlightColor", Color) = (0, 0.8125, 0, 0.5)
		isSectionHighlighted("isSectionHighlighted", Int) = 0
		isSectionSelected("isSectionSelected", Int) = 0
	}
	Subshader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Cull Off
			Blend DstAlpha Zero
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#if UNITY_EDITOR
				#pragma enable_d3d11_debug_symbols
			#endif
			#include "UnityCG.cginc"
			#include "Assets/Shaders/highlightDroneFrustumCommon.cginc"
			float4 sectionHighlightColor;
			int isSectionHighlighted;
			uniform float4 sectionSelectionColor;
			int isSectionSelected;
			fragIn vert(appdata_base i)
			{
				fragIn o;
				o.pos = UnityObjectToClipPos(i.vertex);
				//calcSlopeAndLocalSpacePos(i.vertex, i.normal, droneV, droneP, o.droneSpacePos, o.slope);
				// per vertex light
				calcLight(modelColor.xyz, i.vertex, i.normal, o.color);
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