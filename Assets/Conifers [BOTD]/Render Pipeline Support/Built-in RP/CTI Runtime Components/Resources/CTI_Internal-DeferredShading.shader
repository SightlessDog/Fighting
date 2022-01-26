Shader "Hidden/CTI/Internal-DeferredShading" {
Properties {
	_LightTexture0 ("", any) = "" {}
	_LightTextureB0 ("", 2D) = "" {}
	_ShadowMapTexture ("", any) = "" {}
	_SrcBlend ("", Float) = 1
	_DstBlend ("", Float) = 1
}
SubShader {

// Pass 1: Lighting pass
//  LDR case - Lighting encoded into a subtractive ARGB8 buffer
//  HDR case - Lighting additively blended into floating point buffer
Pass {
	ZWrite Off
	Blend [_SrcBlend] [_DstBlend]

CGPROGRAM
#pragma target 3.0
#pragma vertex vert_deferred
#pragma fragment frag
#pragma multi_compile_lightpass
#pragma multi_compile ___ UNITY_HDR_ON

#pragma exclude_renderers nomrt

#include "UnityCG.cginc"
#include "UnityDeferredLibrary.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardUtils.cginc"
#include "UnityStandardBRDF.cginc"

sampler2D _CameraGBufferTexture0;
sampler2D _CameraGBufferTexture1;
sampler2D _CameraGBufferTexture2;
		
half4 CalculateLight (unity_v2f_deferred i)
{
	float3 wpos;
	float2 uv;
	float atten, fadeDist;
	UnityLight light;
	UNITY_INITIALIZE_OUTPUT(UnityLight, light);
	UnityDeferredCalculateLightParams (i, wpos, uv, light.dir, atten, fadeDist);

	half4 gbuffer0 = tex2D (_CameraGBufferTexture0, uv);
	half4 gbuffer1 = tex2D (_CameraGBufferTexture1, uv);
	half4 gbuffer2 = tex2D (_CameraGBufferTexture2, uv);

	// Check for translucent material
	half TransMat = floor(gbuffer2.a * 3 + 0.5f) == 2 ? 1 : 0;

	light.color = _LightColor.rgb * atten;
	half3 baseColor = gbuffer0.rgb;

	// Rewrite specColor if needed
	half3 specColor = (TransMat == 1)? gbuffer1.rrr : gbuffer1.rgb;
	

	half oneMinusRoughness = gbuffer1.a;
	half3 normalWorld = gbuffer2.rgb * 2 - 1;
	normalWorld = normalize(normalWorld);
	float3 eyeVec = normalize(wpos-_WorldSpaceCameraPos);
	half oneMinusReflectivity = 1 - SpecularStrength(specColor.rgb);
	light.ndotl = LambertTerm(normalWorld, light.dir);

	UnityIndirect ind;
	UNITY_INITIALIZE_OUTPUT(UnityIndirect, ind);
	ind.diffuse = 0;
	ind.specular = 0;


//	Energy conserving wrapped around diffuse lighting / http://blog.stevemcauley.com/2011/12/03/energy-conserving-wrapped-diffuse/
	half wrap1 = 0.4;
	half NdotLDirect = saturate( ( dot(normalWorld, light.dir) + wrap1 ) / ( (1 + wrap1) * (1 + wrap1) ) );
	//light.ndotl = (gbuffer2.a == 0) ? wrappedNdotL1 : LambertTerm(normalWorld, light.dir); // As Unity 5.5. does not know light.ndotl anymore
	NdotLDirect = /*TransMat ? NdotLDirect :*/ saturate(dot(normalWorld, light.dir));


    half4 res = UNITY_BRDF_PBS (baseColor, specColor, oneMinusReflectivity, oneMinusRoughness, normalWorld, -eyeVec, light, ind);

/*	Deprecated
//	Thin Layer Translucency
	// Only using dotNL gives us more lively lighting beyond the shadow distance.
	half backlight = saturate( dot(-normalWorld, light.dir) + 0.2);
	half fresnel = (1.0 - backlight) * (1.0 - backlight);
	fresnel *= fresnel;
	//#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
	res.rgb += baseColor * backlight * (1.0 - fresnel) * 4.0f * gbuffer1.b * light.color * TransMat;
*/

	UNITY_BRANCH
	if(TransMat * gbuffer1.b) {
		half transPower = gbuffer1.g * 10.0f;
		half3 transLightDir = light.dir + normalWorld * 0.01;
		half transDot = dot( transLightDir, eyeVec ); // sign(minus) comes from eyeVec
		transDot = exp2(saturate(transDot) * transPower - transPower);
		half3 lightScattering = transDot * light.color * (1.0 - saturate(dot(normalWorld, light.dir)) );
		res.rgb += baseColor * 4.0 * gbuffer1.b * lightScattering;
	}
	
	return res;
}

#ifdef UNITY_HDR_ON
half4
#else
fixed4
#endif
frag (unity_v2f_deferred i) : SV_Target
{
	half4 c = CalculateLight(i);
	#ifdef UNITY_HDR_ON
	return c;
	#else
	return exp2(-c);
	#endif
}

ENDCG
}


// Pass 2: Final decode pass.
// Used only with HDR off, to decode the logarithmic buffer into the main RT
Pass {
	ZTest Always Cull Off ZWrite Off
	Stencil {
		ref [_StencilNonBackground]
		readmask [_StencilNonBackground]
		// Normally just comp would be sufficient, but there's a bug and only front face stencil state is set (case 583207)
		compback equal
		compfront equal
	}

CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers nomrt

sampler2D _LightBuffer;
struct v2f {
	float4 vertex : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

v2f vert (float4 vertex : POSITION, float2 texcoord : TEXCOORD0)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(vertex);
	o.texcoord = texcoord.xy;
	#ifdef UNITY_SINGLE_PASS_STEREO
		o.texcoord = TransformStereoScreenSpaceTex(o.texcoord, 1.0f);
	#endif
	return o;
}

fixed4 frag (v2f i) : SV_Target
{
	return -log2(tex2D(_LightBuffer, i.texcoord));
}
ENDCG 
}

}
Fallback Off
}
