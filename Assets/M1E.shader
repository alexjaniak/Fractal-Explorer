Shader "Fractals/M1E"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area ("_Area", vector) = (0,0,4,4)
        _MaxIter ("_MaxIter", int) = 255
        _Speed ("_Speed", float) = 1
        _Repeat ("_Repeat", float) = 1

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
                float2 uv : TEXCOORD0;s
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
            float _MaxIter;
            float _Speed;
            float _Repeat;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 a = float4(0.5,0.5,0.5,1);
                float4 b = float4(0.5,0.5,0.5,1);
                float4 c1 = float4(1,1,1,1);
                float4 d = float4(0,0.33,0.67,1);  


                float2 c = _Area.zw*(i.uv-0.5)+_Area.xy;
                float2 z;
                float iter;
                float r = 20;

                for (iter = 0; iter < _MaxIter; iter++) {
                    z = float2(z.x*z.x - z.y*z.y, 2*z.x*z.y) + c;
                    if (length(z) > r) break;
                }

                if (iter >= _MaxIter) return 0; //black
                
                //interpolation
                float dist = length(z);
                float interp = log2(log(dist)/log(r));
                iter -= interp;

                //calculate color
                float iterRatio = sqrt(iter/_MaxIter);
                float gradientPos = iterRatio * _Repeat +  _Time.y * _Speed;
                float4 col = a + b*sin(6.28318*(c1*gradientPos+d));

                return 0;
            }
            ENDCG
        }
    }
}
