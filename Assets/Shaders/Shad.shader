Shader "Unlit/Shad" // reference: http://tips.hecomi.com/entry/2016/07/26/013535
{
    Properties
    {
        _MainTex ("Color and Depth Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque"}

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
				float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

			struct outdata
			{
				fixed4 color : SV_Target0;
				float depth : SV_Depth;
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

			float GetDepthForBuffer(float2 uv)
			{
				float4 vpPos = mul(UNITY_MATRIX_VP, float4(GetWorldCoord(uv), 1.0));
				return vpPos.z / vpPos.w;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			outdata frag (v2f i)
            {
				outdata o;
                o.color = tex2D(_MainTex, i.uv);
				o.depth = GetDepthForBuffer(i.uv);
                return o;
            }
            ENDCG
        }
    }
}
