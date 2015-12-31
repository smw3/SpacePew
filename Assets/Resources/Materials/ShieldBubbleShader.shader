Shader "Custom/ShieldBubbleShader" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_PatternTex ("Pattern Texture", 2D) = "white" {}

	_LastFrame ("Last Frame", 2D) = "white" {}
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
			float4x4 _ShieldHitBuffer;


			fixed4 _TintColor;
			
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

			// Note: I did not write this. wtf
			float Color2Float(float4 c)
			{
				float f;
				uint fi;

				fi = c.x * 255;
				uint i32 = (fi & 128) / 128;
				uint i31 = (fi & 64) / 64;
				uint i30 = (fi & 32) / 32;
				uint i29 = (fi & 16) / 16;
				uint i28 = (fi & 8) / 8;
				uint i27 = (fi & 4) / 4;
				uint i26 = (fi & 2) / 2;
				uint i25 = (fi & 1) / 1;

				fi = c.y * 255;
				uint i24 = (fi & 128) / 128;
				uint i23 = (fi & 64) / 64;
				uint i22 = (fi & 32) / 32;
				uint i21 = (fi & 16) / 16;
				uint i20 = (fi & 8) / 8;
				uint i19 = (fi & 4) / 4;
				uint i18 = (fi & 2) / 2;
				uint i17 = (fi & 1) / 1;

				fi = c.z * 255;
				uint i16 = (fi & 128) / 128;
				uint i15 = (fi & 64) / 64;
				uint i14 = (fi & 32) / 32;
				uint i13 = (fi & 16) / 16;
				uint i12 = (fi & 8) / 8;
				uint i11 = (fi & 4) / 4;
				uint i10 = (fi & 2) / 2;
				uint i09 = (fi & 1) / 1;

				fi = c.w * 255;
				uint i08 = (fi & 128) / 128;
				uint i07 = (fi & 64) / 64;
				uint i06 = (fi & 32) / 32;
				uint i05 = (fi & 16) / 16;
				uint i04 = (fi & 8) / 8;
				uint i03 = (fi & 4) / 4;
				uint i02 = (fi & 2) / 2;
				uint i01 = (fi & 1) / 1;

				float _sign = 1.0;
				if (i32 == 1)
				{
					_sign = -1.0;
				}
				float _bias = 127.0;
				float _exponent = i24 * 1 + i25*2.0 + i26*4.0 + i27*8.0 + i28*16.0 + i29*32.0 + i30*64.0 + i31*128.0;
				float _mantisa = 1.0 + (i23 / 2.0) + (i22 / 4.0) + (i21 / 8.0) + (i20 / 16.0) + (i19 / 32.0) + (i18 / 64.0) + (i17 / 128.0) + (i16 / 256.0) + (i15 / 512.0) + (i14 / 1024.0) + (i13 / 2048.0) + (i12 / 4096.0) + (i11 / 8192.0) + (i10 / 16384.0) + (i09 / 32768.0) + (i08 / 65536.0) + (i07 / 131072.0) + (i06 / 262144.0) + (i05 / 524288.0) + (i04 / 1048576.0) + (i03 / 2097152.0) + (i02 / 4194304.0) + (i01 / 8388608.0);

				if (((_exponent == 255.0) || (_exponent == 0.0)) && (_mantisa == 0.0))
				{
					f = 0.0;
				}
				else
				{
					_exponent = _exponent - _bias;
					f = _sign * _mantisa * pow(2.0, _exponent);
				}

				return f;
			}
			// End wtf
			 
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
				float relativeX;
				float relativeY;
				float relativeZ;
				float distanceSquared;

				float fade1;

				float timeFade;
				float totalFade = 0.0;

				for (uint i = 0; i < 4; i++) {
					// Compute the distance squared of the hit to the current vertex
					relativeX = iv.worldPos.x - _ShieldHitBuffer[i][0];
					relativeY = iv.worldPos.y - _ShieldHitBuffer[i][1];
					relativeZ = iv.worldPos.z - _ShieldHitBuffer[i][2];

					distanceSquared = relativeX * relativeX + relativeY * relativeY + relativeZ * relativeZ;

					fade1 = max((20.0 - distanceSquared) / 40.0, 0.0);
					if ((1.0 - distanceSquared) > 0.0) fade1 = 1.0;

					half elapsedTime = _Time.y - _ShieldHitBuffer[i][3];
					timeFade = max((0.5 - elapsedTime) / 0.5, 0.0); // Not sure about this.

					totalFade = max(totalFade, fade1*timeFade);
				}
				
				//totalFade = min(totalFade, 1.0);
				
				//float4 col = _TintColor * tex2D(_PatternTex, i.texcoord);
				float4 col = float4(1.0, 1.0, 1.0, 1.0);
				//col = tex2D(_ShieldHitBufferTexture, float2(0, 0));
				col *= totalFade;

				col *= _TintColor * tex2D(_PatternTex, iv.texcoord);
				
				float4 newCol = tex2D(_LastFrame, half2(iv.texcoord1.x, iv.texcoord1.y)) * 0.9;
				col = max(col, newCol);
				
				return col;
			}
			ENDCG 
		}
	}	
}
}

