﻿Shader "Hidden/PostProcessingComposite" {
    Properties
    {
        _MainTex ("Texture", RECT) = "white" {}        
        _GlowTex ("Texture", RECT) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _GlowTex;
            float4 _MainTex_ST;
            float4 _BlurTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
               	fixed4 color = tex2D(_MainTex, i.uv);
				fixed4 glow = tex2D(_GlowTex, float2(i.uv.x, i.uv.y));
				
				color += glow;
				
				return color;
            }
            ENDCG
        }
    }
}
