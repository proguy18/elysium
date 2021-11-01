#include "UnityCG.cginc"

struct vertIn
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct vertOut
    {
        float4 vertex : SV_POSITION;
    };

    // Declare variables
    half _OutlineWidth;
    static const half4 OUTLINE_COLOR = half4(0,0,0,0);

	vertOut vert (vertIn v)
    {
        // Offset vertices in the direction of the normal
        v.vertex.xyz += v.normal * _OutlineWidth;

        vertOut o;
        o.vertex = UnityObjectToClipPos(v.vertex);

        return o;
    }

    fixed4 frag () : SV_Target
    {
        // All pixels of the outline have the same constant color
        return OUTLINE_COLOR;
    }
