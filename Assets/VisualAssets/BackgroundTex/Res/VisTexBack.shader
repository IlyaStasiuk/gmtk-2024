Shader "Unlit/VisTexBack"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale("X World Scale", Float) = 1.0
        _Speed("Speed", Range(-3,3)) = 0.2
        _Slide("Speed", Range(-3,3)) = 0.2
        _ColorMul ("Color Mul", Color) = (1,1,1,1)
        _ColorAdd ("Color Add", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
     //       #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
           //     UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed _Speed, _Scale, _Slide;
            fixed4 _ColorAdd, _ColorMul;
          //  float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                float wPos = mul(UNITY_MATRIX_M, v.vertex).x * _Scale;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = float2(wPos + (_Time.y * _Speed) + wPos * _Slide, v.uv.y);
          //      UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, float2(frac(i.uv.x ),i.uv.y)) * _ColorMul + _ColorAdd;
                // apply fog
            //    UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
