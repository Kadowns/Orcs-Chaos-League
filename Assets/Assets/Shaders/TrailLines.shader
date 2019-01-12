Shader "OCL/TrailLines"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Spacing("Spacing", float) = 1
	}
	SubShader
	{
	    Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			uniform fixed4 _Color;
			uniform fixed _Spacing;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = mul(unity_ObjectToWorld, v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = _Color;
				fixed f = frac((i.uv.y + _Time.w * _Spacing) / _Spacing);
				c.a = 3 * f * (1 - f);

				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}
			ENDCG
		}
	}
}
