Shader "Hidden/CTI/Internal-DeferredReflections" {
Properties {
	_SrcBlend ("", Float) = 1
	_DstBlend ("", Float) = 1
}
SubShader {

// Calculates reflection contribution from a single probe (rendered as cubes) or default reflection (rendered as full screen quad)
Pass {
	ZWrite Off
	ZTest LEqual
	Blend [_SrcBlend] [_DstBlend]
CGPROGRAM
#pragma target 3.0
#pragma vertex vert_deferred
#pragma fragment frag

#include "UnityCG.cginc"
#include "UnityDeferredLibrary.cginc"
#include "UnityStandardUtils.cginc"
#include "UnityStandardBRDF.cginc"
#include "UnityPBSLighting.cginc"

sampler2D _CameraGBufferTexture0;
sampler2D _CameraGBufferTexture1;
sampler2D _CameraGBufferTexture2;

half3 distanceFromAABB(half3 p, half3 aabbMin, half3 aabbMax)
{
	return max(max(p - aabbMax, aabbMin - p), half3(0.0, 0.0, 0.0));
}


half4 frag (unity_v2f_deferred i) : SV_Target
{
	// Stripped from UnityDeferredCalculateLightParams, refactor into function ?
	i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
	float2 uv = i.uv.xy / i.uv.w;

	// read depth and reconstruct world position
	float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
	depth = Linear01Depth (depth);
	float4 viewPos = float4(i.ray * depth,1);
	float3 worldPos = mul (unity_CameraToWorld, viewPos).xyz;

	half4 gbuffer0 = tex2D (_CameraGBufferTexture0, uv);
	half4 gbuffer1 = tex2D (_CameraGBufferTexture1, uv);
	half4 gbuffer2 = tex2D (_CameraGBufferTexture2, uv);

	// Check for translucent material
	half TransMat = floor(gbuffer2.a * 3 + 0.5f) == 2 ? 1 : 0;

	// Rewrite specColor if needed
	half3 specColor = (TransMat == 1)? gbuffer1.rrr : gbuffer1.rgb;
	half oneMinusRoughness = gbuffer1.a;
	half3 worldNormal = gbuffer2.rgb * 2 - 1;
	worldNormal = normalize(worldNormal);
	float3 eyeVec = normalize(worldPos - _WorldSpaceCameraPos);
	half oneMinusReflectivity = 1 - SpecularStrength(specColor.rgb);
	half occlusion = gbuffer0.a;

	half3 worldNormalRefl = reflect(eyeVec, worldNormal);
	float blendDistance = unity_SpecCube1_ProbePosition.w; // will be set to blend distance for this probe
	#if UNITY_SPECCUBE_BOX_PROJECTION
		// For box projection, use expanded bounds as they are rendered; otherwise
		// box projection artifacts when outside of the box.
		float4 boxMin = unity_SpecCube0_BoxMin - float4(blendDistance,blendDistance,blendDistance,0);
		float4 boxMax = unity_SpecCube0_BoxMax + float4(blendDistance,blendDistance,blendDistance,0);
		half3 worldNormal0 = BoxProjectedCubemapDirection (worldNormalRefl, worldPos, unity_SpecCube0_ProbePosition, boxMin, boxMax);
	#else
		half3 worldNormal0 = worldNormalRefl;
	#endif

	Unity_GlossyEnvironmentData g;
	g.roughness		= 1 - oneMinusRoughness;
	g.reflUVW		= worldNormal0;

	half3 env0 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, g);

	UnityLight light;
	light.color = 0;
	light.dir = 0;
	light.ndotl = 0;

	UnityIndirect ind;
	ind.diffuse = 0;
	ind.specular = env0 * occlusion;

	half3 rgb = UNITY_BRDF_PBS (0, specColor, oneMinusReflectivity, oneMinusRoughness, worldNormal, -eyeVec, light, ind).rgb;

	// Calculate falloff value, so reflections on the edges of the probe would gradually blend to previous reflection.
	// Also this ensures that pixels not located in the reflection probe AABB won't
	// accidentally pick up reflections from this probe.
	half3 distance = distanceFromAABB(worldPos, unity_SpecCube0_BoxMin.xyz, unity_SpecCube0_BoxMax.xyz);
	half falloff = saturate(1.0 - length(distance)/blendDistance);
	return half4(rgb, falloff);
}

ENDCG
}

// Adds reflection buffer to the lighting buffer
Pass
{
	ZWrite Off
	ZTest Always
	Blend [_SrcBlend] [_DstBlend]

	CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile ___ UNITY_HDR_ON

		#include "UnityCG.cginc"

		sampler2D _CameraReflectionsTexture;

		struct v2f {
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
		};

		v2f vert (float4 vertex : POSITION)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(vertex);
			o.uv = ComputeScreenPos (o.pos).xy;
			return o;
		}

		half4 frag (v2f i) : SV_Target
		{
			half4 c = tex2D (_CameraReflectionsTexture, i.uv);
			#ifdef UNITY_HDR_ON
			return float4(c.rgb, 0.0f);
			#else
			return float4(exp2(-c.rgb), 0.0f);
			#endif

		}
	ENDCG
}

}
Fallback Off
}
