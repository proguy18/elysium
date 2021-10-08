Shader "Unlit/LavaFlow"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}	
	}
	SubShader
	{
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
				"RenderType" = "Opaque"
				}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			//#include "TutorialLighting.cginc"
			#include "LavaToonLighting.cginc"
			
			ENDCG
		}
		
		Pass {
            Tags {
                "LightMode" = "ForwardAdd"
            }

            Blend One One
            ZWrite Off

            CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

            #include "LavaToonLighting.cginc"	

            ENDCG
        }
	}
}