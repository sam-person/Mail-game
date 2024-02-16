// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skybox"
{
	Properties
	{
		_CloudColor("CloudColor", Color) = (1,1,1,0)
		_DayTopColor("DayTopColor", Color) = (0.4198113,0.6890934,1,0)
		_DayBottomColor("DayBottomColor", Color) = (0.9137255,0.2092025,0.1254902,1)
		_FogColor("FogColor", Color) = (1,0.4575472,0.4575472,1)
		_FogHeight("FogHeight", Float) = 0
		_FogOpacity("FogOpacity", Range( 0 , 1)) = 0.5
		_HorizonWidth("HorizonWidth", Float) = 0
		_CloudsOpacity("CloudsOpacity", Range( 0 , 1)) = 0
		_CloudsHorizonHeight("CloudsHorizonHeight", Range( 0 , 30)) = 0
		[Toggle]_CloudsTouchHorizon("CloudsTouchHorizon", Float) = 0
		_CloudHeight("CloudHeight", Range( 0 , 0.99)) = 0
		_CloudsCutoff("CloudsCutoff", Range( 0 , 0.2)) = 1
		_BaseNoise("BaseNoise", 2D) = "white" {}
		_BaseNoiseScale("BaseNoiseScale", Vector) = (1,1,0,0)
		_DistortionScale("DistortionScale", Vector) = (0.5,0.5,0,0)
		_BaseNoise_ScrollSpeed("BaseNoise_ScrollSpeed", Float) = 1
		_Distortion_ScrollSpeed("Distortion_ScrollSpeed", Float) = 0.1
		[Toggle]_Stars("Stars", Float) = 0
		_StarBloom("StarBloom", Float) = 0
		_StarTiling("StarTiling", Vector) = (80,20,0,0)
		_StarsNoise("StarsNoise", 2D) = "white" {}
		_StarsTwinkleNoise("StarsTwinkleNoise", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#define ASE_USING_SAMPLING_MACROS 1
		#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
		#else//ASE Sampling Macros
		#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
		#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex2Dlod(tex,float4(coord,0,lod))
		#endif//ASE Sampling Macros

		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float4 _DayBottomColor;
		uniform float _Stars;
		uniform float4 _DayTopColor;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_StarsNoise);
		uniform float2 _StarTiling;
		SamplerState sampler_StarsNoise;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_StarsTwinkleNoise);
		SamplerState sampler_StarsTwinkleNoise;
		uniform float _StarBloom;
		uniform float _HorizonWidth;
		uniform float4 _CloudColor;
		uniform float _CloudsCutoff;
		uniform float _CloudsTouchHorizon;
		UNITY_DECLARE_TEX2D_NOSAMPLER(_BaseNoise);
		uniform float2 _BaseNoiseScale;
		uniform float _BaseNoise_ScrollSpeed;
		SamplerState sampler_BaseNoise;
		uniform float2 _DistortionScale;
		uniform float _Distortion_ScrollSpeed;
		uniform float _CloudsHorizonHeight;
		uniform float _CloudHeight;
		uniform float _CloudsOpacity;
		uniform float4 _FogColor;
		uniform float _FogHeight;
		uniform float _FogOpacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 normalizeResult90 = normalize( ase_worldPos );
			float3 break91 = normalizeResult90;
			float2 appendResult97 = (float2(( atan2( break91.x , break91.z ) / 6.28318548202515 ) , ( asin( break91.y ) / ( UNITY_PI / 2.0 ) )));
			float temp_output_77_0 = ( _Time.y / 100.0 );
			float4 lerpResult2 = lerp( _DayBottomColor , (( _Stars )?( ( _DayTopColor + ( saturate( ( 1.0 - ( SAMPLE_TEXTURE2D( _StarsNoise, sampler_StarsNoise, (appendResult97*_StarTiling + 0.0) ) + ( 1.0 - SAMPLE_TEXTURE2D( _StarsTwinkleNoise, sampler_StarsTwinkleNoise, (appendResult97*50.0 + ( temp_output_77_0 * float2( 1,5 ) )) ) ) ) ) ) * _StarBloom ) ) ):( _DayTopColor )) , saturate( ( abs( ( i.uv_texcoord.y * _HorizonWidth ) ) * ( i.uv_texcoord.y + 1.0 ) ) ));
			float4 temp_cast_0 = (_CloudsCutoff).xxxx;
			float4 temp_cast_1 = (( _CloudsCutoff + 0.0 )).xxxx;
			float4 tex2DNode35 = SAMPLE_TEXTURE2D_LOD( _BaseNoise, sampler_BaseNoise, (appendResult97*_BaseNoiseScale + ( temp_output_77_0 * _BaseNoise_ScrollSpeed )), 0.0 );
			float4 temp_output_66_0 = ( tex2DNode35 * SAMPLE_TEXTURE2D_LOD( _BaseNoise, sampler_BaseNoise, (( ( tex2DNode35 * 0.5 ) + float4( appendResult97, 0.0 , 0.0 ) )*float4( _DistortionScale, 0.0 , 0.0 ) + ( temp_output_77_0 * _Distortion_ScrollSpeed )).rg, 0.0 ) );
			float smoothstepResult114 = smoothstep( saturate( ( _CloudHeight + 1.0 ) ) , _CloudHeight , ( i.uv_texcoord.y + 0.1 ));
			float4 smoothstepResult62 = smoothstep( temp_cast_0 , temp_cast_1 , saturate( ( ( (( _CloudsTouchHorizon )?( temp_output_66_0 ):( ( temp_output_66_0 * saturate( ( abs( ( i.uv_texcoord.y * _CloudsHorizonHeight ) ) * ( i.uv_texcoord.y + 1.0 ) ) ) ) )) * 20.0 ) * smoothstepResult114 ) ));
			float4 lerpResult55 = lerp( lerpResult2 , _CloudColor , ( saturate( smoothstepResult62 ) * _CloudsOpacity ));
			float4 lerpResult126 = lerp( lerpResult55 , _FogColor , saturate( ( pow( ( 1.0 - i.uv_texcoord.y ) , _FogHeight ) * _FogOpacity ) ));
			o.Emission = lerpResult126.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.CommentaryNode;174;-1116.785,-1191.027;Inherit;False;2009.521;572.767;Stars;14;144;146;167;171;154;170;148;172;168;151;173;161;163;162;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;139;1328.161,992.2933;Inherit;False;1074.306;379.299;StylizeClouds;6;72;62;69;70;68;73;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;138;2776.476,938.1381;Inherit;False;1359.944;636.144;Fog;9;130;134;135;137;133;126;127;128;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;122;-1945.344,545.8228;Inherit;False;844.8827;881.1839;Scrolling;12;38;37;32;30;43;42;44;63;77;45;141;142;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;121;-3849.952,756.5852;Inherit;False;1316.917;506.9994;SkyboxUV;11;90;93;100;99;89;91;96;94;98;101;97;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;-3519.262,-790.0388;Inherit;False;1700.954;516.3668;Gradient;8;7;81;82;83;85;86;87;88;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;119;1376.199,247.1992;Inherit;False;1499.469;527.2304;Color;7;55;2;6;5;143;150;175;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;118;-3485.03,2276.193;Inherit;False;801.6929;327.4;Horizon;3;16;20;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;117;-302.1664,1728.599;Inherit;False;933.3251;471.8256;Mask Cloud Height;7;108;109;114;110;107;112;113;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;-15.16664,2024.425;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;114;379.1587,1876.599;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;110;127.8333,2025.425;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;112;-195.6066,1785.068;Inherit;False;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;113;129.1589,1778.599;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-2916.337,2349.593;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-3203.872,2326.892;Inherit;True;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-3435.03,2326.193;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;55;2614.668,520.4293;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;1431.4,297.199;Inherit;False;Property;_DayTopColor;DayTopColor;1;0;Create;True;0;0;0;False;0;False;0.4198113,0.6890934,1,0;0.3130562,0.5566926,0.990566,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;1426.199,470.0987;Inherit;False;Property;_DayBottomColor;DayBottomColor;2;0;Create;True;0;0;0;False;0;False;0.9137255,0.2092025,0.1254902,1;0.4481131,0.7251507,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;90;-3575.952,902.5852;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ATan2OpNode;93;-3191.953,806.5852;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;100;-3191.953,1126.585;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;99;-3383.953,1126.585;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;89;-3799.952,902.5852;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;91;-3383.953,902.5852;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TauNode;96;-3159.953,918.5852;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;94;-2983.953,806.5852;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ASinOpNode;98;-3191.953,1030.585;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;101;-3047.953,1062.585;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;97;-2766.035,923.975;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;38;-1385.587,595.8229;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1648.344,730.5646;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1613.728,1233.007;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;42;-1342.461,1149.641;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;-2162.945,928.86;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2139.777,1007.659;Inherit;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;77;-1986.301,956.3781;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;124;-2056.657,1498.284;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;547.5432,838.4311;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;956.7052,1049.204;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;80;4925.813,274.5682;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Skybox;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;1;False;;1;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;True;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;3451.865,1202.871;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;137;3700.7,1206.179;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;126;3875.42,989.7202;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;72;2224.467,1091.241;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;62;1958.393,1095.105;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2735849,0.2735849,0.2735849,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;1710.607,1232.821;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;73;1436.123,1042.293;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;133;3374.898,988.1381;Inherit;False;Property;_FogColor;FogColor;3;0;Create;True;0;0;0;False;0;False;1,0.4575472,0.4575472,1;0.3176471,0.9501659,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;69;1378.161,1258.592;Inherit;False;Constant;_CloudsSmoothness;CloudsSmoothness;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-339.3319,870.7557;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;140;271.0602,877.2568;Inherit;False;Property;_CloudsTouchHorizon;CloudsTouchHorizon;9;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-252.1664,2087.425;Inherit;False;Constant;_CloudEdge;CloudEdge;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;1414.186,1143.214;Inherit;False;Property;_CloudsCutoff;CloudsCutoff;11;0;Create;True;0;0;0;False;0;False;1;0.1;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-1488.192,995.9029;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-1629.408,898.8139;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.5;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-228.8409,1900.599;Inherit;False;Property;_CloudHeight;CloudHeight;10;0;Create;True;0;0;0;False;0;False;0;0.9;0;0.99;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;135;3138.865,1180.871;Inherit;False;Property;_FogOpacity;FogOpacity;5;0;Create;True;0;0;0;False;0;False;0.5;0.109;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;143;2335.692,564.6824;Inherit;False;Property;_CloudColor;CloudColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;144;-677.8049,-1125.375;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;146;-935.581,-1101.182;Inherit;False;Property;_StarTiling;StarTiling;19;0;Create;True;0;0;0;False;0;False;80,20;5,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScaleAndOffsetNode;167;-715.7629,-908.5234;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;171;-65.77956,-946.9064;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;170;172.0645,-975.0547;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;148;319.324,-971.7954;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-900.503,-880.6236;Inherit;False;Constant;_Float3;Float 3;21;0;Create;True;0;0;0;False;0;False;50;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;151;-895.3406,-806.3936;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;173;-1066.785,-779.26;Inherit;False;Constant;_Vector0;Vector 0;22;0;Create;True;0;0;0;False;0;False;1,5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SaturateNode;161;483.6426,-971.1927;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;163;478.6951,-885.7773;Inherit;False;Property;_StarBloom;StarBloom;18;0;Create;True;0;0;0;False;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;162;659.7359,-970.7084;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;2;2107.507,475.2343;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;175;1840.536,396.5288;Inherit;False;Property;_Stars;Stars;17;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;150;1718.287,287.0678;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;154;-431.5212,-1141.027;Inherit;True;Property;_StarsNoise;StarsNoise;20;0;Create;True;0;0;0;False;0;False;-1;be03ab7a299e4ae4e90631c9bd5b1ef2;5c1dcbd1539173d4494282a721c60157;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;172;-434.1986,-946.5666;Inherit;True;Property;_StarsTwinkleNoise;StarsTwinkleNoise;21;0;Create;True;0;0;0;False;0;False;-1;3c17c402897ee994d8e590f3223cf83e;3c17c402897ee994d8e590f3223cf83e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;6.278179,872.2526;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;35;-911.0082,564.9299;Inherit;True;Property;_BaseNoise;BaseNoise;12;0;Create;True;0;0;0;False;0;False;-1;8e665e4e13911c242a7b664835b47a52;409a2f73b7536004cb20e19205befaab;True;0;False;white;Auto;False;Object;-1;MipLevel;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-906.7513,1127.85;Inherit;True;Property;_NoiseDistortion;NoiseDistortion;12;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;409a2f73b7536004cb20e19205befaab;True;0;False;white;Auto;False;Instance;35;MipLevel;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;142;-1854.704,931.285;Inherit;False;Constant;_DistortionStrength;DistortionStrength;15;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1895.344,811.5646;Inherit;False;Property;_BaseNoise_ScrollSpeed;BaseNoise_ScrollSpeed;15;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1884.218,1261.154;Inherit;False;Property;_Distortion_ScrollSpeed;Distortion_ScrollSpeed;16;0;Create;True;0;0;0;False;0;False;0.1;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;44;-1649.353,1103.952;Inherit;False;Property;_DistortionScale;DistortionScale;14;0;Create;True;0;0;0;False;0;False;0.5,0.5;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;32;-1681.371,606.7104;Inherit;False;Property;_BaseNoiseScale;BaseNoiseScale;13;0;Create;True;0;0;0;False;0;False;1,1;1,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PowerNode;130;3188.933,1274.703;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;127;2821.648,1276.826;Inherit;True;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;128;2995.256,1274.343;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;357.0898,997.516;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-3463.243,-741.0584;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;85;-2963.557,-472.0832;Inherit;False;Constant;_HorizonBrightness;HorizonBrightness;11;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2933.549,-544.0994;Inherit;False;Property;_HorizonWidth;HorizonWidth;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-2714.54,-604.5945;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-2713,-507.7721;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;83;-2562.203,-603.9426;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-2431.208,-531.6718;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;88;-2278.312,-529.2242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;178;-2884.709,-99.13318;Inherit;False;Constant;_HorizonBrightness1;HorizonBrightness;11;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;180;-2635.692,-231.6445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-2634.152,-134.8221;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;182;-2483.355,-230.9926;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-2352.36,-158.7218;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;184;-2199.464,-156.2742;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-2914.5,-186.7495;Inherit;False;Property;_CloudsHorizonHeight;CloudsHorizonHeight;8;0;Create;True;0;0;0;False;0;False;0;6.1;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;185;2442.653,935.2146;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;186;2115.653,869.2146;Inherit;False;Property;_CloudsOpacity;CloudsOpacity;7;0;Create;True;0;0;0;False;0;False;0;0.084;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;131;2819.816,1495.101;Inherit;False;Property;_FogHeight;FogHeight;4;0;Create;True;0;0;0;False;0;False;0;50;0;0;0;1;FLOAT;0
WireConnection;109;0;107;0
WireConnection;109;1;108;0
WireConnection;114;0;113;0
WireConnection;114;1;110;0
WireConnection;114;2;107;0
WireConnection;110;0;109;0
WireConnection;112;0;124;0
WireConnection;113;0;112;0
WireConnection;16;0;20;0
WireConnection;16;1;9;2
WireConnection;20;0;9;0
WireConnection;55;0;2;0
WireConnection;55;1;143;0
WireConnection;55;2;185;0
WireConnection;90;0;89;0
WireConnection;93;0;91;0
WireConnection;93;1;91;2
WireConnection;100;0;99;0
WireConnection;91;0;90;0
WireConnection;94;0;93;0
WireConnection;94;1;96;0
WireConnection;98;0;91;1
WireConnection;101;0;98;0
WireConnection;101;1;100;0
WireConnection;97;0;94;0
WireConnection;97;1;101;0
WireConnection;38;0;97;0
WireConnection;38;1;32;0
WireConnection;38;2;37;0
WireConnection;37;0;77;0
WireConnection;37;1;30;0
WireConnection;43;0;77;0
WireConnection;43;1;45;0
WireConnection;42;0;63;0
WireConnection;42;1;44;0
WireConnection;42;2;43;0
WireConnection;77;0;33;0
WireConnection;77;1;78;0
WireConnection;124;0;7;2
WireConnection;75;0;140;0
WireConnection;75;1;76;0
WireConnection;115;0;75;0
WireConnection;115;1;114;0
WireConnection;80;2;126;0
WireConnection;134;0;130;0
WireConnection;134;1;135;0
WireConnection;137;0;134;0
WireConnection;126;0;55;0
WireConnection;126;1;133;0
WireConnection;126;2;137;0
WireConnection;72;0;62;0
WireConnection;62;0;73;0
WireConnection;62;1;68;0
WireConnection;62;2;70;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;73;0;115;0
WireConnection;66;0;35;0
WireConnection;66;1;21;0
WireConnection;140;0;67;0
WireConnection;140;1;66;0
WireConnection;63;0;141;0
WireConnection;63;1;97;0
WireConnection;141;0;35;0
WireConnection;141;1;142;0
WireConnection;144;0;97;0
WireConnection;144;1;146;0
WireConnection;167;0;97;0
WireConnection;167;1;168;0
WireConnection;167;2;151;0
WireConnection;171;0;172;0
WireConnection;170;0;154;0
WireConnection;170;1;171;0
WireConnection;148;0;170;0
WireConnection;151;0;77;0
WireConnection;151;1;173;0
WireConnection;161;0;148;0
WireConnection;162;0;161;0
WireConnection;162;1;163;0
WireConnection;2;0;5;0
WireConnection;2;1;175;0
WireConnection;2;2;88;0
WireConnection;175;0;6;0
WireConnection;175;1;150;0
WireConnection;150;0;6;0
WireConnection;150;1;162;0
WireConnection;154;1;144;0
WireConnection;172;1;167;0
WireConnection;67;0;66;0
WireConnection;67;1;184;0
WireConnection;35;1;38;0
WireConnection;21;1;42;0
WireConnection;130;0;128;0
WireConnection;130;1;131;0
WireConnection;127;0;7;2
WireConnection;128;0;127;0
WireConnection;81;0;7;2
WireConnection;81;1;82;0
WireConnection;86;0;7;2
WireConnection;86;1;85;0
WireConnection;83;0;81;0
WireConnection;87;0;83;0
WireConnection;87;1;86;0
WireConnection;88;0;87;0
WireConnection;180;0;7;2
WireConnection;180;1;179;0
WireConnection;181;0;7;2
WireConnection;181;1;178;0
WireConnection;182;0;180;0
WireConnection;183;0;182;0
WireConnection;183;1;181;0
WireConnection;184;0;183;0
WireConnection;185;0;72;0
WireConnection;185;1;186;0
ASEEND*/
//CHKSM=56B4C4909953C04C8375E8E98B8896D67ED0B753