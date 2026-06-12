Shader "BaobabGuardian/BaobabTree"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // Glow Properties
        _GlowColor ("Glow Color", Color) = (1, 0.84, 0, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 2)) = 1
        _GlowPulseSpeed ("Glow Pulse Speed", Range(0, 5)) = 2
        
        // Leaf Sway Properties
        _LeafSwayAmount ("Leaf Sway Amount", Range(0, 0.3)) = 0.1
        _LeafSwaySpeed ("Leaf Sway Speed", Range(0, 5)) = 2
        
        // Energy Flow
        _EnergyFlow ("Energy Flow", Range(0, 1)) = 0.5
        _EnergySpeed ("Energy Speed", Range(0, 5)) = 1
        
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float2 worldPos : TEXCOORD1;
            };

            fixed4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowPulseSpeed;
            float _LeafSwayAmount;
            float _LeafSwaySpeed;
            float _EnergyFlow;
            float _EnergySpeed;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                OUT.worldPos = IN.vertex.xy;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D (_AlphaTex, uv);
                color.a = lerp (color.a, alpha.r, alpha.a);
                #endif

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Sample sprite texture
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                
                // If pixel is fully transparent, discard it
                if (c.a < 0.01)
                    discard;
                
                // Calculate glow pulse
                float glowPulse = sin(_Time.y * _GlowPulseSpeed) * 0.5 + 0.5;
                float glowAmount = glowPulse * _GlowIntensity;
                
                // Calculate energy flow (vertical movement)
                float energyFlow = frac(_Time.y * _EnergySpeed + IN.texcoord.y);
                
                // Detect trunk area (darker brown colors) - roughly center of sprite
                float trunkMask = 0;
                if (IN.texcoord.x > 0.35 && IN.texcoord.x < 0.65)
                {
                    // Check if pixel is brownish (trunk)
                    if (c.r > 0.3 && c.r > c.g * 0.8 && c.b < c.g * 0.6)
                    {
                        trunkMask = 1;
                    }
                }
                
                // Detect leaf area (greenish colors)
                float leafMask = 0;
                if (c.g > c.r * 1.1 && c.g > c.b)
                {
                    leafMask = 1;
                }
                
                // Apply glow to trunk area
                float4 glowEffect = _GlowColor * glowAmount * trunkMask;
                c.rgb += glowEffect.rgb * glowEffect.a;
                
                // Apply energy flow to trunk
                float energyGlow = sin(energyFlow * 3.14159) * _EnergyFlow;
                c.rgb += float3(0.7, 0.9, 0.2) * energyGlow * trunkMask * 0.5;
                
                // Add subtle leaf shimmer
                float leafShimmer = sin(_Time.y * _LeafSwaySpeed + IN.texcoord.x * 10) * 0.5 + 0.5;
                c.rgb += float3(0.1, 0.15, 0) * leafShimmer * leafMask * 0.3;
                
                // Maintain alpha
                c.a = c.a;
                
                return c;
            }
        ENDCG
        }
    }
}
