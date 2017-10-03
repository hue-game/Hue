Shader "Effects/SwitchShockwave"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_NoiseTex("Texture (R,G=X,Y Distortion; B=Mask; A=Unused)", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0.1
		_Color("Tint", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent" 
		}

		GrabPass
		{
			"_BackgroundTexture"
		}

		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _BackgroundTexture;
			sampler2D _NoiseTex;
			float _Intensity;
			float4 _NoiseTex_ST;

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.pos);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 d = tex2D(_NoiseTex, i.grabPos);
				float4 p = i.grabPos + (d * _Intensity);
				float4 bgcolor = tex2Dproj(_BackgroundTexture, p);
		
				return bgcolor;
			}

			ENDCG
		}
	}
}