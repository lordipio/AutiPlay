Shader "Custom/CircleTransition"
{
    Properties
    {
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Range(0,1)) = 0.3
        _Aspect ("Aspect Ratio", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            float4 _Center;
            float _Radius;
            float _Aspect;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float2 uv = i.uv;

                // اصلاح فاصله گرفتن طبق نسبت عرض/ارتفاع
                float2 diff = uv - center;
                diff.x *= _Aspect; // فقط X را با نسبت ضرب میکنیم

                float dist = length(diff);

                if (dist < _Radius)
                {
                    return fixed4(0, 0, 0, 0);
                }
                else
                {
                    return fixed4(0, 0, 0, 1);
                }
            }
            ENDCG
        }
    }
}
