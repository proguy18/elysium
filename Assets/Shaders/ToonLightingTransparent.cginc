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

sampler2D _MainTex;
sampler2D _HeightMap;
sampler2D _NormalMap;
sampler2D _SkyboxLight;
float _HeightIntensity;
float _NormalIntensity;
float4 _Color;
float4 _MainTex_ST;
float4 _AmbientColor;
float _Glossiness;
float4 _SpecularColor;
float4 _RimColor;
float _RimAmount;
float _RimThreshold;
float4 _FinalColor;

/* float4 _OutlineColor;
float _LitOutlineThickness;
float _UnlitOutlineThickness;
float4 outline; */

vertOut vert (vertIn v)
{
    vertOut o;

    o.uv = TRANSFORM_TEX(v.uv, _MainTex);

    #if HEIGHTMAP_ON
    // Height mapping
    // https://forum.unity.com/threads/moving-vertices-based-on-a-heightmap.89478/
    float height = tex2Dlod(_HeightMap, float4(o.uv, 0, 0)).x;
    height = (height * 2 - 1);
    //v.normal = normalize(v.normal);
    v.vertex.xyz += v.normal * height * _HeightIntensity;
    #endif

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

float4 applyFog (float4 color, vertOut i) {
    float viewDistance = length(_WorldSpaceCameraPos - i.worldPos);
    UNITY_CALC_FOG_FACTOR_RAW(viewDistance);
    
    color.rgb = lerp(unity_FogColor.rgb, color.rgb, saturate(unityFogFactor*2000));
    return color;
    
}

/*float2 DirToRectilinear(float3 dir)
{
    float x = atan2(dir.z, dir.z) / TAU + 0.5;
    float y = dir.y * 0.5 + 0.5;
    return float2(x,y);
}*/

float4 frag (vertOut i) : SV_Target
{
    float4 sample = tex2D(_MainTex, i.uv);

    // Normal maps
    float3 tangentSpaceNormal = UnpackNormal(tex2D(_NormalMap, i.uv));
    tangentSpaceNormal = normalize(lerp(float3(0,0,1), tangentSpaceNormal, _NormalIntensity));

    float3x3 mtxTangToWorld = {
        i.tangent.x, i.bitangent.x, i.worldNormal.x,
        i.tangent.y, i.bitangent.y, i.worldNormal.y,
        i.tangent.z, i.bitangent.z, i.worldNormal.z
    };

    float3 normal = mul(mtxTangToWorld, tangentSpaceNormal);

    //normal = normalize(i.worldNormal);
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

    float4 multiplier = light + specular + rim;

    float4 skyboxLight = 0;

    #ifdef IS_IN_BASE_PASS
        skyboxLight = float4(tex2D(_SkyboxLight, i.uv));
        multiplier += skyboxLight;
    #endif

    //skyboxLight = float4(tex2D(_SkyboxLight, i.uv))/10;
    
    // IF point or spot light
    // https://www.reddit.com/r/shaders/comments/5vmlm9/help_unity_cel_shader_point_light_troubles/
    #if defined (POINT) || defined (SPOT)
        float3 L = _WorldSpaceLightPos0.xyz - i.worldPos.xyz;
        float dist = length(L);
        float dot1 = max(dot(normalize(L), normal), 0);

        UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos.xyz);
        // return _Color * sample * attenuation * dot1 * multiplier /*+ skyboxLight*/;
        return float4((_Color * sample * attenuation * dot1 * multiplier).rgb, 0.1);

    #else

    // Outlining
    /* float3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos.xyz);
    if (dot(viewDir, normal) < lerp(_UnlitOutlineThickness, _LitOutlineThickness, max(0.0, dot(normal, lightDir)))) {
        outline = float4(_LightColor0.rgb * _OutlineColor.rgb, 0);
    } */

    //float4 color = _Color * sample * (_AmbientColor + light + rim);
    //return applyFog(color, i);

    //return _Color * sample * multiplier /*+ skyboxLight*/;
    return float4((_Color * sample * multiplier).rgb, 0.1);
    
    //return _Color * sample * (_AmbientColor + light);
    
    #endif
}

#endif