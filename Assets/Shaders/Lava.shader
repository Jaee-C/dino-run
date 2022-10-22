Shader "Custom/Lava"
{
    Properties
    {
        // the corresponding information of lava's foam parts
        [Header(Foam)]
        _FoamTex("Foam Texture", 2D) = "white" {}         // foam noise picture
        _FoamColour("Foam Colour", Color) = (1,1,1,1)     // default lava color
        _FoamRange("Foam Range", Range(0,1)) = 1          // foam range
        _FoamSpeed("Foam Speed", Float) = 0.1             // foam flow speed
        _FoamNoise("Foam Noise", Float) = 1               // noise range

        [Header(Lava Colour)]
        _LavaColour("Lava Colour",Color) = (1,1,1,1)

        [Header(Wave)]
        _WaveFrequencySpeed("X Frequency(x), X Speed(y), Z Frequency(z), Z Speed(w),",Vector) = (0.2,1,0.2,1)

        [Header(Caustics)]
        _CausticTex("Caustic Texture", 2D) = "white" {}          //caustic texture
        _CausticColour1("Caustics Colour 1",Color) = (1,1,1,1)   // caustics color 1 and 2
        _CausticColour2("Caustics Colour 2",Color) = (1,1,1,1)

    }

    SubShader
    {
        Tags { 
            "Queue" = "Transparent" //change queue to transparent
        }

        Pass{
            HLSLPROGRAM                             
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"                
            #include "Lighting.cginc"

            // decelear shader variable
            float _FoamRange;
            float4 _LavaColour;
            float4 _FoamColour;
            float _FoamSpeed;
            float4 _WaveFrequencySpeed;
            float _FoamNoise;
            float4 _CausticColour1;
            float4 _CausticColour2;

            sampler2D _FoamTex;
            sampler2D _CausticTex;
            float4 _CausticTex_ST;

            // get CPU date to the vertex function
            struct vertIn
            {
                float4 vertex           :POSITION;  // vertex data
                float2 uv               :TEXCOORD0; // uv data
            };

            struct vertOut
            {
                float4 vertex           :SV_POSITION;   
                float2 uv               :TEXCOORD0;       
            };

            // Implementing Vertex Shader
            vertOut vert( vertIn v)
            {
                vertOut o;

                // Create Wave movement using cos and sin with the time
                // v.vertex.y += cos(_Time.y * _WaveFrequencySpeed.y +  v.vertex.x) * _WaveFrequencySpeed.x;
                // v.vertex.y += sin(_Time.y * _WaveFrequencySpeed.w +  v.vertex.z ) * _WaveFrequencySpeed.z;

                // Convert model vertices in model space to model vertices in clipping space
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;
                return o;
            }

            // Implementing Pixel/Fragment Shader
            float4 frag(vertOut v): SV_Target
            {
                half foamRange = _FoamRange;

                // Make the foam move diagonally accrosss over time
                half foam_tex = tex2D(_FoamTex, v.uv + _Time.y * _FoamSpeed);
                // Enhance the contrast of the foam texture
                foam_tex = pow(foam_tex,_FoamNoise);

                // Makes the foam have hard edges  (stylistic choice)
                half4 foam_color = foamRange < foam_tex * _FoamColour;

                // Place two caustic textures
                half caustic1 = tex2D(_CausticTex, v.uv + _CausticTex_ST.xy).r;
                half caustic2 = tex2D(_CausticTex, v.uv + _CausticTex_ST.zw).r;
                half3 caustic_color1 = caustic1 * _CausticColour1.rgb;
                half3 caustic_color2 = caustic2 * _CausticColour2.rgb;

                // Combine the foam, caustics and lava colour to get the final
                float3 finalColour = _LavaColour.rgb + foam_color.rgb + caustic_color1+ caustic_color2;

                return float4(finalColour, 1);
            }
            ENDHLSL 
        }
    }
    FallBack "Packages/com.unity.render-pipelines.universal/FallbackError"
}