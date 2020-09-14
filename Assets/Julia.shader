Shader "Fractals/Julia"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area ("_Area", vector) = (0,0,4,4)
        _MaxIter ("_MaxIter", int) = 255
        _Speed ("_Speed", float) = 0.5
        _Repeat ("_Repeat", float) = 10
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
    
            float _MaxIter;
            float4 _Area;
            float _Speed;
            float _Repeat;
            
            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 _A = float4(0.5,0.5,0.5,1);
                float4 _B = float4(0.5,0.5,0.5,1);
                float4 _C = float4(1,1,1,1);
                float4 _D = float4(0,0.33,0.67,1);  


                float2 z = _Area.zw*(i.uv-0.5)+_Area.xy;


                float2 c = 0.7885*float2(cos(_Time.y*0.5),sin(_Time.y*0.5));

 
                float r = 20;
                float iter;

                for (iter = 0; iter < _MaxIter; iter++) {
                    z = float2(z.x*z.x - z.y*z.y, 2*z.x*z.y) + c;
                    if (length(z) > r) break;
                }

                //interpolation 
                float dist = length(z);
                float interp = log2(log(dist)/log(r));
                iter -= interp;

                //calculate color
                float iterRatio = sqrt(iter/_MaxIter);
                float gradientPos = iterRatio * _Repeat +  _Time.y * _Speed;
                float4 col = _A + _B*sin(6.28318*(_C*gradientPos+_D));

                if (iter >= _MaxIter) return 0; //black
                return col;
            }
            ENDCG
        }
    }
}
