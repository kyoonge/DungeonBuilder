Shader "Custom/Lim" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
        _PulseSpeed ("Pulse Speed", Range(0, 10)) = 2
    }
 
    SubShader {
        Tags { "Queue" = "Transparent" }
        LOD 100
 
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
 
            float _OutlineWidth;
            float _PulseSpeed;
 
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            half4 frag (v2f i) : SV_Target {
                half4 col = tex2D(_MainTex, i.uv);
                half dist = tex2D(_MainTex, i.uv + float2(_OutlineWidth, 0)).r;
                half pulse = 0.5 * (1 + sin(_Time.y * _PulseSpeed));
                half4 outlineColor = half4(_OutlineColor.rgb, dist * pulse);
                col = lerp(col, outlineColor, dist);
                return col;
            }
            ENDCG
        }
    }
}