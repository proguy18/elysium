// https://catlikecoding.com/unity/tutorials/flow/waves/
// https://catlikecoding.com/unity/tutorials/flow/texture-distortion/

Shader "Unlit/LavaFlow"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		//_WaveA("Wave A (dir, steepness, wavelength)", Vector) = (1,0,0.5,10)
		[NoScaleOffset] _FlowMap ("Flow", 2D) = "black" {}
		[NoScaleOffset] _NormalMap ("Normals", 2D) = "bump" {}
		_UJump ("U jump per phase", Range(-0.25, 0.25)) = 0.25
		_VJump ("V jump per phase", Range(-0.25, 0.25)) = 0.25
		_Tiling ("Tiling", Float) = 1
		_Speed ("Speed", Float) = 1
		_FlowStrength ("Flow Strength", Float) = 1
		_FlowOffset ("Flow offset", Float) = 0
	}
	SubShader
	{
		Pass {
			Tags {
				"LightMode" = "ForwardBase"
				}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
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
			#pragma multi_compile_fwdadd
            
			#pragma vertex vert
			#pragma fragment frag

            #include "LavaToonLighting.cginc"	

            ENDCG
        }
	}
}