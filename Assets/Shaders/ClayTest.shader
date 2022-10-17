Shader "Custom/ClayTest"
{
    Properties
    {
        // the corresponding information of water's foam parts

        [Header(Color)]
        _Color("Colour", Color) = (1,1,1,1)

        [Header(Fingerprints)]
        _FingerprintTex("CausticTex", 2D) = "white" {}
        _FingerprintColour1("Fingerprint Colour1", Color) = (1,1,1,1)
        _FingerprintColour2("Fingerprint Colour2", Color) = (1,1,1,1)

    }

    SubShader
    {
        Pass{
            HLSLPROGRAM                             
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"                
            #include "Lighting.cginc"

            // decelear shader variable
            float4 _Color;
            float _FoamNoise;
            float4 _FingerprintColour1;
            float4 _FingerprintColour2;

            sampler2D _CameraDepthTexture;
            sampler2D _FoamTex;
            float4 _FoamTex_ST;
            sampler2D _FingerprintTex;
            float4 _FingerprintTex_ST;

            sampler2D _MainTex;

            // get CPU date to the vertex function
            struct vertIn
            {
                float4 vertex           :POSITION;
                float2 uv               :TEXCOORD0;
            };

            struct vertOut
            {
                float4 vertex           :SV_POSITION;   
                float2 uv               :TEXCOORD0;     
                float3 pos_world        :TEXCOORD1;     
                float4 pos_screen       :TEXCOORD2;     
            };

            struct Input {
                float2 uv_MainTex;
            };

            void surf (Input IN, inout SurfaceOutput o) {
                o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
            }

            vertOut vert( vertIn v)
            {
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

 
            float4 frag(vertOut i):SV_TARGET
            {
                float4 waterColor = _Color;

                // sampled twice for focal dispersion and misaligned by _FingerprintTex_ST
                half fingerprintcolour1 = tex2D(_FingerprintTex,i.uv + _FingerprintTex_ST.xy).r * _FingerprintColour1.rgb;
                half fingerprintcolour2 = tex2D(_FingerprintTex,i.uv + _FingerprintTex_ST.zw).r * _FingerprintColour2.rgb;

                //combine all
                float3 finalColor = waterColor.rgb + fingerprintcolour1+ fingerprintcolour2;

                return float4(finalColor, 1);
            }
            ENDHLSL 
        }
    }
    FallBack "Diffuse"
}