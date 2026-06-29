Shader "Custom/GrassInstanced"
{
    Properties
    {
        _MainTex      ("Texture",       2D)    = "white" {}
        _Color        ("Color Tint",    Color)  = (0.4, 0.8, 0.3, 1)
        _ColorBottom  ("Color Bottom",  Color)  = (0.2, 0.5, 0.1, 1)
        _Cutoff       ("Alpha Cutoff",  Range(0,1)) = 0.3
        _Smoothness   ("Smoothness",    Range(0,1)) = 0.1
        _WindStrength  ("Wind Strength",  Float) = 0.5
        _WindFrequency ("Wind Frequency", Float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "TransparentCutout"
            "Queue"      = "AlphaTest"
        }

        Cull Off
        LOD 200

        // ═══════════════════════════════════════════════
        // PASS 1 — Forward Rendering (Built-in RP)
        // ═══════════════════════════════════════════════
        Pass
        {
            Name "ForwardBase"
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            // GPU Instancing — WAJIB
            #pragma multi_compile_instancing
            #pragma instancing_options assumeuniformscaling

            // Built-in RP lighting variants
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4    _MainTex_ST;
            fixed4    _Color;
            fixed4    _ColorBottom;
            float     _Cutoff;
            float     _WindStrength;
            float     _WindFrequency;

            // Global dari GrassWind.cs
            float4 _WindDir;    // xyz = arah, w = time * speed

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv     : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float  height : TEXCOORD2;
                UNITY_FOG_COORDS(3)
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert(appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                float3 posOS = v.vertex.xyz;

                // Faktor ketinggian dari UV.y (0=pangkal, 1=ujung)
                float h = saturate(v.uv.y);
                o.height = h;

                // Animasi angin — hanya ujung yang berayun (h*h agar pangkal diam)
                float3 worldPos  = mul(unity_ObjectToWorld, v.vertex).xyz;
                float  windNoise = sin(_WindDir.w + worldPos.x * _WindFrequency
                                                  + worldPos.z * _WindFrequency * 0.7);
                float  windOff   = windNoise * _WindStrength * h * h;
                posOS.x += _WindDir.x * windOff;
                posOS.z += _WindDir.z * windOff;

                o.pos    = UnityObjectToClipPos(float4(posOS, 1));
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                fixed4 tex = tex2D(_MainTex, i.uv);
                clip(tex.a - _Cutoff);

                // Gradient warna bawah → atas
                fixed4 col = lerp(_ColorBottom, _Color, i.height) * tex;

                // Pencahayaan sederhana
                half3 normalDir = normalize(i.normal);
                half  NdotL     = max(0, dot(normalDir, _WorldSpaceLightPos0.xyz));
                half3 lighting  = _LightColor0.rgb * (NdotL * 0.7 + 0.3);

                fixed4 finalCol = fixed4(col.rgb * lighting, 1.0);
                UNITY_APPLY_FOG(i.fogCoord, finalCol);
                return finalCol;
            }
            ENDCG
        }

        // ═══════════════════════════════════════════════
        // PASS 2 — Shadow Caster (Built-in RP)
        // ═══════════════════════════════════════════════
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off

            CGPROGRAM
            #pragma vertex   vertShadow
            #pragma fragment fragShadow
            #pragma multi_compile_instancing
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float     _Cutoff;

            struct appdata_shadow
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f_shadow
            {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            v2f_shadow vertShadow(appdata_shadow v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f_shadow o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 fragShadow(v2f_shadow i) : SV_Target
            {
                fixed a = tex2D(_MainTex, i.uv).a;
                clip(a - _Cutoff);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }

    FallBack "Legacy Shaders/Transparent/Cutout/Diffuse"
}
