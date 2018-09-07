Shader "Custom/Monitor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DistortionTex ("Distortion Noise", 2D) = "white" {}
		_DistortionAmount ("DistortionAmount", Range(0, 2)) = 1
		_XOffSet("X OffSet", float) = 0
		_YOffSet("Y OffSet", float) = 0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionAmount ("Emission Amount", float) = 1
		_Flickering("Flickering Amount", float) = 0
		yScrollSpeed("Scroll Speed Y", Range(-5, 5)) = 0
		xScrollSpeed("Scroll Speed X", Range(-5, 5)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		struct Input {
			float2 uv_MainTex;
			float2 uv_DistortionTex;
		    float4 pixelCoord : SV_POSITION;
		};
		

        fixed yScrollSpeed;
        fixed _XOffSet;
        fixed _YOffSet;
		sampler2D _MainTex;
		sampler2D _DistortionTex;
		half _Flickering;
        half _DistortionAmount;
        half _EmissionAmount;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			
			fixed2 scrolledUV = IN.uv_DistortionTex;
			
			fixed2 mainTexUV = IN.uv_MainTex;
 
            fixed yScrollValue = yScrollSpeed * _Time;
 
            mainTexUV += fixed2((_Flickering + _XOffSet), (_Flickering + _YOffSet)); 
            scrolledUV += fixed2(0, yScrollValue + (_SinTime.x * 0.15));

			fixed4 c = tex2D (_MainTex, mainTexUV) * _Color;
			c /= 2;
			c += (tex2D(_DistortionTex, scrolledUV) * (_DistortionAmount + _SinTime.w * 0.01));
			
			o.Albedo = c.rgb;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = _EmissionAmount * c.rgb + _SinTime.w * 0.05;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
