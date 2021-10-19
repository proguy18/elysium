Shader "Custom/Triplanar"{
	//show values to edit in inspector
	Properties{
		_Color ("Tint", Color) = (0, 0, 0, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_Sharpness ("Blend sharpness", Range(1, 64)) = 1
		_TextureScale ("Texture Scale", Range(0, 1000)) = 1
		
		// for toon lighting
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
		[HDR] _SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness("Glossiness", Float) = 32
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}

	SubShader{
		
		Pass{
			Tags {
				"LightMode" = "ForwardBase"
				}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fwdbase
			#pragma multi_compile _ SHADOWS_SCREEN

			#include "Triplanar.cginc"
			
			
			ENDCG
		}

		Pass {
            Tags {
                "LightMode" = "ForwardAdd"
            }

            Blend One One
            ZWrite Off

            CGPROGRAM
            #pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vert
			#pragma fragment frag

            #include "Triplanar.cginc"	

            ENDCG
        }

        Pass {
            Tags {
                "LightMode" = "ShadowCaster"
            }

            CGPROGRAM
            #pragma multi_compile_shadowcaster

            #pragma vertex vert
			#pragma fragment frag

            #include "ToonShadows.cginc"	

            ENDCG
        }
	}
	//FallBack "Standard" //fallback adds a shadow pass so we get shadows on other objects
}