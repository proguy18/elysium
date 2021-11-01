// https://www.ronja-tutorials.com/post/010-triplanar-mapping/

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "UnityLightingCommon.cginc"

//texture and transforms of the texture
sampler2D _MainTex;
float4 _MainTex_ST;

float4 _Color;
float _Sharpness;
float _TextureScale;

struct vertIn{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
};

struct vertOut{
	float4 pos : SV_POSITION;
	float3 worldPos : TEXCOORD0;
	float3 normal : NORMAL;
	float3 viewDir : TEXCOORD1;
	float3 tangent  : TEXCOORD3;
	float3 bitangent : TEXCOORD4;
	SHADOW_COORDS(2)
};

vertOut vert(vertIn v){
	vertOut o;
	//calculate the position in clip space to render the object
	o.pos = UnityObjectToClipPos(v.vertex);
	//calculate world position of vertex
	float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
	o.worldPos = worldPos.xyz;
	//calculate world normal
	o.normal = UnityObjectToWorldNormal(v.normal);
	o.viewDir = WorldSpaceViewDir(v.vertex);

	// Normal mapping
	o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
	o.bitangent = cross(o.normal, o.tangent) * (v.tangent.w * unity_WorldTransformParams.w);
	TRANSFER_SHADOW(o);
	
	return o;
}

fixed4 frag(vertOut i) : SV_TARGET{
	//calculate UV coordinates for three projections
	float2 uv_front = TRANSFORM_TEX(i.worldPos.xy, _MainTex);
	float2 uv_side = TRANSFORM_TEX(i.worldPos.zy, _MainTex);
	float2 uv_top = TRANSFORM_TEX(i.worldPos.xz, _MainTex);
	
	//read texture at uv position of the three projections
	fixed4 col_front = tex2D(_MainTex, uv_front / _TextureScale);
	fixed4 col_side = tex2D(_MainTex, uv_side / _TextureScale);
	fixed4 col_top = tex2D(_MainTex, uv_top / _TextureScale);

	//generate weights from world normals
	float3 weights = i.normal;
	//show texture on both sides of the object (positive and negative)
	weights = abs(weights);
	//make the transition sharper
	weights = pow(weights, _Sharpness);
	//make it so the sum of all components is 1
	weights = weights / (weights.x + weights.y + weights.z);

	//combine weights with projected colors
	col_front *= weights.z;
	col_side *= weights.x;
	col_top *= weights.y;

	//combine the projected colors
	float4 sample = (col_front + col_side + col_top) + _Color;
	float4 sample1 = (col_front + col_side + col_top);
	
	float3 normal = normalize(i.normal);
	float NdotL = dot(_WorldSpaceLightPos0.xyz, normal);

	// Shadows
	float shadow1 = SHADOW_ATTENUATION(i);

	///// start tutorial lighting adaptation /////

	// Calculate ambient RGB intensities
	float Ka = 0.3;
	float3 amb = sample1.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;
	
	float fAtt = 1;
	float Kd = 1;
	float3 L1 = _WorldSpaceLightPos0; 
	float LdotN = NdotL;
	float3 dif = fAtt * _LightColor0 * Kd * sample.rgb * shadow1;
	
	float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
	returnColor.rgb = amb.rgb + dif.rgb/* + spe.rgb*/;

	#if defined (POINT) || defined (SPOT)
		float3 L = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
		float dist = length(L);
		float dot1 = max(dot(normalize(L), normal), 0);

		// https://janhalozan.com/2017/08/12/phong-shader/
		float oneOverDistance = 1.0 / length(L);

		UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
		returnColor *= attenuation * dot1;
		return returnColor;

	#else

	return returnColor;
    
	#endif
}

#endif