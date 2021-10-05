/* #if !defined(MY_OUTLINE_INCLUDED)
#define MY_OUTLINE_INCLUDED */

#include "UnityCG.cginc"

struct appdata
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f
    {
        float4 vertex : SV_POSITION;
    };

    // Declare variables
    half _OutlineWidth;
    static const half4 OUTLINE_COLOR = half4(0,0,0,0);

    /* v2f vert (appdata v) {
    // Convert vertex position and normal to the clip space
    float4 clipPosition = UnityObjectToClipPos(v.vertex);
    float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));

    // Calculating vertex offset.
    // Taking into account "perspective division" and multiplying it with W component
    // to keep constant offset
    // independent from distance to the camera
    float2 offset = normalize(clipNormal.xy) * _OutlineWidth * clipPosition.w;

    // We also need take into account aspect ratio.
    // _ScreenParams - built-in Unity variable
    float aspect = _ScreenParams.x / _ScreenParams.y;
    offset.y *= aspect;

    // Applying offset
    clipPosition.xy += offset;

    v2f o;
    o.vertex = clipPosition;

    return o;
	} */

	v2f vert (appdata v)
    {
        // Offset vertices in the direction of the normal
        v.vertex.xyz += v.normal * _OutlineWidth;

        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);

        return o;
    }

    fixed4 frag () : SV_Target
    {
        // All pixels of the outline have the same constant color
        return OUTLINE_COLOR;
    }

/* #endif */