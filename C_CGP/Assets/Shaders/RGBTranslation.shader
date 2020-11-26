Shader "CGP/RGBTranslation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Rx("RVectorX", Float) = 0.0
        _Ry("RVectorY", Float) = 0.0
        _Gx("GVectorX", Float) = 0.0
        _Gy("GVectorY", Float) = 0.0
        _Bx("BVectorX", Float) = 0.0
        _By("BVectorY", Float) = 0.0
        _Multiplicator("Multiplicator", Range(0.0,1.0)) = 1.0
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

            float _Rx;
            float _Ry;
            float _Gx;
            float _Gy;
            float _Bx;
            float _By;
            float _Multiplicator;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 vR = { _Rx, _Ry};
                float2 vG = { _Gx, _Gy };
                float2 vB = { _Bx, _By };
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 colr = tex2D(_MainTex, i.uv - vR);
                colr.g = 0;
                colr.b = 0;
                fixed4 colg = tex2D(_MainTex, i.uv - vG);
                colg.r = 0;
                colg.b = 0;
                fixed4 colb = tex2D(_MainTex, i.uv - vB);
                colb.g = 0;
                colb.r = 0;
                col = col * (1.0f - _Multiplicator) + (colr + colg + colb)*_Multiplicator;
                return col;
            }
            ENDCG
        }
    }
}
