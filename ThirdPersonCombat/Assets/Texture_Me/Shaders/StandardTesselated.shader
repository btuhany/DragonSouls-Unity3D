Shader "TextureMe/Tessellation" {
    Properties{
        
        _Displacement("Height", Range(0, 0.2)) = 0.05
        _EdgeLength("Tesselation Size", Range(2,50)) = 11
        _AbientOccStrength("Ambient Occlusion Strength", Range(0,1)) = 0.5
        [Space]
        [Space]
        [Space]
        _OffsetTile("Offset, Tiling", Vector) = (1,1,0,0)
        [NoScaleOffset]
        _MainTex("Base (RGB)", 2D) = "white" {}
        [NoScaleOffset]
        _DispTex("Height Texture", 2D) = "gray" {}
        [NoScaleOffset]
        _NormalTex("Normalmap", 2D) = "bump" {}
        [NoScaleOffset]
        _AmbientOccTex("Ambient Occlusion", 2D) = "white" {}
        [NoScaleOffset]
        _SpecularTex("Specularity", 2D) = "white"{}
        _Smoothness("Smoothness", Range(0,1)) = 0
        [NoScaleOffset]
        _EmissiveTex("Emissive", 2D) = "black" {}
        _EmissiveStrength("Emissive Strength", Range(0,1)) = 1.0

        _Color("Color", color) = (1,1,1,0)
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 300

            CGPROGRAM
            #pragma surface surf Standard addshadow fullforwardshadows vertex:disp tessellate:tessEdge nolightmap  
            #pragma target 4.6
            #include "Tessellation.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct Input {
                float2 uv_MainTex;
            };


            uniform sampler2D _MainTex;
            uniform sampler2D _DispTex;
            uniform sampler2D _NormalTex;
            uniform sampler2D _AmbientOccTex;
            uniform sampler2D _SpecularTex;
            uniform sampler2D _EmissiveTex;

            uniform float _EdgeLength;
            uniform float _Displacement;
            uniform float _AbientOccStrength;
            uniform float4 _OffsetTile;
            uniform fixed4 _Color;
            uniform fixed4 _Specular;
            uniform float _Smoothness;
            uniform float _EmissiveStrength;

            float4 tessEdge(appdata v0, appdata v1, appdata v2) {
                return UnityEdgeLengthBasedTessCull(v0.vertex, v1.vertex, v2.vertex, _EdgeLength, 0.2);
            }

            void disp(inout appdata v) {
                float2 disp_uv = v.texcoord.xy*_OffsetTile.xy + _OffsetTile.zw;
                float d = (tex2Dlod(_DispTex, float4(disp_uv.xy,0,0)).r * 2 - 1) * _Displacement;
                v.vertex.xyz += (v.normal * d);
            }

            void surf(Input IN, inout SurfaceOutputStandard o){
                float2 uv = IN.uv_MainTex*_OffsetTile.xy + _OffsetTile.zw;
                o.Albedo = (tex2D(_MainTex, uv) * _Color).rgb;
                o.Occlusion = lerp(tex2D(_AmbientOccTex, uv).r, 1, 1-_AbientOccStrength);
                o.Normal = UnpackNormal(tex2D(_NormalTex, uv));
                o.Smoothness = tex2D(_SpecularTex, uv).r * _Smoothness;
                o.Emission = tex2D(_EmissiveTex, uv).rgb*_EmissiveStrength;
            }
            ENDCG
        }
            FallBack "Standard"
}