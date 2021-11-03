Shader "MixedReality/Screen" // reference: http://tips.hecomi.com/entry/2016/07/26/013535
{
    Properties
    {
        _MainTex ("Color and Depth Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "DisableBatching" = "True" "Queue" = "Geometry+10" }
        Cull Off

        Pass
        {
            Tags { "LightMode" = "Deferred" }

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            struct GBufferOut
            {
                float4 diffuse  : SV_Target0; // rgb: diffuse,  a: occlusion
                float4 specular : SV_Target1; // rgb: specular, a: smoothness
                float4 normal   : SV_Target2; // rgb: normal,   a: unused
                float4 emission : SV_Target3; // rgb: emission, a: unused
                float depth    : SV_Depth;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float GetDepth(float2 uv)
            {
                uv.y *= -1.0; // flip y axis
                float d = tex2D(_MainTex, uv).a;
                return 1/(1-d);
            }

            float3 GetWorldCoord(float2 uv)
            {
                float fact = 0.57735026919; // tan(30.0 deg), fov ~= 60.0 (deg)
                float z = GetDepth(uv);
                float u = 2 * (uv.x - 0.5); // scale to -1 ~ 1
                float v = 2 * (uv.y - 0.5); // scale to -1 ~ 1
                float x = u * fact * z;
                float y = v * fact * z;

                return float3(x, y, z);
            }

            float3 GetNormal(float2 uv)
            {
                float2 uvX = uv - float2(1, 0);
                float2 uvY = uv - float2(0, 1);

                float3 pos0 = GetWorldCoord(uv);
                float3 posX = GetWorldCoord(uvX);
                float3 posY = GetWorldCoord(uvY);

                float3 dirX = normalize(posX - pos0);
                float3 dirY = normalize(posY - pos0);

                return 0.5 + 0.5 * cross(dirY, dirX);
            }

            float GetDepthForBuffer(float2 uv)
            {
                float4 vpPos = mul(UNITY_MATRIX_VP, float4(GetWorldCoord(uv), 1.0));
                return vpPos.z / vpPos.w;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            GBufferOut frag (v2f i)
            {
                GBufferOut o;
                o.diffuse = tex2D(_MainTex, i.uv);
                o.specular = float4(0.0, 0.0, 0.0, 0.0);
                o.emission = float4(0.0, 0.0, 0.0, 0.0);
                o.normal = float4(GetNormal(i.uv), 1.0);
                o.depth = GetDepthForBuffer(i.uv);
                return o;
            }
            ENDCG
        }
    }
}
