Shader "AllIn1SpriteShader/AllIn1SpriteShader" {
	Properties {
		[Header(_General Properties_)] _MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Main Color", Vector) = (1,1,1,1)
		_Alpha ("General Alpha", Range(0, 1)) = 1
		_GlowColor ("Glow Color", Vector) = (1,1,1,1)
		_Glow ("Glow Intensity", Range(0, 100)) = 10
		[Space] [Space] [Toggle()] _GlowTexUsed ("Glow Texture used?", Float) = 0
		[Header(Texture does not support Tiling)] _GlowTex ("Glow Texture", 2D) = "white" {}
		_FadeTex ("Fade Texture", 2D) = "white" {}
		_FadeAmount ("Fade Amount", Range(-0.1, 1)) = -0.1
		_FadeBurnWidth ("Fade Burn Width", Range(0, 1)) = 0.025
		_FadeBurnTransition ("Fade Burn Smooth Transition", Range(0.01, 0.5)) = 0.075
		_FadeBurnColor ("Fade Burn Color", Vector) = (1,1,0,1)
		_FadeBurnTex ("Fade Burn Texture", 2D) = "white" {}
		_FadeBurnGlow ("Fade Burn Glow", Range(1, 50)) = 2
		[Header(_Outline Basic Properties_)] _OutlineColor ("Outline Base Color", Vector) = (1,1,1,1)
		_OutlineAlpha ("Outline Base Alpha", Range(0, 1)) = 1
		_OutlineGlow ("Outline Base Glow", Range(1, 100)) = 1.5
		[Toggle()] _Outline8Directions ("Outline Base High Resolution?", Float) = 0
		_OutlineWidth ("Outline Base Width", Range(0, 0.2)) = 0.004
		[Header(_Outline Width_)] [Toggle()] _OutlineIsPixel ("Outline Base is Pixel Perfect?", Float) = 0
		_OutlinePixelWidth ("Outline Base Pixel Width", Float) = 1
		[Space] [Header(_Outline Texture_)] [Toggle()] _OutlineTexToggle ("Outline uses texture?", Float) = 0
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_OutlineTexXSpeed ("Outline Texture scroll speed X axis", Range(-50, 50)) = 10
		_OutlineTexYSpeed ("Outline Texture scroll speed Y axis", Range(-50, 50)) = 0
		[Toggle()] _OutlineTexGrey ("Outline Texture is Greyscaled?", Float) = 0
		[Space] [Header(_Outline Distortion_)] [Toggle()] _OutlineDistortToggle ("Outline uses distortion?", Float) = 0
		_OutlineDistortTex ("Outline Distortion Texture", 2D) = "white" {}
		_OutlineDistortAmount ("Outline Distortion Amount", Range(0, 2)) = 0.5
		_OutlineDistortTexXSpeed ("Outline Distortion scroll speed X axis", Range(-50, 50)) = 5
		_OutlineDistortTexYSpeed ("Outline Distortion scroll speed Y axis", Range(-50, 50)) = 5
		_GradBlend ("Gradient Blend", Range(0, 1)) = 1
		_GradTopLeftCol ("Gradient Top Left Color", Vector) = (1,0,0,1)
		_GradTopRightCol ("Gradient Top Right Color", Vector) = (1,0,0,1)
		_GradBotLeftCol ("Gradient Bot Left Color", Vector) = (0,0,1,1)
		_GradBotRightCol ("Gradient Bot Right Color", Vector) = (0,0,1,1)
		_ColorSwapTex ("Color Swap Texture", 2D) = "black" {}
		[Header(You will need a mask texture (see Documentation))] _ColorSwapRed ("Color Swap Red Channel", Vector) = (1,1,1,1)
		_ColorSwapRedLuminosity ("Color Swap Red luminosity", Range(-1, 1)) = 0.5
		_ColorSwapGreen ("Color Swap Green Channel", Vector) = (1,1,1,1)
		_ColorSwapGreenLuminosity ("Color Swap Green luminosity", Range(-1, 1)) = 0.5
		_ColorSwapBlue ("Color Swap Blue Channel", Vector) = (1,1,1,1)
		_ColorSwapBlueLuminosity ("Color Swap Blue luminosity", Range(-1, 1)) = 0.5
		_HsvShift ("Hue Shift", Range(0, 360)) = 180
		_HsvSaturation ("Hue Shift Saturation", Range(0, 2)) = 1
		_HsvBright ("Hue Shift Bright", Range(0, 2)) = 1
		_HitEffectColor ("Hit Effect Color", Vector) = (1,1,1,1)
		_HitEffectGlow ("Hit Effect Glow Intensity", Range(1, 100)) = 5
		[Space] [Header(_Tip_ Animate the following property)] _HitEffectBlend ("Hit Effect Blend", Range(0, 1)) = 1
		_NegativeAmount ("Negative Amount", Range(0, 1)) = 1
		[Header(Looks bad with distorition effects)] _PixelateSize ("Pixelate size", Range(4, 512)) = 32
		[Header(Texture does not support Tiling)] _ColorRampTex ("Color ramp Texture", 2D) = "white" {}
		_ColorRampLuminosity ("Color ramp luminosity", Range(-1, 1)) = 0
		[Toggle()] _ColorRampOutline ("Color ramp affects outline?", Float) = 0
		_GreyscaleLuminosity ("Greyscale luminosity", Range(-1, 1)) = 0
		[Toggle()] _GreyscaleOutline ("Greyscale affects outline?", Float) = 0
		_GreyscaleTintColor ("Greyscale Tint Color", Vector) = (1,1,1,1)
		_PosterizeNumColors ("Posterize Number of Colors", Range(0, 100)) = 8
		_PosterizeGamma ("Posterize Amount", Range(0.1, 10)) = 0.75
		[Toggle()] _PosterizeOutline ("Posterize affects outline and glow?", Float) = 0
		[Header(This effect will not affect the outline)] _BlurIntensity ("Blur Intensity", Range(0, 100)) = 10
		[Toggle()] _BlurHD ("Blur is Low Res?", Float) = 0
		_MotionBlurAngle ("Motion Blur Angle", Range(-1, 1)) = 0.1
		_MotionBlurDist ("Motion Blur Distance", Range(-3, 3)) = 1.25
		[Header(This effect will not affect the outline)] _GhostColorBoost ("Ghost Color Boost", Range(0, 5)) = 1
		[Header(Choose the ghost color with the Main Color Property)] _GhostTransparency ("Ghost Transparency", Range(0, 1)) = 0
		[Header(This effect will place the inner outlines over the original sprite)] _InnerOutlineColor ("Inner Outline Color", Vector) = (1,0,0,1)
		_InnerOutlineThickness ("Inner Outline Thickness", Range(0, 3)) = 1
		_InnerOutlineAlpha ("Inner Outline Alpha", Range(0, 1)) = 1
		_InnerOutlineGlow ("Inner Outline Glow", Range(1, 250)) = 1
		_AlphaCutoffValue ("Alpha cutoff value", Range(0, 1)) = 0.25
		[Toggle()] _OnlyOutline ("Only render outline?", Float) = 0
		[Toggle()] _OnlyInnerOutline ("Only render innerr outline?", Float) = 0
		_HologramStripesAmount ("Hologram Stripes Amount", Range(0, 1)) = 0.2
		_HologramUnmodAmount ("Hologram Unchanged Amount", Range(0, 1)) = 0.4
		_HologramStripesSpeed ("Hologram Stripes Speed", Range(0, 20)) = 5
		_HologramMinAlpha ("Hologram Min Alpha", Range(0, 1)) = 0
		_HologramMaxAlpha ("Hologram Max Alpha", Range(0, 10)) = 1
		_ChromAberrAmount ("ChromAberr Amount", Range(0, 1)) = 1
		_ChromAberrAlpha ("ChromAberr Alpha", Range(0, 1)) = 0.4
		_GlitchAmount ("Glitch Amount", Range(0, 20)) = 3
		_FlickerPercent ("Flicker Percent", Range(0, 1)) = 0.05
		_FlickerFreq ("Flicker Frequency", Range(0, 5)) = 0.2
		_FlickerAlpha ("Flicker Alpha", Range(0, 1)) = 0
		_ShadowX ("Shadow X Axis", Range(-0.5, 0.5)) = 0.1
		_ShadowY ("Shadow Y Axis", Range(-0.5, 0.5)) = -0.05
		_ShadowAlpha ("Shadow Alpha", Range(0, 1)) = 0.5
		_ShadowColor ("Shadow Color", Vector) = (0,0,0,1)
		_HandDrawnAmount ("Hand Drawn Amount", Range(2, 20)) = 10
		_HandDrawnSpeed ("Hand Drawn Speed", Range(1, 15)) = 5
		_GrassSpeed ("Grass Speed", Range(0, 50)) = 2
		_GrassWind ("Grass Wind (bend amount)", Range(0, 50)) = 20
		[Space] [Toggle()] _GrassManualToggle ("Grass is manually animated?", Float) = 0
		_GrassManualAnim ("Grass Manual Anim", Range(-1, 1)) = 1
		_WaveAmount ("Wave Amount", Range(0, 25)) = 7
		_WaveSpeed ("Wave Speed", Range(0, 25)) = 10
		_WaveStrength ("Wave Strength", Range(0, 25)) = 7.5
		_WaveX ("Wave X Axis", Range(0, 1)) = 0
		_WaveY ("Wave Y Axis", Range(0, 1)) = 0.5
		[Header(Only on single sprites, spritesheets NOT supported)] _RectSize ("Rect Size", Range(1, 4)) = 1
		_OffsetUvX ("Offset X axis", Range(-1, 1)) = 0
		_OffsetUvY ("Offset Y axis", Range(-1, 1)) = 0
		_ClipUvLeft ("Clipping Left", Range(0, 1)) = 0
		_ClipUvRight ("Clipping Right", Range(0, 1)) = 0
		_ClipUvUp ("Clipping Up", Range(0, 1)) = 0
		_ClipUvDown ("Clipping Down", Range(0, 1)) = 0
		[Header(Set Texture Wrap Mode to Repeat)] _TextureScrollXSpeed ("Texture Scroll Speed X Axis", Range(-5, 5)) = 1
		_TextureScrollYSpeed ("Texture Scroll Speed Y Axis", Range(-5, 5)) = 0
		_ZoomUvAmount ("Zoom Amount", Range(0.1, 5)) = 0.5
		_DistortTex ("Distortion Texture", 2D) = "white" {}
		_DistortAmount ("Distortion Amount", Range(0, 2)) = 0.5
		_DistortTexXSpeed ("Distortion scroll speed X axis", Range(-50, 50)) = 5
		_DistortTexYSpeed ("Distortion scroll speed Y axis", Range(-50, 50)) = 5
		_TwistUvAmount ("Twist Amount", Range(0, 3.1416)) = 1
		_TwistUvPosX ("Twist Pos X Axis", Range(0, 1)) = 0.5
		_TwistUvPosY ("Twist Pos Y Axis", Range(0, 1)) = 0.5
		_TwistUvRadius ("Twist Radius", Range(0, 3)) = 0.75
		[Header(_Tip_ Use Clipping effect to avoid possible undesired parts)] _RotateUvAmount ("Rotate Angle (in radians)", Range(0, 6.2831)) = 0
		_FishEyeUvAmount ("Fish Eye Amount", Range(0, 0.5)) = 0.35
		_PinchUvAmount ("Pinch Amount", Range(0, 0.5)) = 0.35
		_ShakeUvSpeed ("Shake Speed", Range(0, 20)) = 2.5
		_ShakeUvX ("Shake X Multiplier", Range(0, 5)) = 1.5
		_ShakeUvY ("Shake Y Multiplier", Range(0, 5)) = 1
		_ColorChangeTolerance ("Tolerance", Range(0, 1)) = 0.25
		_ColorChangeTarget ("Color to change", Vector) = (1,0,0,1)
		_ColorChangeNewCol ("New Color", Vector) = (1,1,0,1)
		_ColorChangeLuminosity ("New Color Luminosity", Range(0, 1)) = 0
		_RoundWaveStrength ("Round Wave Strength", Range(0, 1)) = 0.7
		_RoundWaveSpeed ("Round Wave Speed", Range(0, 5)) = 2
		[Toggle()] _BillboardY ("Billboard on both axis?", Float) = 0
		_ZWrite ("Depth Write", Float) = 0
		_MySrcMode ("SrcMode", Float) = 5
		_MyDstMode ("DstMode", Float) = 10
		_ShineColor ("Shine Color", Vector) = (1,1,1,1)
		_ShineLocation ("Shine Location", Range(0, 1)) = 0.5
		_ShineWidth ("Shine Width", Range(0.05, 1)) = 0.1
		_ShineGlow ("Shine Glow", Range(0, 100)) = 1
		_ShineMask ("Shine Mask", 2D) = "white" {}
		_GlitchSize ("Glitch Size", Range(0.25, 5)) = 1
		_ZTestMode ("Z Test Mode", Float) = 4
		[HideInInspector] _MinXUV ("_MinXUV", Range(0, 1)) = 0
		[HideInInspector] _MaxXUV ("_MaxXUV", Range(0, 1)) = 1
		[HideInInspector] _MinYUV ("_MinYUV", Range(0, 1)) = 0
		[HideInInspector] _MaxYUV ("_MaxYUV", Range(0, 1)) = 1
		[HideInInspector] _RandomSeed ("_MaxYUV", Range(0, 10000)) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Sprites/Default"
	//CustomEditor "AllIn1SpriteShader"
}