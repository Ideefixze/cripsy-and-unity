﻿Shader "CGP/Palette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PaletteSize ("PaletteSize", Int) = 4
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _PaletteSize;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                col.r = (float)((int)(col.r * _PaletteSize) % (_PaletteSize+1)) / (float)(_PaletteSize);
                col.g = (float)((int)(col.g * _PaletteSize) % (_PaletteSize+1)) / (float)(_PaletteSize);
                col.b = (float)((int)(col.b * _PaletteSize) % (_PaletteSize+1)) / (float)(_PaletteSize);
                return col;
            }
            ENDCG
        }
    }
}
