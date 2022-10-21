Shader "Custom/PlaneTransition"
{
    Properties
    {
        // the corresponding information of water's foam parts

        [Header(Color)]
        _Colour1("Colour 1", Color) = (1,1,1,1)
        _Colour2("Colour 2", Color) = (1,1,1,1)
        _MainTex("Transition Texture", 2D) = "white" {}
        _TexPow("Texture Power", Float) = 1
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
            float4 _Colour1;
            float4 _Colour2;

            sampler2D _MainTex;
            float _TexPow;

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
            };

            vertOut vert( vertIn v)
            {
                // Mapping the input vertices to the output vertices (no change)
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(vertOut v):SV_TARGET
            {
                half4 tex = tex2D(_MainTex, v.uv);
                tex = pow(tex, _TexPow);

                // Colouring the black parts of the texture with Colour1
                half3 colour1 = float4(1.0 - tex.rgb, 1.0) * _Colour1.rgb;

                // Colouring the white parts of the texture with Colour2
                half3 colour2 = tex * _Colour2.rgb;

                float3 finalColor = colour1 + colour2;
                return float4(finalColor, 1);
            }
            ENDHLSL 
        }
    }
    FallBack "Diffuse"
}