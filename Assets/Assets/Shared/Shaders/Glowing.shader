Shader "Custom/Glowing" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_EmissionAmount ("Emission Amount", Float) = 1

	}
	SubShader {
		Tags { "Queue"="Transparent" }
		LOD 200
		Cull Off

		CGPROGRAM

		#pragma surface surf Standard alpha:blend

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
	
		half _EmissionAmount;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Emission = c.rgb * _EmissionAmount;
			// Metallic and smoothness come from slider variables
		
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
