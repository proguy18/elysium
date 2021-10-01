// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Adapted from https://roystan.net/articles/toon-shader.html

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "Lighting.cginc"
#include "AutoLight.cginc"
#include "UnityCG.cginc"

struct vertIn
{
    float4 vertex : POSITION;				
    float4 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

struct vertOut
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 worldNormal : NORMAL;
    float3 viewDir : TEXCOORD1;
    float3 worldPos : TEXCOORD3;
    SHADOW_COORDS(2)
};

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _AmbientColor;
float _Glossiness;
float4 _SpecularColor;
float4 _RimColor;
float _RimAmount;
float _RimThreshold;

/* float4 _OutlineColor;
float _LitOutlineThickness;
float _UnlitOutlineThickness;
float4 outline; */

vertOut vert (vertIn v)
{
    vertOut o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.worldNormal = UnityObjectToWorldNormal(v.normal);
    o.viewDir = WorldSpaceViewDir(v.vertex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    TRANSFER_SHADOW(o);
    return o;
}

float4 _Color;

float4 applyFog (float4 color, vertOut i) {
    float viewDistance = length(_WorldSpaceCameraPos - i.worldPos);
    UNITY_CALC_FOG_FACTOR_RAW(viewDistance);
    
    color.rgb = lerp(unity_FogColor.rgb, color.rgb, saturate(unityFogFactor*2000));
    return color;
    
}

float4 frag (vertOut i) : SV_Target
{
    float3 normal = normalize(i.worldNormal);
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

    // Rim lighting
    float4 rimDot = 1 - dot(viewDir, normal);
    float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
    rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
    float4 rim = rimIntensity * _RimColor;

    float4 sample = tex2D(_MainTex, i.uv);

    // IF point or spot light
    #if defined (POINT) || defined (SPOT)
        float3 L = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
        float dist = length(L);
        float dot1 = max(dot(normalize(L), normal), 0);

        UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
        return _Color * sample * attenuation * dot1 * (_AmbientColor + light + specular + rim);

    #else

    // Outlining
    /* float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
    if (dot(viewDir, normal) < lerp(_UnlitOutlineThickness, _LitOutlineThickness, max(0.0, dot(normal, lightDir)))) {
        outline = float4(_LightColor0.rgb * _OutlineColor.rgb, 0);
    } */

    //float4 color = _Color * sample * (_AmbientColor + light + rim);
    //return applyFog(color, i);

    //return _Color * sample * (_AmbientColor + light + rim);
    return _Color * sample * (_AmbientColor + light + specular + rim);
    
    //return _Color * sample * (_AmbientColor + light);
    
    #endif
}

#endif