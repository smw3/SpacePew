Shader "Custom/ShieldBubbleShader" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_PatternTex ("Pattern Texture", 2D) = "white" {}
	_HitPosition ("Hit Position", Vector) = (0,0,0,0)
	_HitIntensity ("Hit Intensity", float) = 0.0
	_TimeOfHit ("Time since last Hit", float) = 0.0
	_LastFrame ("Last Frame", 2D) = "white" {}
	// I'm a test change to test changes
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
			sampler2D _LastFrame;

			fixed4 _TintColor;
			float3 _HitPosition;
			
			float _HitIntensity;
			float _TimeOfHit;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float distanceSquared : Float;
			};
			
			float4 _PatternTex_ST;
			float4 _LastFrame_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				
				// Compute the distance squared of the last hit to the current vertex
				float3 worldPos = mul(_Object2World, v.vertex).xyz;
				
				float relativeX = worldPos.x - _HitPosition.x;
				float relativeY = worldPos.y - _HitPosition.y;
				float relativeZ = worldPos.z - _HitPosition.z;
				
				o.distanceSquared = relativeX * relativeX + relativeY * relativeY + relativeZ * relativeZ;
				
				
				o.texcoord = TRANSFORM_TEX(v.texcoord,_PatternTex);
				o.texcoord1 = TRANSFORM_TEX(v.vertex.xz,_LastFrame);

				// Correction offset
				o.texcoord1 += 0.5;

				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float distance1 = _HitIntensity * 3.0;
				float distance2 = _HitIntensity * 20.0;
				float distance3 = _HitIntensity * 100.0;			
			
				float fade1 = max((distance1 - i.distanceSquared) / distance1, 0.0);
				float fade2 = max((distance2 - i.distanceSquared) / distance2, 0.0);
				float fade3 = max((distance3 - i.distanceSquared) / distance3, 0.0);
				
				float timeFade = (_Time.y - _TimeOfHit) > 0.01 ? 0.0 : 1.0; // Not sure about this.
				
				float totalFade = (fade1*0.4 + fade2*0.2 + fade3*0.1) * timeFade;
				
				//float4 col = _TintColor * tex2D(_PatternTex, i.texcoord);
				float4 col = float4(1.0, 1.0, 1.0, 1.0);
				col *= totalFade;

				col *= _TintColor * tex2D(_PatternTex, i.texcoord);
				
				float4 newCol = tex2D(_LastFrame, i.texcoord1) * 0.99;
				
				col = max(col, newCol);
							
				return col;
			}
			ENDCG 
		}
	}	
}
}

