Shader "Custom/DepthCameraShader"
{
    Properties
    {
        _Near("Near Clipping Plane", Float) = 0.3
        _Far("Far Clipping Plane", Float) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _CameraDepthTexture;
            float _Near;
            float _Far;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.pos);
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                float rawDepth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r;

                // Convert to linear depth (0 to 1)
                float linearDepth = Linear01Depth(rawDepth);

                // Remap to custom near/far range
                float worldDepth = linearDepth * (_Far - _Near) + _Near;

                // Normalize and invert: near = white, far = black
                float normalized = saturate(1.0 - ((worldDepth - _Near) / (_Far - _Near)));

                return half4(normalized, normalized, normalized, 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}

