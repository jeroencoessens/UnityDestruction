Shader "Custom/CelShaded_ShadowDetails" {
	Properties {

		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 0.03)) = .005

		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 

		_ShadowDetailsColor ("Shadow Details Color", Color) = (1,1,1,1)
		_ShadowDetailsMap ("Shadow Details Map(A)", 2D) = "black" {}
		

	}


	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf ToonRamp fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct SurfaceOutputCustom // turns out you can do this
		{
		    fixed3 Albedo;
		    fixed3 Normal;
		    fixed3 Emission;
		    half Specular;
		    fixed Gloss;
		    fixed Alpha;
		 
		    half ShadowDetails; // <- new guy
		};

		struct Input {
			float2 uv_MainTex;
			float2 uv_ShadowDetailsMap;
		};


		sampler2D _MainTex;
		sampler2D _Ramp;
		sampler2D _ShadowDetailsMap;

		float4 _ShadowDetailsColor;
		float4 _Color;

		// in order

		// 1st
		void surf (Input IN, inout SurfaceOutputCustom o) {

			o.ShadowDetails = tex2D(_ShadowDetailsMap, IN.uv_ShadowDetailsMap).a;  // for 2nd

			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

		}

		// 2nd
		// custom lighting function that uses a texture ramp based
		// on angle between light direction and normal
		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp (SurfaceOutputCustom o, half3 lightDir, half atten) // light attenuation
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif

			half d = dot (o.Normal, lightDir)*0.5 + 0.5;
			half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;

			half lumens = ramp * (atten * 2); // brightness

			half4 c;
			c.rgb = o.Albedo * _LightColor0.rgb * lumens;

			half invert = max(0,sign(lumens)); // add emission to the darkside
			invert = 1-invert;              // lives up to the name
			c.rgb += _ShadowDetailsColor.rgb * o.ShadowDetails * invert; // add it
			c.a = 0;

			return c;
		}


		ENDCG

		// for the outline
		UsePass "Custom/Diffuse Outline/OUTLINE_PASS"
	} 
	
	Fallback "Diffuse"
}