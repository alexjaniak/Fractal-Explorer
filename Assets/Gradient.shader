Shader "Fractals/Gradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _A ("_A", vector) = (0.5,0.5,0.5,1) //Offset
        _B ("_B", vector) = (0.5,0.5,0.5,1) //Amplitude
        _C ("_C", vector) = (1,1,1,1) //Frequency
        _D ("_D", vector) = (0,0.33,0.67,1) //Phase
        //http://dev.thi.ng/gradients/
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
            float4 _A;
            float4 _B;
            float4 _C;
            float4 _D;

            fixed4 frag (v2f i) : SV_Target
            {
                return _A + _B*sin(6.28318*(_C*i.uv.x+_D));
            }
            ENDCG
        }
    }
}
