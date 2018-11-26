Shader "Custom/Ground" {
	Properties {
		_ColdColor ("Cold Color", Color) = (1,1,1,1)
		_HotColor ("Hot Color", Color) = (1,0,0,1)
		_BlendRegion("Blend Region", Range(0, 1)) = 0.5
		_BlendThreshold("Blend Threshold", Range(0, 1)) = 0.1
		_BlendOscilation("Blend Oscilation", float) = 0.05
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
		
		half lin(half min, half max, half value, int inverse) {
            half range = max - min;
            half result = (value - min) / range;
            result = clamp(result, 1, 0);
            return inverse == 1 ? 1 - result : result;
        }

		half _Glossiness;
		half _Metallic;
		half _BlendRegion;
		half _BlendThreshold;
		half _BlendOscilation;
		half _FlashAmount;
		fixed4 _ColdColor;
		fixed4 _HotColor;
		
		void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.yPosition = v.vertex.y / 5;//GAMBIARRA DO CARALHOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
        }

		void surf (Input IN, inout SurfaceOutputStandard o) {
		    float weight = IN.yPosition * 0.5 + 0.5;		
		    float blend = _BlendRegion + (_SinTime.w * 0.5 + 0.5) * _BlendOscilation;
		    fixed4 c;
		    if (weight > blend + _BlendThreshold){
		        c = _ColdColor;
		    } else if (weight < blend - _BlendThreshold) {
		        c = _HotColor;      
		        o.Emission = c.rgb;
		    } else {
		        fixed percent = (weight - (blend - _BlendThreshold)) / (_BlendThreshold * 2);
		        c = lerp(_HotColor, _ColdColor, percent);
		        o.Emission = c.rgb * (1 - percent);
		    }
			o.Albedo = c.rgb;
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
