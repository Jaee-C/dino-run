Shader "Custom/Water"
{
    Properties
    {
        // the corresponding information of water's foam parts

        [Header(Color)]
        _Colour1("Colour 1", Color) = (1,1,1,1)
        _Colour2("Colour 2", Color) = (1,1,1,1)
        _MainTex("Transition Texture", 2D) = "white" {}
        _TexPow("Texture Power", Float) = 1
        _Frequency("Movement Frequency", Float) = 1
        _Threshold("Threshold", Float) = 1
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
            

            // decelear shader variable
            float4 _Colour1;
            float4 _Colour2;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TexPow;
            float _Frequency;
            float _Threshold;

            // get CPU date to the vertex function
            struct vertIn
            {
                float4 vertex           :POSITION;
                float2 uv               :TEXCOORD0;
                fixed4 vertColor :COLOR0;  
            };

            struct vertOut
            {
                float4 vertex           :SV_POSITION;   
                float2 uv               :TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                fixed4 vertColor :COLOR0;    
            };

            vertOut vert( vertIn v)
            {
                vertOut o;

                float4 displacement = float4(0.0f, 0.0f, sin(v.vertex.x + 0.1 + _Time.y * 0.01), 0.0f);
				v.vertex += displacement;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv;
                o.vertColor = v.vertColor;
				return o;
            }

            float4 frag(vertOut v):SV_TARGET
            {
                // half4 tex = tex2D(_MainTex, v.uv + _Time.y * 0.1);
                // // tex = pow(tex, _TexPow);

                // // Colouring the black parts of the texture with Colour1
                // half4 colour1 = tex * _Colour1;

                // // Colouring the white parts of the texture with Colour2
                // half3 colour2 = tex * _Colour2.rgb;


                // float4 finalColour = colour1 * v.vertColor;
                // finalColour.a *= tex2D(_MainTex, v.uv2).r;
                // finalColour.a *= 1 - ((v.vertex.z / v.vertex.w));
                // return float4(finalColour);
                // makes uv move and return the new uv
                
                float2 uv = v.uv + _Frequency * _Time.x;
                half4 tex = tex2D(_MainTex, uv);
                tex = pow(tex, _TexPow);

                half4 fog = tex * _Colour1;
                // half4 transparent = (float4(1.0 - tex.rgb, 1.0) * _Colour2.rgb, 0.1f);
                half4 finalColour = fog;
                finalColour.a = tex2D(_MainTex, uv);
                // finalColour.a *= 1 - ((v.vertex.z / v.vertex.w));
                return finalColour;
                
                // finalColour.a *= 1 - ((v.vertex.z / v.vertex.w));
            }
            ENDHLSL 
        }
    }
}