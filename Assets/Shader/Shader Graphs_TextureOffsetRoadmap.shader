Shader "Shader Graphs/TextureOffsetRoadmap" {
	Properties {
		_ScrollValue ("ScrollValue", Float) = -2.07
		[NoScaleOffset] _Texture_1 ("Texture_1", 2D) = "white" {}
		_Offset_1 ("Offset_1", Vector) = (0,0.4,0,0)
		_Tiling_1 ("Tiling_1", Vector) = (0,0,0,0)
		[NoScaleOffset] _Texture_2 ("Texture_2", 2D) = "white" {}
		_Offset_2 ("Offset_2", Vector) = (0,0,0,0)
		_Tiling_2 ("Tiling_2", Vector) = (0,0,0,0)
		[NoScaleOffset] _Texture_3 ("Texture_3", 2D) = "white" {}
		_Offset_3 ("Offset_3", Vector) = (0,0,0,0)
		_Tiling_3 ("Tiling_3", Vector) = (0,0,0,0)
		[NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
		[HideInInspector] _BUILTIN_QueueOffset ("Float", Float) = 0
		[HideInInspector] _BUILTIN_QueueControl ("Float", Float) = -1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}