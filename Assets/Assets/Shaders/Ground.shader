Shader "Custom/Ground" {
	Properties {
		_Color ("MainColor", Color) = (1,1,1,1)
		_HotColor ("HotColor", Color) = (1,0,0,1)
		_HotStartRegion("Hot Start Region", Range(0,1)) = 0
		_HotEndRegion("Hot End Region", Range(0,1)) = 0.5
		_ColdStartRegion("Cold Start Region", Range(0,1)) = 0.5
		_ColdEndRegion("Cold End Region", Range(0,1)) = 1.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
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
		
		half lin(half min, half max, half value, int inverse) {
            half range = max - min;
            half result = (value - min) / range;
            result = clamp(result, 1, 0);
            return inverse == 1 ? 1 - result : result;
        }

		half _Glossiness;
		half _Metallic;
		half _BlendRegion;
		half _HotStartRegion;
		half _HotEndRegion;
		half _ColdStartRegion;
		half _ColdEndRegion;
		fixed4 _Color;
		fixed4 _HotColor;
		
		void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.yPosition = v.vertex.yw;
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
		    float weight = IN.yPosition / 4 + 2;			
			fixed4 c = lerp(_HotColor, _Color, weight);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
