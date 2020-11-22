Shader "Hidden/Dithering"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitheringTexture("Dithering Texture", 2D) = "white" {}
        _DitheringScale("DitheringScale", Int) = 32 
        _DitheringStrength("DitheringStrength", Float) = 4.0 
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
            sampler2D _DitheringTexture;
            int _DitheringScale;
            float _DitheringStrength;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //if (col.r < 0.5 && col.g < 0.5 && col.b < 0.5)
                //{
                    col.rgb = col.rgb - tex2D(_DitheringTexture, i.uv*_DitheringScale)*_DitheringStrength;
                //}
                
                return col;
            }
            ENDCG
        }
    }
}
