// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skybox"
{
	Properties
	{
		_DayBottomColor("DayBottomColor", Color) = (0.9137255,0.2092025,0.1254902,1)
		_DayTopColor("DayTopColor", Color) = (0.4198113,0.6890934,1,0)
		_Noise1("Noise1", 2D) = "white" {}
		_ScrollSpeed1("ScrollSpeed1", Float) = 1
		_ScrollSpeed3("ScrollSpeed2", Float) = 0.1
		_Scale1("Scale1", Vector) = (1,1,0,0)
		_Scale2("Scale2", Vector) = (0.5,0.5,0,0)
		_BaseNoise("BaseNoise", 2D) = "white" {}
		_CloudsCutoff("CloudsCutoff", Float) = 1
		_CloudsSmoothness("CloudsSmoothness", Float) = 1
		_HorizonWidth("HorizonWidth", Float) = 0
		_HorizonBrightness("HorizonBrightness", Float) = 0
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
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _DayBottomColor;
		uniform float4 _DayTopColor;
		uniform float _HorizonWidth;
		uniform float _HorizonBrightness;
		uniform float _CloudsCutoff;
		uniform float _CloudsSmoothness;
		uniform sampler2D _BaseNoise;
		uniform float2 _Scale1;
		uniform float _ScrollSpeed1;
		uniform sampler2D _Noise1;
		uniform float2 _Scale2;
		uniform float _ScrollSpeed3;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float temp_output_83_0 = abs( ( i.uv_texcoord.y * _HorizonWidth ) );
			float temp_output_88_0 = saturate( ( temp_output_83_0 * ( i.uv_texcoord.y + _HorizonBrightness ) ) );
			float4 lerpResult2 = lerp( _DayBottomColor , _DayTopColor , temp_output_88_0);
			float4 temp_cast_0 = (1.0).xxxx;
			float4 temp_cast_1 = (_CloudsCutoff).xxxx;
			float4 temp_cast_2 = (( _CloudsCutoff + _CloudsSmoothness )).xxxx;
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_16_0 = ( (ase_worldPos).xz / ase_worldPos.y );
			float temp_output_77_0 = ( _Time.y / 100.0 );
			float4 tex2DNode35 = tex2D( _BaseNoise, (temp_output_16_0*_Scale1 + ( temp_output_77_0 * _ScrollSpeed1 )) );
			float4 smoothstepResult62 = smoothstep( temp_cast_1 , temp_cast_2 , saturate( ( ( ( tex2DNode35 * tex2D( _Noise1, (( tex2DNode35 + float4( temp_output_16_0, 0.0 , 0.0 ) )*float4( _Scale2, 0.0 , 0.0 ) + ( temp_output_77_0 * _ScrollSpeed3 )).rg ) ) * temp_output_88_0 ) * 20.0 ) ));
			float4 lerpResult55 = lerp( saturate( lerpResult2 ) , temp_cast_0 , saturate( smoothstepResult62 ));
			o.Emission = saturate( lerpResult55 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1812.71,85.14816;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-875.3432,557.7968;Inherit;True;Property;_BaseNoise;BaseNoise;7;0;Create;True;0;0;0;False;0;False;-1;8e665e4e13911c242a7b664835b47a52;be03ab7a299e4ae4e90631c9bd5b1ef2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;38;-1362.097,770.0436;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1624.854,904.7853;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;32;-1842.48,790.031;Inherit;False;Property;_Scale1;Scale1;5;0;Create;True;0;0;0;False;0;False;1,1;0.1,0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;30;-1871.854,985.7853;Inherit;False;Property;_ScrollSpeed1;ScrollSpeed1;3;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1204.6,1540.34;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;42;-933.3334,1456.974;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;1,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1451.6,1621.34;Inherit;False;Property;_ScrollSpeed3;ScrollSpeed2;4;0;Create;True;0;0;0;False;0;False;0.1;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;44;-1422.226,1425.586;Inherit;False;Property;_Scale2;Scale2;6;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.3,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-1035.017,1247.348;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;56;2032.815,287.1436;Inherit;False;Constant;_Float1;Float 1;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;2566.837,388.2567;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-2412.82,1270.995;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-2931.513,1247.595;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;20;-2700.355,1248.294;Inherit;True;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-88.71667,893.5389;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;62;1161.799,1091.05;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.2735849,0.2735849,0.2735849,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;69;581.567,1254.537;Inherit;False;Property;_CloudsSmoothness;CloudsSmoothness;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;70;914.0136,1228.766;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;617.5918,1139.159;Inherit;False;Property;_CloudsCutoff;CloudsCutoff;8;0;Create;True;0;0;0;False;0;False;1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;72;1905.367,1095.612;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;21;-628.0013,1434.538;Inherit;True;Property;_Noise1;Noise1;2;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;409a2f73b7536004cb20e19205befaab;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;71;1587.585,1225.234;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;76;387.0898,1090.516;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;73;859.3431,903.8959;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;33;-2392.006,891.1597;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;77;-2064.635,901.059;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2368.838,969.9587;Inherit;False;Constant;_Float2;Float 2;11;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;2275.908,230.7867;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;80;2803.421,380.7656;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Skybox;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;1;False;;1;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;547.5432,838.4311;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;236.8454,839.9161;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1411.194,140.6926;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1685.733,242.8073;Inherit;False;Property;_HorizonWidth;HorizonWidth;10;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;83;-1181.856,137.3445;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-1719.211,333.2039;Inherit;False;Property;_HorizonBrightness;HorizonBrightness;11;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-1439.654,297.5149;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;65;-809.5891,132.8168;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-534.6536,267.5149;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2;-124.7944,-178.3341;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-548.5994,-261.533;Inherit;False;Property;_DayTopColor;DayTopColor;1;0;Create;True;0;0;0;False;0;False;0.4198113,0.6890934,1,0;0.3066035,0.5174669,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-553.8004,-88.63333;Inherit;False;Property;_DayBottomColor;DayBottomColor;0;0;Create;True;0;0;0;False;0;False;0.9137255,0.2092025,0.1254902,1;0.504717,0.8426749,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;8;163.7999,-182.2326;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;88;-339.7567,271.9625;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;35;1;38;0
WireConnection;38;0;16;0
WireConnection;38;1;32;0
WireConnection;38;2;37;0
WireConnection;37;0;77;0
WireConnection;37;1;30;0
WireConnection;43;0;77;0
WireConnection;43;1;45;0
WireConnection;42;0;63;0
WireConnection;42;1;44;0
WireConnection;42;2;43;0
WireConnection;63;0;35;0
WireConnection;63;1;16;0
WireConnection;57;0;55;0
WireConnection;16;0;20;0
WireConnection;16;1;9;2
WireConnection;20;0;9;0
WireConnection;66;0;35;0
WireConnection;66;1;21;0
WireConnection;62;0;73;0
WireConnection;62;1;68;0
WireConnection;62;2;70;0
WireConnection;70;0;68;0
WireConnection;70;1;69;0
WireConnection;72;0;62;0
WireConnection;21;1;42;0
WireConnection;71;0;62;0
WireConnection;73;0;75;0
WireConnection;77;0;33;0
WireConnection;77;1;78;0
WireConnection;55;0;8;0
WireConnection;55;1;56;0
WireConnection;55;2;72;0
WireConnection;80;2;57;0
WireConnection;75;0;67;0
WireConnection;75;1;76;0
WireConnection;67;0;66;0
WireConnection;67;1;88;0
WireConnection;81;0;7;2
WireConnection;81;1;82;0
WireConnection;83;0;81;0
WireConnection;86;0;7;2
WireConnection;86;1;85;0
WireConnection;65;0;83;0
WireConnection;87;0;83;0
WireConnection;87;1;86;0
WireConnection;2;0;5;0
WireConnection;2;1;6;0
WireConnection;2;2;88;0
WireConnection;8;0;2;0
WireConnection;88;0;87;0
ASEEND*/
//CHKSM=F98CF5AA09718CE6778BA9C9EFBCF99BFD63E98D