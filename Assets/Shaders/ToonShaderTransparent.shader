Shader "Unlit/ToonTransparent"
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
		
		_SkyboxLight ("Skybox Light", 2D) = "black" {}

		_HeightMap ("Height Map", 2D) = "black" {}
		_HeightIntensity ("Height Intensity", Range(0, 1)) = 0

		_NormalMap ("Normal Map", 2D) = "bump" {}
		_NormalIntensity ("Normal Intensity", Range(0,10)) = 0

		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
      	_LitOutlineThickness ("Lit Outline Thickness", Range(0,1)) = 0.1
      	_UnlitOutlineThickness ("Unlit Outline Thickness", Range(0,1)) = 0.4

		_OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.00001
	}
	SubShader
	{
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
				"Queue"="Transparent"
				"RenderType"="Transparent"
				"IgnoreProjector"="True"
				}

			/*Cull Off

			Stencil {
				Ref 1
				Comp Always
				Pass Replace
			}*/
			
			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma multi_compile HEIGHTMAP_ON HEIGHTMAP_OFF

            #pragma multi_compile _ SHADOWS_SCREEN

            #include "ToonLightingTransparent.cginc"			
			
			ENDCG
		}
		
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
				"Queue"="Transparent"
				"RenderType"="Transparent"
				"IgnoreProjector"="True"
				}

			/*Cull Off

			Stencil {
				Ref 1
				Comp Always
				Pass Replace
			}*/
			
			Cull Back
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog

			#pragma multi_compile HEIGHTMAP_ON HEIGHTMAP_OFF

            #pragma multi_compile _ SHADOWS_SCREEN

            #include "ToonLightingTransparent.cginc"			
			
			ENDCG
		}

		Pass {
			Cull Front

			/*Stencil {
				Ref 1
				Comp Greater
			}*/

			CGPROGRAM
			#pragma vertex vert
    		#pragma fragment frag

			#include "ToonOutlines.cginc"

			ENDCG
		}

        Pass {
            Tags {
                "LightMode" = "ForwardAdd"
            	"Queue"="Transparent"
				"RenderType"="Transparent"
            	"IgnoreProjector"="True"
            }

            //Blend One One
        	Cull Front
        	Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

            #include "ToonLightingTransparent.cginc"	

            ENDCG
        }
		
		Pass {
            Tags {
                "LightMode" = "ForwardAdd"
            	"Queue"="Transparent"
				"RenderType"="Transparent"
            	"IgnoreProjector"="True"
            }

            //Blend One One
        	Cull Back
        	Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

            #include "ToonLightingTransparent.cginc"	

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