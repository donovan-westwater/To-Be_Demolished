Shader "Hidden/FogEffect"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _EnableFog("Enable Fog",int) = 0
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
            #pragma multi_compile_fog

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

            sampler2D _MainTex , _CameraDepthTexture;
            int _EnableFog;
            fixed4 frag (v2f i) : SV_Target
            {
               float3 sourceColor = tex2D(_MainTex, i.uv).rgb;
                // just invert the colors
            if (_EnableFog) {
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                depth *= 10;
                depth = saturate(depth);
                //return float4(depth, depth, depth, 1);
                return float4(sourceColor, 1)*(depth) + float4(.5,.5,.5,0)*(1-depth);
            }
                return float4(sourceColor.rgb,1);
            }
            ENDCG
        }
    }
}
