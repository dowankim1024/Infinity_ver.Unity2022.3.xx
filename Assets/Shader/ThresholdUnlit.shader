Shader "Unlit/ThresholdMask"
{
    Properties{
        _MainTex("Source", 2D) = "white" {}
        _Threshold("Threshold", Range(0,1)) = 0.5
        _FlipX("Flip X", Float) = 0
    }
    SubShader{
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            float _Threshold;
            float _FlipX;

            struct appdata { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert(appdata v){
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                if (_FlipX > 0.5) o.uv.x = 1.0 - o.uv.x;
                return o;
            }

            half4 frag(v2f i):SV_Target{
                float3 rgb = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb;
                float luma = dot(rgb, float3(0.299, 0.587, 0.114)); // 밝기
                float m = luma > _Threshold ? 1.0 : 0.0;           // 흑/백
                return half4(m, m, m, 1.0);
            }
            ENDHLSL
        }
    }
}
