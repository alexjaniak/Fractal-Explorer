Shader "Fractals/CosineGraph"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Area("_Area", vector) = (0.5,0.5,1,1)
        _A("_A", vector) = (0.5,0.5,0.5,1) //Offset
        _B("_B", vector) = (0.5,0.5,0.5,1) //Amplitude
        _C("_C", vector) = (1,1,1,1)       //Frequency
        _D("_D", vector) = (0,0.33,0.67,1) //Phase
        e("_LineThickness",float) = 0.005
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
            float4 _A;
            float4 _B;
            float4 _C;
            float4 _D;
            float e; //line thickness

            float ColorFunction(float x, float a, float b, float c, float d) {
                return b*cos(6.28318*(c*x+d)) + a;
            }

            float derColorFunction(float x, float a, float b, float c, float d) {
                return -c * b * 6.2318 * sin(6.28318 * (c * x + d));
            }

            bool GraphFunction(float x, float y, int z) {
                float r = ColorFunction(x, _A[z], _B[z], _C[z], _D[z]);
                float dr = derColorFunction(x, _A[z], _B[z], _C[z], _D[z]);
                float rtheta = atan(-1 / dr);
                float rx = e * cos(rtheta);
                float ry = e * sin(rtheta);
                float r1 = ColorFunction(x - rx, _A[z], _B[z], _C[z], _D[z]) + ry;
                float r2 = ColorFunction(x + rx, _A[z], _B[z], _C[z], _D[z]) - ry;

                if (dr < 0) {
                    if (r2 > 1 - 2 * e) r2 = 1 - 2 * e; //lower bound for function above 1
                    if (r1 < 2 * e) r1 = 2 * e; //upper bound for function below 0
                    if (y < r1 && y > r2) return true;
                }
                if (dr > 0) {
                    if (r1 > 1 - 2 * e) r1 = 1 - 2 * e; //lower bound for function above 1
                    if (r2 < 2 * e) r2 = 2 * e; //upper bound for functioin below 0
                    if (dr > 0) if (y > r1 && y < r2) return true;
                }
                return false;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = _Area.zw * (i.uv - 0.5) + _Area.xy;
                if (GraphFunction(p.x,p.y, 0)) return float4(1,0,0,1); //red
                if (GraphFunction(p.x,p.y, 1)) return float4(0,1,0,1); //green
                if (GraphFunction(p.x,p.y, 2)) return float4(0, 0, 1, 1); //blue
                return float4(1, 1, 1, 1); //backround
            }
            ENDCG
        }
    }
}
