Shader "Unlit/ColorBlendUV"
{
    Properties
    {
       _Color0 ("Color", Color) = (0,0,0,1)
       _Color1 ("Color1", Color) = (1,1,1,1)
       _Scale("Scale", Float) = 1
       _Offset("Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
          //  #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float uv : TEXCOORD0;
          //      UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color0, _Color1;
            float _Scale, _Offset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.y;
            //    UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = lerp(_Color0, _Color1, saturate(i.uv * _Scale + _Offset));
                // apply fog
             //   UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
