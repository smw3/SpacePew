Shader "Custom/ShieldBubbleShader" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_PatternTex ("Pattern Texture", 2D) = "white" {}

	_RippleSpeed("Ripple Speed", float) = 2.0
	_RippleWidth("Ripple Width", float) = 10.0
	_RippleSizeMultiplier("Ripple Size Multiplier", float) = 3.0

	_RippleSineFalloff("Ripple Sine Falloff", float) = 2.0

	_RippleFadeTime("Ripple Fade Time", float) = 3.0

}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}
	Blend SrcAlpha OneMinusSrcAlpha, One One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off

	SubShader {
		GrabPass { }
	
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			fixed4 _TintColor;
			sampler2D _PatternTex;

			float _RippleSpeed;
			float _RippleWidth;
			float _RippleSizeMultiplier;

			float _RippleSineFalloff;
			float _RippleFadeTime;

			uniform int _Points_Length = 0;
			uniform float3 _Points[1023];		// (x, y, z) = position
			uniform float3 _Properties[1023];	// x = radius, y = intensity, z = time


			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float3 worldPos : Float3;
			};
			 
			float4 _PatternTex_ST;
			float4 _LastFrame_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					
				o.texcoord = TRANSFORM_TEX(v.texcoord,_PatternTex);
				o.texcoord1 = TRANSFORM_TEX(v.vertex.xz,_LastFrame);

				o.worldPos = mul(_Object2World, v.vertex).xyz;
				// Correction offset
				o.texcoord1 += 0.5;

				return o;
			}
			
			float4 frag (v2f iv) : SV_Target
			{

				// Loops over all the points
				half h = 0;
				for (int i = 0; i < _Points_Length; i++)
				{
					// Calculates the contribution of each point
					half dist = distance(iv.worldPos, _Points[i].xyz);
					half radius = _RippleSizeMultiplier * _Properties[i].x;

					half hi = 1 - saturate(dist / radius);

					half time_passed = _Time[1] - _Properties[i].z;
					half intensity = saturate(_Properties[i].y * (1 - (time_passed / _RippleFadeTime)));
						 
					half point_influence = saturate(hi * intensity * sin(_RippleWidth * dist - _RippleSpeed * time_passed) / (_RippleSineFalloff * dist));

					h += point_influence;
				}

				float4 col = float4(1.0, 1.0, 1.0, 1.0);

				col *= saturate(h);
				col *= _TintColor * tex2D(_PatternTex, iv.texcoord);

				return col;
			}
			ENDCG 
		}
	}	
}
}

