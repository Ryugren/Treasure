Shader "Custom/RandomNoise"
{
	Properties
	{
		_MainTex("Albedo(RGB)", 2D) = "white"{}
		_Alpha("Alpha", float) = 1
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0
		sampler2D _MainTex;
		float _Alpha;

		struct Input
		{
			float2 uv_MainTex;
		};

		float random(fixed2 p)
		{
			return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
		}


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed2 uv = IN.uv_MainTex;
			uv.x += 0.1 * _Time;
			uv.y += 0.2 * _Time;
			float c = random(uv);
			o.Albedo = fixed4(c, c, c, 1);
			o.Alpha = _Alpha;
		}
		ENDCG
	}
			FallBack "Diffuse"
}