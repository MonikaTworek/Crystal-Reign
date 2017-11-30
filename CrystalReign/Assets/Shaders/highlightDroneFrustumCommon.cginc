// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
//#include "Assets/Shaders/texSample.cginc"
struct fragIn {
	float4 pos : SV_POSITION;
	float slope : TEXCOORD3;
	float4 color : TEXCOORD1;
	//position local to drone camera
	float4 droneSpacePos : TEXCOORD0;
	float2 uv : TEXCOORD2;
};
//drone camera data
uniform float4x4 imagesAreaV;
uniform float4x4 imagesAreaP;
uniform sampler2D imagesAreaDepthMap;
uniform float4 imagesAreaDepthMapInvSize;
uniform sampler2D imagesAreaTex;
uniform float4 imagesAreaInvSize;
uniform float4x4 droneV;
uniform float4x4 droneP;
uniform sampler2D droneCamDepthMap;
uniform float4 droneDepthMapInvSize;
uniform int useDirectionalLights;
uniform float4 lightDir;
uniform float4 lightDiff;
uniform float4 modelColor;
uniform int useHighlight;
uniform int useImagesArea;
uniform float4 highlightColor;
uniform float4 footprintColor;
void calcLight(float3 ambient, float4 pos, float3 normal, inout float4 color)
{
	float3 lookDir = _WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, pos);
	normal = mul(unity_ObjectToWorld, normal);
	fixed NdotL = max(dot(normalize(normal), normalize(lookDir.xyz)), 0);
	NdotL /= pow(cos(NdotL), -0.7) * 1.0;
	NdotL += 0.3;
	//ambient.xyz = float3(1, 1, 1);
	//lightDiff.xyzw = float4(1, 1, 1, 1);
	//color.xyz = saturate(ambient.xyz + (lightDiff.xyz*lightDiff.w - ambient.xyz) * NdotL * useDirectionalLights);
	color.xyz = float3(NdotL, NdotL, NdotL);
	color.w = 1;
}
//calculate slope and position local to object with V
/*void calcSlopeAndLocalSpacePos(float4 pos, float3 normal, float4x4 V, float4x4 P, out float4 droneSpacePos, out float slope)
{
	droneSpacePos = mul(mul(mul(P, V), unity_ObjectToWorld), pos);
	slope = 1 - max(dot(mul(mul(V, unity_ObjectToWorld), normal), -mul(mul(V, unity_ObjectToWorld), pos)), 0);
}
int isSeenFrom(float4 pos, float slope, sampler2D depthMap, float2 depthMapInvSize)
{
	//pixel is behind us
	if (pos.z < 0)
	{
		return 0;
	}
	//from [-1,1] to [0, 1]
	pos.xyz /= pos.w;
	pos.xy += float2(1, 1);
	pos.xy *= 0.5;
	//pixel is outside frustum
	if (pos.x > 1 || pos.x < 0 ||
		pos.y > 1 || pos.y < 0)
	{
		return 0;
	}
	float epsilon = 0.00043;
	//fix values by slope factor
	if (slope > 0.8)
	{
		// too paraller to see anything
		return 0;
	}
	// depth as seen from pos
	float depthFromMap = sampleDepth4x(depthMap, pos.xy, 10, depthMapInvSize);
	// f: [0.6, 1.0] -> [0, 1] 
	float f = (slope - 0.6)*(1.0 / 0.4);
	// smoothed and multisampled depth
	float depthFromMap2 = sampleDepth4x(depthMap, pos.xy, 15, depthMapInvSize) * f + depthFromMap * (1 - f);
	if (slope > 0.6)
	{
		//self-shading fix
		epsilon += f * 0.00794;
		depthFromMap = depthFromMap2;
	}
	if (pos.z - epsilon > (depthFromMap * 2 - 1))
	{
		//pixel is covered
		return 0;
	}
	return 1;
}
void calcHighlightAndFootprintColor(float2 uv, float4 droneSpacePos, float slope, inout float4 color)
{
	// how many pictures cover that pixel
	float4 footprintColorSample = sampleTex(imagesAreaTex, uv);
	float4 footprintColorSample2 = sampleTex4xMax(imagesAreaTex, uv, 1, imagesAreaInvSize);
	if (useImagesArea == 1)
	{
		if (footprintColorSample.a > 0)
		{
			color = alphaBlend(color, footprintColorSample);
		}
		else if (footprintColorSample2.a > 0)
		{
			color = alphaBlend(color, footprintColorSample2);
		}
	}
	//return here if prjector is off
	if (useHighlight == 1 && isSeenFrom(droneSpacePos, slope, droneCamDepthMap, droneDepthMapInvSize))
	{
		color = alphaBlend(color, highlightColor);
	}
}*/