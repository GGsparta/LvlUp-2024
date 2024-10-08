Shader "CraftemIpsum/NormalMapCreator"
{
    // The properties block of the Unity shader. In this example this block is empty
    // because the output color is predefined in the fragment shader code.
    Properties
    { 
        _MainTex ("Texture", 2D) = "white"
        _Top ("Texture", 2D) = "white"
        _Right ("Texture", 2D) = "white"
        _Left ("Texture", 2D) = "white"
    }

    // The SubShader block containing the Shader code. 
    SubShader
    {
        // SubShader Tags define when and under which conditions a SubShader block or
        // a pass is executed.
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            // The HLSL code block. Unity SRP uses the HLSL language.
            HLSLPROGRAM
            // This line defines the name of the vertex shader. 
            #pragma vertex vert
            // This line defines the name of the fragment shader. 
            #pragma fragment frag

            // The Core.hlsl file contains definitions of frequently used HLSL
            // macros and functions, and also contains #include references to other
            // HLSL files (for example, Common.hlsl, SpaceTransforms.hlsl, etc.).
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            // The structure definition defines which variables it contains.
            // This example uses the Attributes structure as an input structure in
            // the vertex shader.
            struct Attributes
            {
                // The positionOS variable contains the vertex positions in object
                // space.
                float4 positionOS   : POSITION;   
                float2 uv           : TEXCOORD0;              
            };

            struct Varyings
            {
                // The positions in this struct must have the SV_POSITION semantic.
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };        
            
            TEXTURE2D(_MainTex);
            TEXTURE2D(_Top);
            TEXTURE2D(_Left);
            TEXTURE2D(_Right);

            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_Top);
            SAMPLER(sampler_Left);
            SAMPLER(sampler_Right);
            
            CBUFFER_START(UnityPerMaterial)
                // The following line declares the _BaseMap_ST variable, so that you
                // can use the _BaseMap variable in the fragment shader. The _ST 
                // suffix is necessary for the tiling and offset function to work.
                float4 _MainTex_ST;
                float4 _Top_ST;
                float4 _Left_ST;
                float4 _Right_ST;
            CBUFFER_END

            // The vertex shader definition with properties defined in the Varyings 
            // structure. The type of the vert function must match the type (struct)
            // that it returns.
            Varyings vert(Attributes IN)
            {
                // Declaring the output object (OUT) with the Varyings struct.
                Varyings OUT;
                // The TransformObjectToHClip function transforms vertex positions
                // from object space to homogenous space
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                // Returning the output.
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

                return OUT;
            }

            // The fragment shader definition.            
            half4 frag(Varyings IN) : SV_Target
            {
                // sample the texture
                half t = SAMPLE_TEXTURE2D(_Top, sampler_Top, IN.uv).r;
                half l = SAMPLE_TEXTURE2D(_Left, sampler_Left, IN.uv).r;
                half r = SAMPLE_TEXTURE2D(_Right, sampler_Right, IN.uv).r;

                half3 n = half3(r - l, t, 0);
                n.z = sqrt(1 - n.x*n.x - n.y*n.y);
                n = (n + 1) / 2.0;

                return half4(n, 1);
            }
            ENDHLSL
        }
    }
}
