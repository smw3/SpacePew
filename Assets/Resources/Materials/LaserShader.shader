Shader "Custom/Laser" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InterferenceTex ("Interference Texture", 2D) = "white" {}
	_MaxInterference ("Max Interference", Range (0.0, 1)) = 0.7
	_LastFrameTex ("Last Frame Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile_particles
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _LastFrameTex;
			sampler2D _InterferenceTex;
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};
			
			float4 _MainTex_ST;
			float4 _InterferenceTex_ST;
			float4 _LastFrameTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord1 = TRANSFORM_TEX(v.texcoord1,_InterferenceTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord2,_LastFrameTex); 
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _MaxInterference;
			
			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord) * tex2D(_InterferenceTex, i.texcoord2);
				
				fixed4 interference = _MaxInterference + tex2D(_InterferenceTex, i.texcoord1) * (1.0f - _MaxInterference);
				
				fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord) * interference + _TintColor * tex2D(_MainTex, i.texcoord) * 10;
				
				col.a = _TintColor.a * tex2D(_MainTex, i.texcoord).a;
				

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG 
		}
	}	
}
}
