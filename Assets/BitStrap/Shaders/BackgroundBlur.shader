Shader "BitStrap/UI/BackgroundBlur"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_BlurAmount("Blur Amount", Range(0,10)) = 0.0075
		_DesaturationAmount("Desaturation Amount", Range(0,1)) = 0

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
	}

	SubShader
	{
		GrabPass { }

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		// Horizontal blur pass
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Name "HorizontalBlur"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture : register(s0);
			uniform half4 _GrabTexture_TexelSize;
			half _BlurAmount;
			half _DesaturationAmount;

			struct v2f
			{
				float4  pos : SV_POSITION;
				float4  color : COLOR;
				float2  uv : TEXCOORD0;
			};

			float4 _GrabTexture_ST;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 uv = (o.pos.xy + float2(1.0, 1.0))*0.5;
				o.uv = TRANSFORM_TEX(uv, _GrabTexture);
				o.uv.y = 1 - o.uv.y;
				o.color = v.color;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float blurAmount = _BlurAmount * _GrabTexture_TexelSize.x;

				half4 sum = half4(0.0, 0.0, 0.0, 0.0);

				sum += tex2D(_GrabTexture, float2(i.uv.x - 5.0 * blurAmount, i.uv.y)) * 0.025;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 4.0 * blurAmount, i.uv.y)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 3.0 * blurAmount, i.uv.y)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 2.0 * blurAmount, i.uv.y)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x - blurAmount, i.uv.y)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y)) * 0.16;
				sum += tex2D(_GrabTexture, float2(i.uv.x + blurAmount, i.uv.y)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 2.0 * blurAmount, i.uv.y)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 3.0 * blurAmount, i.uv.y)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 4.0 * blurAmount, i.uv.y)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 5.0 * blurAmount, i.uv.y)) * 0.025;

				half gray = (sum.r + sum.g + sum.b) * 0.33;
				sum = lerp(sum, gray, _DesaturationAmount);

				sum.a = i.color.a;

				return sum;
			}
			ENDCG
		}

		GrabPass{}

		// Vertical blur pass
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Name "VerticalBlur"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture : register(s0);
			uniform half4 _GrabTexture_TexelSize;
			half _BlurAmount;

			struct v2f
			{
				float4  pos : SV_POSITION;
				float4  color : COLOR;
				float2  uv : TEXCOORD0;
			};

			float4 _GrabTexture_ST;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 uv = (o.pos.xy + float2(1.0, 1.0))*0.5;
				o.uv = TRANSFORM_TEX(uv, _GrabTexture);
				o.uv.y = 1 - o.uv.y;
				o.color = v.color;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float blurAmount = _BlurAmount * _GrabTexture_TexelSize.y;

				half4 sum = half4(0.0, 0.0, 0.0, 0.0);

				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 5.0 * blurAmount)) * 0.025;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 4.0 * blurAmount)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 3.0 * blurAmount)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 2.0 * blurAmount)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - blurAmount)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y)) * 0.16;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + blurAmount)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 2.0 * blurAmount)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 3.0 * blurAmount)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 4.0 * blurAmount)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 5.0 * blurAmount)) * 0.025;

				sum.a = 1;
				sum *= i.color;

				return sum;
			}
			ENDCG
		}
	}

	CustomEditor "BackgroundBlurEditor"
}
