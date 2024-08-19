Shader "Unlit/WhiteBlinkSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlinkColor("Blink Color", Color) = (1,1,1,1)
        _BlinkValue("Value", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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
                fixed4 vcol : COLOR0;
            };

            struct v2f
            {
                fixed4 vcol : COLOR0;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _BlinkColor;
            fixed _BlinkValue;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vcol = v.vcol;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 fin = col * i.vcol;
                fin.rgb = lerp(fin, _BlinkColor, _BlinkValue);
                return fin;
            }
            ENDCG
        }
    }
}
