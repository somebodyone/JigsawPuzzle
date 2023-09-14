Shader "Unlit/PuzzlePieceOptimized"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		[HideInInspector] _MainTex("Albedo (RGB)", 2D) = "white" {}
		[HideInInspector] _MainUv("Albedo (RGB)", Vector) = (1,1,1,1)
		_Connectors("Connectors", Vector) = (1,1,1,1)
		[HideInInspector] _Clip("Clip", float) = 0.2
		_Transparency("Transparency", float) = 1.0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex;
			float4 _MainTex_ST;
            float4 _MainUv;
			float4 _Color;
			float4 _Connectors;
			float _Clip;
			float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float circle(float2 samplePosition, float radius)
			{
				return length(samplePosition) - radius;
			}

			float rectangle(float2 samplePosition, float2 halfSize)
			{
				float2 componentWiseEdgeDistance = abs(samplePosition) - halfSize;
				return max(componentWiseEdgeDistance.x, componentWiseEdgeDistance.y);
			}

			float4 smax(float4 d1, float d2, float k, float4 sign)
			{
				float4 h = clamp(0.5 - 0.5*sign*(d2 + d1 * sign) / k, 0.0, 1.0);
				return lerp(d2, -d1* sign, h) + sign*k * h*(1.0 - h);
			}

			float distance(float2 uv)
			{
				float cir1 = circle(uv - float2(lerp(0.27, 0.3967, _Connectors.x), 0.5), 0.05);
				float cir2 = circle(uv - float2(lerp(0.73, 0.6033, _Connectors.y), 0.5), 0.05);
				float cir3 = circle(uv - float2(0.5, lerp(0.73, 0.6033, _Connectors.z)), 0.05);
				float cir4 = circle(uv - float2(0.5, lerp(0.27, 0.3967, _Connectors.w)), 0.05);

				float rec = rectangle(uv - float2(0.5, 0.5), float2(0.166666, 0.166666));

				float4 cir = float4(cir1, cir2, cir3, cir4);

				cir = smax(cir, rec, 0.05, (_Connectors - 0.5) * 2);

				float2 norm = normalize(uv - 0.5);

				return cir.x * saturate((dot(float2(-1, 0), norm) - 0.707) * 2)
					 + cir.y * saturate((dot(float2( 1, 0), norm) - 0.707) * 2)
					 + cir.z * saturate((dot(float2( 0, 1), norm) - 0.707) * 2)
					 + cir.w * saturate((dot(float2( 0,-1), norm) - 0.707) * 2);
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uvs = lerp(_MainUv.xy, _MainUv.zw,i.uv);
				fixed4 c = tex2D(_MainTex,uvs) * _Color;
				float alfa = c.a;
				float3 color = c.rgb;

				float dist = distance(i.uv) ;
				
				alfa *= 1-dist;
				/*
				if (alfa > 0 && alfa < 1)
				{
					float value = abs((alfa - 0.5) * 2);
					color *= value * value;
				}*/
				clip(alfa - _Clip);
				alfa *= _Transparency;
                return float4(color,alfa);
            }
            ENDCG
        }
    	        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            sampler2D _MainTex;
			float4 _MainTex_ST;
            float4 _MainUv;
			float4 _Color;
			float4 _Connectors;
			float _Clip;
			float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			float circle(float2 samplePosition, float radius)
			{
				return length(samplePosition) - radius;
			}

			float rectangle(float2 samplePosition, float2 halfSize)
			{
				float2 componentWiseEdgeDistance = abs(samplePosition) - halfSize;
				return max(componentWiseEdgeDistance.x, componentWiseEdgeDistance.y);
			}

			float4 smax(float4 d1, float d2, float k, float4 sign)
			{
				float4 h = clamp(0.5 - 0.5*sign*(d2 + d1 * sign) / k, 0.0, 1.0);
				return lerp(d2, -d1* sign, h) + sign*k * h*(1.0 - h);
			}

			float distance(float2 uv)
			{
				float cir1 = circle(uv - float2(lerp(0.27, 0.3967, _Connectors.x), 0.5), 0.05);
				float cir2 = circle(uv - float2(lerp(0.73, 0.6033, _Connectors.y), 0.5), 0.05);
				float cir3 = circle(uv - float2(0.5, lerp(0.73, 0.6033, _Connectors.z)), 0.05);
				float cir4 = circle(uv - float2(0.5, lerp(0.27, 0.3967, _Connectors.w)), 0.05);

				float rec = rectangle(uv - float2(0.5, 0.5), float2(0.166666, 0.166666));

				float4 cir = float4(cir1, cir2, cir3, cir4);

				cir = smax(cir, rec, 0.05, (_Connectors - 0.5) * 2);

				float2 norm = normalize(uv - 0.5);

				return cir.x * saturate((dot(float2(-1, 0), norm) - 0.707) * 2)
					 + cir.y * saturate((dot(float2( 1, 0), norm) - 0.707) * 2)
					 + cir.z * saturate((dot(float2( 0, 1), norm) - 0.707) * 2)
					 + cir.w * saturate((dot(float2( 0,-1), norm) - 0.707) * 2);
			}

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uvs = lerp(_MainUv.xy, _MainUv.zw,i.uv);
				fixed4 c = tex2D(_MainTex,uvs) * _Color;
				float alfa = c.a;
				float3 color = c.rgb;

				float dist = distance(i.uv) ;
				
				alfa *= 1-dist;
				/*
				if (alfa > 0 && alfa < 1)
				{
					float value = abs((alfa - 0.5) * 2);
					color *= value * value;
				}*/
				clip(alfa - _Clip);
				alfa *= _Transparency;
                return float4(color,alfa);
            }
            ENDCG
        }
    }
}
