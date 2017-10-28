// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BitStrap/Scanline" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1.0, 0.0, 0.0, 1.0)

		_ScanlineColor("Scanline Color", Color) = (0.7, 0.7, 1.0, 1.0)
		_ScanlineDelay("Scanline Delay", Range(1.0, 5.0)) = 1.5
		_ScanlineHeight("Scanline Height", Range(0.0, 1.0)) = 0.0
		_ScanlinePower("Scanline Power", Range(1.0, 10.0)) = 1.0
		_ScanlineVelocity("Scanline Velocity", Range(-50.0, 50.0)) = 1.0

		_SubScanlineColor("SubScanline Color", Color) = (0.7, 0.7, 1.0, 1.0)
		_SubScanlineFrequency("SubScanline Frequency", Range(0.0, 50.0)) = 10.0
		_SubScanlineHeight("SubScanline Height", Range(0.0, 1.0)) = 0.0
		_SubScanlinePower("SubScanline Power", Range(1.0, 10.0)) = 1.0
		_SubScanlineVelocity("SubScanline Velocity", Range(-50.0, 50.0)) = 1.0
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct v2f
		{
			half4 pos : POSITION;
			half2 uv : TEXCOORD;
		};

		struct appdata_outline
		{
			half4 vertex : POSITION;
			half2 texcoord : TEXCOORD;
		};

		sampler2D _MainTex;
		half4 _Color;

		half4 _ScanlineColor;
		half _ScanlineDelay;
		half _ScanlineHeight;
		half _ScanlinePower;
		half _ScanlineVelocity;

		half4 _SubScanlineColor;
		half _SubScanlineFrequency;
		half _SubScanlineHeight;
		half _SubScanlinePower;
		half _SubScanlineVelocity;

		v2f vert(appdata_outline v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;

			return o;
		}

		fixed4 frag(v2f pixelData) : COLOR
		{
			half h = fmod(pixelData.uv.y + _Time.x * _ScanlineVelocity, _ScanlineDelay);
			h = frac(fmod(clamp(h, -1.0, 1.0), 1.0));

			half sub_h = frac(pixelData.uv.y * _SubScanlineFrequency + _Time.x * _SubScanlineVelocity);

			fixed t = (h - (1.0 - _ScanlineHeight)) / _ScanlineHeight;
			t = sign(t) * pow(t, _ScanlinePower);
			t = saturate(t - (1.0 - _ScanlineColor.a));

			fixed sub_t = (sub_h - (1.0 - _SubScanlineHeight)) / _SubScanlineHeight;
			sub_t = sign(sub_t) * pow(sub_t, _SubScanlinePower);
			sub_t = saturate(sub_t - (1.0 - _SubScanlineColor.a));

			fixed4 textureColor = tex2D(_MainTex, pixelData.uv) * _Color;
			fixed4 c = lerp(textureColor, _ScanlineColor, t);
			fixed4 sub_c = lerp(textureColor, _SubScanlineColor, sub_t);

			c = (c + sub_c) * 0.5;
			c.a = textureColor.a * _Color.a;

			return c;
		}
			ENDCG

			Subshader {
			Tags{ "Queue" = "Transparent" }
				Pass{
					ZWrite Off
					Cull Off
					Fog { Mode off }
					ColorMask RGB
					Blend SrcAlpha OneMinusSrcAlpha

					CGPROGRAM
					#pragma fragmentoption ARB_precision_hint_fastest
					#pragma vertex vert
					#pragma fragment frag
					ENDCG
			}
		}

		Fallback off
}
