Shader "Custom/Water"
{
    Properties
    {
        [Header(Color)]
        _Colour("Colour", Color) = (1, 1, 1, 1)
        _MainTex("Main Texture", 2D) = "white" {}
        _TexPow("Texture Power", Float) = 1
        _Frequency("Movement Frequency", Float) = 1
    }

    SubShader
    {
        Tags { 
            "Queue" = "Transparent" //change queue to transparent
            "RenderType" = "Transparent"
        }
        // display destination color without modify the source color
        Blend SrcAlpha OneMinusSrcAlpha
        // ZWrite Off is for drawing semitransparent effect
        ZWrite Off
        // Disable Culling, to show all faces of the object
        Cull Off
        Pass{
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"                
            #include "Lighting.cginc"

            // decelear shader variables
            float4 _Colour;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TexPow;
            float _Frequency;

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

            // Implementing Vertex Shader
            vertOut vert( vertIn v)
            {
                vertOut o;

                // Create waves; similar to Workshop 7
                float4 displacement = float4(0.0f, 0.0f, sin(v.vertex.x + 0.1 + _Time.y * 0.01), 0.0f);
				v.vertex += displacement;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
            }

            // Implementing Pixel/Fragment Shader
            float4 frag(vertOut v):SV_TARGET
            {
                // Make texture move over time
                float2 uv = v.uv + _Frequency * _Time.x;
                half4 tex = tex2D(_MainTex, uv);
                // Enhance the contrast of the texture
                tex = pow(tex, _TexPow);

                // Colouring texture
                half4 finalColour = tex * _Colour;
                // Setting transparency to be the same as texture
                finalColour.a = tex2D(_MainTex, uv);

                return finalColour;
            }
            ENDHLSL 
        }
    }
}