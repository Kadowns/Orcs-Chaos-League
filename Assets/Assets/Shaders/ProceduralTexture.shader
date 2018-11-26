Shader "Custom/Noise/Texture"
{
    Properties
    {
        _OscilationScale ("Oscilation Scale", float) = 1.0
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _GridSize ("GridSize", float) = 1.0
        _X ("Seed X", float) = 1.0
        _Y ("Seed Y", float) = 1.0
        _AnimationTime("Animation Time", float) = 0.0
        _Scale("Noise Scale", float) = 1.0
        _Persistence("Persistence", float) = 0.5
        _Lacunarity("Lacunarity", float) = 2.0
        _Octaves("Octaves", int) = 8
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            float3 mod289(float3 x) {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }

            float4 mod289(float4 x) {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }

            float4 permute(float4 x) {
                return mod289(((x*34.0)+1.0)*x);
            }

            float4 taylorInvSqrt(float4 r) {
                return 1.79284291400159 - 0.85373472095314 * r;
            }

            float snoise(float3 v) { 
            
                const float2  C = float2(1.0/6.0, 1.0/3.0);
                const float4  D = float4(0.0, 0.5, 1.0, 2.0);            
                // First corner
                float3 i  = floor(v + dot(v, C.yyy) );
                float3 x0 =   v - i + dot(i, C.xxx) ;            
                // Other corners
                float3 g = step(x0.yzx, x0.xyz);
                float3 l = 1.0 - g;
                float3 i1 = min( g.xyz, l.zxy );
                float3 i2 = max( g.xyz, l.zxy );            

                float3 x1 = x0 - i1 + C.xxx;
                float3 x2 = x0 - i2 + C.yyy; 
                float3 x3 = x0 - D.yyy;               
                 
                // Permutations
                i = mod289(i); 
                float4 p = permute( permute( permute(
                    i.z + float4(0.0, i1.z, i2.z, 1.0 )) +
                    i.y + float4(0.0, i1.y, i2.y, 1.0 )) +
                    i.x + float4(0.0, i1.x, i2.x, 1.0 ));            
                // Gradients: 7x7 points over a square, mapped onto an octahedron.
                // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)             
                float n_ = 0.142857142857;
                float3  ns = n_ * D.wyz - D.xzx;           
                float4 j = p - 49.0 * floor(p * ns.z * ns.z);        
                float4 x_ = floor(j * ns.z);
                float4 y_ = floor(j - 7.0 * x_ );             
                float4 x = x_ *ns.x + ns.yyyy;
                float4 y = y_ *ns.x + ns.yyyy;
                float4 h = 1.0 - abs(x) - abs(y);       
                float4 b0 = float4( x.xy, y.xy );
                float4 b1 = float4( x.zw, y.zw );

                float4 s0 = floor(b0)*2.0 + 1.0;
                float4 s1 = floor(b1)*2.0 + 1.0;
                float4 sh = -step(h, float4(0.0, 0.0, 0.0, 0.0));
                float4 a0 = b0.xzyw + s0.xzyw*sh.xxyy ;
                float4 a1 = b1.xzyw + s1.xzyw*sh.zzww ;
                float3 p0 = float3(a0.xy,h.x);
                float3 p1 = float3(a0.zw,h.y);
                float3 p2 = float3(a1.xy,h.z);
                float3 p3 = float3(a1.zw,h.w);
                
                //Normalise gradients
                float4 norm = taylorInvSqrt(float4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
                p0 *= norm.x;
                p1 *= norm.y;
                p2 *= norm.z;
                p3 *= norm.w;
                
                // Mix final noise value
                float4 m = max(0.6 - float4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
                m = m * m;
                
                return 42.0 * dot( m*m, float4( dot(p0,x0), dot(p1,x1), 
                                dot(p2,x2), dot(p3,x3) ) );
            }
            
            float inverseLerp(float first, float second, float value) {
                return (value - first) / (second - first);
            }
           
            float _OscilationScale;
            float _Persistence;
            float _Lacunarity;
            float _Scale;
            float _AnimationTime;
            int _Octaves;
            fixed4 _Color;
            float _GridSize;
            float _X;
            float _Y;
            
            struct vData {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            
            float calculateNoise(float3 coord){
		                 
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (int octave = 0; octave < _Octaves; octave++) {
                    float sampleX = coord.x / _Scale * frequency;
                    float sampleZ = coord.y / _Scale * frequency;
                    float ns = snoise(float3(sampleX, sampleZ, coord.z));
                    noiseHeight += ns * amplitude;
                    
                    amplitude *= _Persistence;
                    frequency *= _Lacunarity;
                }                    
                noiseHeight += _AnimationTime;       
                return noiseHeight;
		    }
		    
            vData vert(appdata_img v) {
                vData  o;
                v.vertex.y += sin((v.vertex.x + v.vertex.z) * _Time.x * 0.05) * _OscilationScale;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }
            
            fixed4 frag (vData v) : SV_Target {
			    // Albedo comes from a texture tinted by color
		        v.uv *= _GridSize;
                v.uv += float2(_Time.w * _X, _Time.w * _Y);
			    float4 c = _Color * calculateNoise(float3(v.uv, _Time.y * 0.5f));
			    return c;		
		    }
            ENDCG
        }
    }
}