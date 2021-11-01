// https://catlikecoding.com/unity/tutorials/flow/waves/

#if !defined(LAVA_LIGHTING_INCLUDED)
#define LAVA_LIGHTING_INCLUDED

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _FlowMap;
sampler2D _NormalMap;
float _UJump;
float _VJump;
float _Tiling;
float _Speed;
float _FlowStrength;
float _FlowOffset;
float4 _Color;

struct vertIn
{
    float4 vertex : POSITION;				
    float4 uv : TEXCOORD0;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
};

struct vertOut
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 worldNormal : NORMAL;
    float3 viewDir : TEXCOORD1;
    float3 worldPos : TEXCOORD3;
    float3 tangent  : TEXCOORD4;
    float3 bitangent : TEXCOORD5;
    SHADOW_COORDS(2)
};


// Implementation of the vertex shader
vertOut vert(vertIn v)
{
    vertOut o;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.pos = UnityObjectToClipPos(v.vertex);
    o.worldNormal = UnityObjectToWorldNormal(v.normal);

    // Normal mapping
    o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
    o.bitangent = cross(o.worldNormal, o.tangent) * (v.tangent.w * unity_WorldTransformParams.w);

    o.viewDir = WorldSpaceViewDir(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    TRANSFER_SHADOW(o);

    return o;
}

#if !defined(FLOW_INCLUDED)
#define FLOW_INCLUDED

float3 FlowUV (float2 uv, float2 flowVector, float2 jump,
    float flowOffset, float tiling, float time, bool flowB
    ) {
    float phaseOffset = flowB ? 0.5 : 0;
    float progress = frac(time + phaseOffset);
    float3 uvw;
    uvw.xy = uv - flowVector * (progress + flowOffset);
    uvw.xy *= tiling;
    uvw.xy += phaseOffset;
    uvw.xy += (time - progress) * jump;
    uvw.z = 1 - abs(1 - 2 * progress);
    return uvw;
}
#endif

float4 frag (vertOut i) : SV_Target
{
    // Flow effect
    float2 flowVector = tex2D(_FlowMap, i.uv).rg * 2 - 1;
    flowVector *= _FlowStrength;
    float noise = tex2D(_FlowMap, i.uv).a;
    float time = _Time.y * _Speed + noise;
    float2 jump = float2(_UJump, _VJump);
    
    float3 uvwA = FlowUV(i.uv, flowVector, jump, _FlowOffset, _Tiling, time, false);
    float3 uvwB = FlowUV(i.uv, flowVector, jump, _FlowOffset, _Tiling, time, true);

    float3 normalA = UnpackNormal(tex2D(_NormalMap, uvwA.xy)) * uvwA.z;
    float3 normalB = UnpackNormal(tex2D(_NormalMap, uvwB.xy)) * uvwB.z;
    float3 normal = normalize(normalA + normalB);
    
    float4 sample1 = tex2D(_MainTex, uvwA.xy) * uvwA.z;
    float4 sample2 = tex2D(_MainTex, uvwB.xy) * uvwB.z;

    float4 sample = (sample1 + sample2);
    
    #if defined (POINT) || defined (SPOT)
        float3 L = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
        float dist = length(L);
        float dot1 = max(dot(normalize(L), normal), 0);
    
        UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
        return _Color * sample * attenuation * _LightColor0;

    #else

    return _Color * sample * _LightColor0;

    #endif
}

#endif