Shader "Custom/MaskBlack2"
{
    Properties {
        _MainTex ("Base (RGB)" , 2D) = "white" {}
        _TransVal ("Transparency Value", Range(0,10)) = 1.0
    }

    SubShader {
        Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }

        LOD 200

        Lighting off

        Blend  One OneMinusSrcAlpha 

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _TransVal;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
		{
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Emission = tex2D (_MainTex, IN.uv_MainTex);
            o.Alpha = (c.r+ c.g+ c.b) *_TransVal;
        }

        ENDCG
    } 

}

