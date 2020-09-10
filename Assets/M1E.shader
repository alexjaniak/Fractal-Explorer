Shader "Fractals/M1E"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area ("_Area", vector) = (0,0,4,4)
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
            float4 _Area;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 c = _Area.zw*(i.uv-0.5)+_Area.xy;
                float2 z;
                float max_iter = 255;
                float iter;

                for (iter = 0; iter < max_iter; iter++) {
                    z = float2(z.x*z.x - z.y*z.y, 2*z.x*z.y) + c;
                    if (length(z) > 2) break;
                }

                return sqrt(iter/max_iter);
            }
            ENDCG
        }
    }
}
