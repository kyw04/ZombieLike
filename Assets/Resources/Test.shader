Shader "Custom/Test"
{
    Properties
    {
        _TexArray ("Texture Array", 2DArray) = "" {}
        _CenterIndex ("Center Index", Int) = 0
        _AroundIndex ("Around Index", Int) = 1
        _Rotation ("Rotation (Degrees)", Float) = 0
        _CurveAmountY ("Vertical Curve", Float) = 0.3
        _CurveAmountX ("Horizontal Curve", Float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            UNITY_DECLARE_TEX2DARRAY(_TexArray);
            int _CenterIndex;
            int _AroundIndex;
            float _Rotation;
            float _CurveAmountY;
            float _CurveAmountX;

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

            fixed4 frag (v2f i) : SV_Target
            {
                // 중심 텍스처 UV 축소
                float2 center = float2(0.5, 0.5);
                float2 uvCenter = (i.uv - center) * 1.5 + center; // 0.7은 축소 비율 (값이 작을수록 더 작게)

                fixed4 colCenter = UNITY_SAMPLE_TEX2DARRAY(_TexArray, float3(uvCenter, _CenterIndex));

                fixed4 colAround = UNITY_SAMPLE_TEX2DARRAY(_TexArray, float3(uvCenter, _AroundIndex));
                // 주변이 위에 오도록 알파 블렌딩
                fixed4 finalCol = lerp(colCenter, colAround, colAround.a);

                return finalCol;
            }

            ENDCG
        }
    }
}
