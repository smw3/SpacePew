Shader "Custom/UVTest" {
Properties {
	_PatternTex ("Pattern Texture", 2D) = "white" {}
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

			sampler2D _PatternTex;
			
			struct appdata_t {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _PatternTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				o.texcoord.xy = v.vertex.xz;

				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
			 	fixed4 col = tex2D(_PatternTex, i.texcoord);
				return col;
			}
			ENDCG 
		}
	}	
}
}

