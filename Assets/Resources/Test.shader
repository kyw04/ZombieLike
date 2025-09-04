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
                // 중심 텍스처
                fixed4 colCenter = UNITY_SAMPLE_TEX2DARRAY(_TexArray, float3(i.uv, _CenterIndex));

                // 회전 변환
                float rad = radians(_Rotation);
                float2 center = float2(0.5, 0.5);
                float2 uvRot = i.uv - center;
                float cosR = cos(rad);
                float sinR = sin(rad);
                uvRot = float2(
                    uvRot.x * cosR - uvRot.y * sinR,
                    uvRot.x * sinR + uvRot.y * cosR
                ) + center;

                // 위아래 곡률 (세로 방향 휘어짐)
                float curveY = sin((uvRot.y - 0.5) * 3.14159) * _CurveAmountY;
                uvRot.x += curveY;

                // 좌우 곡률 (가로 방향 휘어짐)
                float curveX = sin((uvRot.x - 0.5) * 3.14159) * _CurveAmountX;
                uvRot.y += curveX;

                // 주변 텍스처 (곡률+회전 적용)
                fixed4 colAround = UNITY_SAMPLE_TEX2DARRAY(_TexArray, float3(uvRot, _AroundIndex));

                // 주변이 위에 오도록 알파 블렌딩
                fixed4 finalCol = lerp(colCenter, colAround, colAround.a);

                return finalCol;
            }
            ENDCG
        }
    }
}
