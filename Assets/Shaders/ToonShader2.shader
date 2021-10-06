Shader "Unlit/Toon2"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}	
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
		[HDR] _SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		_Glossiness("Glossiness", Float) = 32
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		_HeightMap ("Height Map", 2D) = "black" {}
		_HeightIntensity ("Height Intensity", Range(0, 1)) = 1

		_NormalMap ("Normal Map", 2D) = "bump" {}
		_NormalIntensity ("Normal Intensity", Range(0,10)) = 1

		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
      	_LitOutlineThickness ("Lit Outline Thickness", Range(0,1)) = 0.1
      	_UnlitOutlineThickness ("Unlit Outline Thickness", Range(0,1)) = 0.4

		_OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
	}
	SubShader
	{
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
				}

			//Cull Off

			//Stencil {
			//	Ref 1
			//	Comp Always
			//	Pass Replace
			//}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma multi_compile HEIGHTMAP_ON HEIGHTMAP_OFF

            #pragma multi_compile _ SHADOWS_SCREEN

            #include "ToonLighting.cginc"			
			
			ENDCG
		}

		Pass {
			Cull Front

			//Stencil {
			//	Ref 1
			//	Comp Greater
			//}

			CGPROGRAM
			#pragma vertex vert
    		#pragma fragment frag

			#include "ToonOutlines.cginc"

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
			#pragma multi_compile_fog

            #include "ToonLighting.cginc"	

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
	CustomEditor "HeightMap"

}