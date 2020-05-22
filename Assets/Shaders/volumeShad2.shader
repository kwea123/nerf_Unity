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
		Cull Back
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha
		//Lighting Off

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

			struct Ray {
				float3 origin;
				float3 dir;
			};

			struct AABB { // axis aligned bounding box
				float3 min;
				float3 max;
			};

			sampler3D _Volume;
			int _Iteration;
			fixed _MinX, _MaxX, _MinY, _MaxY, _MinZ, _MaxZ;
			fixed _Dissolve;

			bool intersect(Ray r, AABB aabb, out float t0, out float t1)
			{
				float3 invR = 1.0 / r.dir;
				float3 tbot = invR * (aabb.min - r.origin);
				float3 ttop = invR * (aabb.max - r.origin);
				float3 tmin = min(ttop, tbot);
				float3 tmax = max(ttop, tbot);
				float2 t = max(tmin.xx, tmin.yz);
				t0 = max(t.x, t.y);
				t = min(tmax.xx, tmax.yz);
				t1 = min(t.x, t.y);
				return t0 <= t1;
			}

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
				Ray ray;
				ray.origin = i.localPos;
				ray.dir = -normalize(ObjSpaceViewDir(i.localPos)); // camera to object

				AABB aabb;
				aabb.min = float3(-0.5, -0.5, -0.5);
				aabb.max = float3(0.5, 0.5, 0.5);

				float near, far;
				intersect(ray, aabb, near, far); // find intersection near and far
				near = max(0.0, near);

				float3 curPos = ray.origin; // start from origin, then move forward
				float3 step = ray.dir * (far - near) / _Iteration;
				float4 finalColor = 0.0;

				[loop]
				for (int j = 0; j < _Iteration; ++j)
				{
					float4 color = sample(curPos + 0.5);
					// use front to back blending
					finalColor.rgb += (1 - finalColor.a) * color.a * color.rgb;
					finalColor.a += (1 - finalColor.a) * color.a;
					if (_Dissolve) {
						curPos += step * (1 + sin(_Time.z / 2))*0.5;
					}
					else {
						curPos += step; // move the ray forward
					}
					if (finalColor.a > 0.999) break;
				}
				return finalColor;
			}
			ENDCG
		}
	}
}
