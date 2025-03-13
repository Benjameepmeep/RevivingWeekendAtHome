Shader "Sprites/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Float) = 2
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Enhanced outline detection with more samples for thicker outlines
                float2 pixelSize = _MainTex_TexelSize.xy;
                
                float neighborAlpha = 0;
                
                // Sample in more directions to create a more complete outline
                // The _OutlineWidth directly controls how far we sample
                for (int x = 1; x <= _OutlineWidth; x++)
                {
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(x * pixelSize.x, 0)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(-x * pixelSize.x, 0)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(0, x * pixelSize.y)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(0, -x * pixelSize.y)).a);
                    
                    // Diagonal samples for better outline
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(x * pixelSize.x, x * pixelSize.y)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(-x * pixelSize.x, x * pixelSize.y)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(x * pixelSize.x, -x * pixelSize.y)).a);
                    neighborAlpha = max(neighborAlpha, tex2D(_MainTex, i.uv + float2(-x * pixelSize.x, -x * pixelSize.y)).a);
                }
                
                // If this pixel is transparent but has opaque neighbors, it's an outline pixel
                float outlineAlpha = neighborAlpha * (1 - col.a) * _OutlineColor.a;
                
                // Blend the outline and sprite colors
                col.rgb = lerp(_OutlineColor.rgb, col.rgb, col.a);
                col.a = max(col.a, outlineAlpha);
                
                return col;
            }
            ENDCG
        }
    }
}