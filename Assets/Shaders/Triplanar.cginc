// https://www.ronja-tutorials.com/post/010-triplanar-mapping/

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"

//texture and transforms of the texture
sampler2D _MainTex;
float4 _MainTex_ST;

fixed4 _Color;
float _Sharpness;

// for toon lighting
sampler2D _HeightMap;
sampler2D _NormalMap;
float _HeightIntensity;
float _NormalIntensity;
float4 _AmbientColor;
float _Glossiness;
float4 _SpecularColor;
float4 _FinalColor;

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
	fixed4 col_front = tex2D(_MainTex, uv_front);
	fixed4 col_side = tex2D(_MainTex, uv_side);
	fixed4 col_top = tex2D(_MainTex, uv_top);

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
	fixed4 sample = col_front + col_side + col_top;
		
	float3 normal = normalize(i.normal);
	float NdotL = dot(_WorldSpaceLightPos0, normal);

	// For spot/point lighting
	float4x4 modelMatrix = unity_ObjectToWorld;

	// Shadows
	float shadow1 = SHADOW_ATTENUATION(i);

	// Toonify it
	float lightIntensity = smoothstep(0, 0.01, NdotL * shadow1);
	float4 light = lightIntensity * _LightColor0;    

	float3 viewDir = normalize(i.viewDir);
	float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
	float NdotH = dot(normal, halfVector);

	float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
    
	// Toonify it
	float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
	float4 specular = specularIntensitySmooth * _SpecularColor;

	float4 multiplier = light + specular;

	// IF point or spot light
	// https://www.reddit.com/r/shaders/comments/5vmlm9/help_unity_cel_shader_point_light_troubles/
	#if defined (POINT) || defined (SPOT)
	float3 L = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
	float dist = length(L);
	float dot1 = max(dot(normalize(L), normal), 0);

	UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
	return _Color * sample * attenuation /** dot1 * multiplier*/ /*+ skyboxLight*/;

	#else

	return _Color * sample * multiplier /*+ skyboxLight*/;
    
	//return _Color * sample * (_AmbientColor + light);
    
	#endif

	//multiply texture color with tint color
}

#endif