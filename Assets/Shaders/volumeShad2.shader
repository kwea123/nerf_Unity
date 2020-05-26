Shader "Unlit/volumeShad2"  // ref https://github.com/mattatz/unity-volume-rendering/blob/master/Assets/VolumeRendering/Shaders/VolumeRendering.cginc
{
	Properties
	{
		[Header(Rendering)]
		_Volume("Volume", 3D) = "" {}
		_Iteration("Iteration", Int) = 10
		[MaterialToggle] _Dissolve("Dissolve", Float) = 0

		[Header(Ranges)]
		_MinX("MinX", Range(0, 1)) = 0.0
		_MaxX("MaxX", Range(0, 1)) = 1.0
		_MinY("MinY", Range(0, 1)) = 0.0
		_MaxY("MaxY", Range(0, 1)) = 1.0
		_MinZ("MinZ", Range(0, 1)) = 0.0
		_MaxZ("MaxZ", Range(0, 1)) = 1.0
	}
	SubShader
	{
		Tags
		{ "Queue" = "Transparent"
		  "RenderType" = "Transparent"
		}
		Cull Front
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 localPos : TEXCOORD0;
			};

			sampler3D _Volume;
			int _Iteration;
			fixed _MinX, _MaxX, _MinY, _MaxY, _MinZ, _MaxZ;
			fixed _Dissolve;

			float4 sample(float3 pos) // clip the volume
			{
				fixed x = step(pos.x, _MaxX) * step(_MinX, pos.x);
				fixed y = step(pos.y, _MaxY) * step(_MinY, pos.y);
				fixed z = step(pos.z, _MaxZ) * step(_MinZ, pos.z);
				return tex3D(_Volume, pos) * x * y * z;
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.localPos = v.vertex;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 rayOrigin = i.localPos + 0.5;
				float3 rayDir = ObjSpaceViewDir(i.localPos);
				float rayLength = length(rayDir);
				rayDir = normalize(rayDir);

				float4 finalColor = 0.0;
				float t = 1.732 / _Iteration; // step size for one iteration

				[loop]
				for (int j = 0; j < _Iteration; ++j)
				{
					float step = t * j;
					if (step > rayLength) // do not render volume that is behind the camera
						break;
					float3 curPos = rayOrigin;
					if (_Dissolve) {
						step *= (1 + sin(_Time.z / 2))*0.5;
					}
					curPos += rayDir * step;
					float4 color = sample(curPos);
					// use back to front composition
					finalColor.rgb = color.a * color.rgb + (1-color.a) * finalColor.rgb;
					finalColor.a = color.a + (1 - color.a) * finalColor.a;
					if (finalColor.a > 1) break;
				}
				return finalColor;

			}
			ENDCG
		}
	}
}
