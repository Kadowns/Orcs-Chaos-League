// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Ground" {
	Properties {
	    _MainTex("Texture", 2D) = "white"{}
		_ColdColor ("Cold Color", Color) = (1,1,1,1)
		_HotColor ("Hot Color", Color) = (1,0,0,1)
		_Blend("Blend", Range(1, 16)) = 2
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FlashAmount("Flash Amount", Range(0, 1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0       

		struct Input {
			float2 uv_MainTex;
			float yPosition;
		};

        sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		half _Blend;
		half _FlashAmount;
		fixed4 _ColdColor;
		fixed4 _HotColor;
		
		void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.yPosition = mul (unity_ObjectToWorld, v.vertex).y;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed weight = clamp(IN.yPosition, 0 , _Blend) / _Blend;
			o.Albedo = _ColdColor * c.rgb;
			o.Emission = _HotColor * c.rgb * (1 - weight) * 2;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = lerp(o.Emission, fixed3(1.0, 1.0, 1.0), _FlashAmount);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
