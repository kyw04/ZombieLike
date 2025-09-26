Shader "Custom/SlotMachineShader"
{
    Properties
    {
        _TexArray ("Texture Array", 2DArray) = "" {}
        _TexCount ("Texture Array Count", int) = 0
        _CenterIndex ("Center Index", Float) = 0   // float로 애니메이션
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
            float _CenterIndex;
            int _TexCount;

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

            fixed4 SetAlpha(float2 uv, int index)
            {
                if (uv.x >= 0.0 && uv.x <= 1.0 && uv.y >= 0.0 && uv.y <= 1.0)
                    return UNITY_SAMPLE_TEX2DARRAY(_TexArray, float3(uv, index));
                return fixed4(0, 0, 0, 0);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 인덱스와 진행 비율
                float baseIndex = floor(_CenterIndex);
                float t = frac(_CenterIndex);

                // 현재, 다음 인덱스
                int currIdx = (int)fmod(baseIndex, _TexCount);
                int nextIdx = (int)fmod(baseIndex + 1, _TexCount);

                // UV 이동: 현재는 내려가고, 다음은 아래에서 올라옴
                float2 uvCurr = i.uv - float2(0, -t);   // 위로 이동
                float2 uvNext = i.uv - float2(0, 1.0 - t); // 아래에서 올라옴

                fixed4 colCurr = SetAlpha(uvCurr, currIdx);
                fixed4 colNext = SetAlpha(uvNext, nextIdx);

                // 알파 블렌딩 (겹치는 부분만)
                return (colCurr.a > 0 ? colCurr : colNext);
            }
            ENDCG
        }
    }
}
