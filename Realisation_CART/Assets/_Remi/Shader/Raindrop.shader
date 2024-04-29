// This shader is converted from 
// Heartfelt(https://www.shadertoy.com/view/ltffzl) - by Martijn Steinrucken aka BigWings - 2017
// countfrolic@gmail.com
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

Shader "Custom/Raindrop" {
	Properties {
		iChannel0("Albedo (RGB)", 2D) = "white" {}
		_PlayerSpeed("Player Speed", Float) = 0.0
		_PlayerRotationPull("Player Rotation Pull", Float) = 0.0
		_RainAmount("RainAmount", Float) = 0.0
		_IsActive("Is Active", Float) = 0.0
		//_PlayerGForce("Player GForce", Vector) = (0, 0, 0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D iChannel0;
			float _PlayerSpeed;
			float _PlayerRotationPull;
			float _RainAmount;
			int _IsActive;
			//Vector _PlayerGForce;

			//#define S(a, b, t) smoothstep(a, b, t)

			float3 N13(float p) 
			{
				float3 p3 = frac(float3(p, p, p) * float3(.1031, .11369, .13787));
				p3 += dot(p3, p3.yzx + 19.19);
				return frac(float3((p3.x + p3.y)*p3.z, (p3.x + p3.z)*p3.y, (p3.y + p3.z)*p3.x));
			}

			float4 N14(float t) 
			{
				return frac(sin(t*float4(123., 1024., 1456., 264.))*float4(6547., 345., 8799., 1564.));
			}

			float RandomFraction(float t) 
			{
				return frac(sin(t*12345.564)*7658.76);
			}

			float Saw(float b, float t) 
			{
				return smoothstep(0., b, t) * smoothstep(1., b, t);
			}

			float2 DropLayer2(float2 uv, float t) 
			{
				float2 UV = uv;
				uv.y += t*0.75;
				//uv.y += _PlayerSpeed * 0.1;
				uv.x += _PlayerRotationPull.x * 0.01;

				//uv.x += abs(_PlayerGForce.z) * 0.01;

				float2 row = float2(6.0, 1.0);
				float2 grid = row * 2.0;
				float2 id = floor(uv*grid);

				float UVColumnShift = RandomFraction(id.x);
				uv.y += UVColumnShift;

				id = floor(uv*grid);
				float3 gridNoise = N13(id.x*35.2 + id.y*2376.1);
				float2 sampleOffset = frac(uv*grid) - float2(.5, 0);

				float x = gridNoise.x - 0.5;

				float y = UV.y*20.;
				float wiggle = sin(y + sin(y));
				x += wiggle*(.5 - abs(x))*(gridNoise.z - .5);
				x *= .7;
				float ti = frac(t + gridNoise.z);
				y = (Saw(.85, ti) - .5)*.9 + .5;
				float2 p = float2(x, y);

				float d = length((sampleOffset - p)*row.yx);

				float mainDrop = smoothstep(.4, .0, d);

				float r = sqrt(smoothstep(1., y, sampleOffset.y));
				float cd = abs(sampleOffset.x - x);
				float trail = smoothstep(.23*r, .15*r*r, cd);
				float trailFront = smoothstep(-.02, .02, sampleOffset.y - y);
				trail *= trailFront*r*r;

				y = UV.y;
				float trail2 = smoothstep(.2*r, .0, cd);
				float droplets = max(0., (sin(y*(1. - y)*120.) - sampleOffset.y)) 
									* trail2 * trailFront * gridNoise.z;

				y = frac(y*10.) + (sampleOffset.y - .5);
				float dd = length(sampleOffset - float2(x, y));
				droplets = smoothstep(.3, 0., dd);
				float m = mainDrop + droplets*r*trailFront;

				return float2(m, trail);
			}

			float StaticDrops(float2 uv, float t) {
				uv *= 40.;

				float2 id = floor(uv);
				uv = frac(uv) - .5;
				float3 n = N13(id.x*107.45 + id.y*3543.654);
				float2 p = (n.xy - .5)*.7;
				float d = length(uv - p);

				float fade = Saw(.025, frac(t   + n.z));
				float c = smoothstep(.3, 0., d) * frac(n.z*10.) * fade;
				return c;
			}

			float2 Drops(float2 uv, float time, float slowDrop, float layer1, float layer2) {
				float staticDropping = StaticDrops(uv, time) * slowDrop;
				float2 layer1Dropping = DropLayer2(uv, time) * layer1;
				float2 layer2Dropping = DropLayer2(uv * 1.85, time) * layer2;

				float allDrops = staticDropping + layer1Dropping.x + layer2Dropping.x;
				allDrops = smoothstep(.3, 1., allDrops);

				return float2(allDrops, max(layer1Dropping.y * slowDrop, layer2Dropping.y * layer1));
			}


			fixed4 frag(v2f_img i) : SV_Target
			{
				if (_IsActive == 0.0f) 
				{
					return tex2D(iChannel0, i.uv);
				}
				
				float2 uv = ((i.uv * _ScreenParams.xy) - .5*_ScreenParams.xy) / _ScreenParams.y;
				float2 UV = i.uv.xy;
				//float3 M = float3(0.0, 0.0, 0.0);
				//float Duration = _Time.y + M.x*2.;
				//float Duration = _Time.y;
				float Duration = _Time.y + _PlayerSpeed;

				float DurationSlown = Duration * 0.2f;

				//float rainAmount = 100.0f;
				//float rainAmount = 10.0f - (_PlayerSpeed * 50.0f);
				float rainAmount = _RainAmount;

				float maxBlur = lerp(3., 6., rainAmount);
				float minBlur = 2.;

				float staticDrops = smoothstep(-.5, 1., rainAmount)*2.;
				float layer1 = smoothstep(.25, .75, rainAmount);
				float layer2 = smoothstep(.0, .5, rainAmount);

				float2 dropsXY = Drops(uv, DurationSlown, staticDrops, layer1, layer2);

				float2 UVShift = float2(0.001, 0.0);
				float dropsX = Drops(uv + UVShift, DurationSlown, staticDrops, layer1, layer2).x;
				float dropsYX = Drops(uv + UVShift.yx, DurationSlown, staticDrops, layer1, layer2).x;
				float2 normals = float2(dropsX - dropsXY.x, dropsYX - dropsXY.x);		

				float focus = lerp(maxBlur - dropsXY.y, minBlur, smoothstep(.1, .2, dropsXY.x));

				float4 texCoord = float4(UV.x + normals.x, UV.y + normals.y, 0, focus);
				float4 lod = tex2Dlod(iChannel0, texCoord);
				float3 col = lod.rgb;

				return fixed4(col, 1);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}