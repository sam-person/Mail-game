// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WindFoliage"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_NoiseSmooth("Noise Smooth", 2D) = "white" {}
		_WindStrength("WindStrength", Range( 0 , 10)) = 10
		_Speed("Speed", Range( 0.001 , 1)) = 0.02
		_NoiseSize("NoiseSize", Range( 0.003 , 1)) = 0.008
		_NoiseJitterSize("NoiseJitterSize", Range( 0.003 , 1)) = 0.24
		_WindJitterSpeed("WindJitterSpeed", Float) = 0.08
		_WindJitterStrength("WindJitterStrength", Range( 0 , 0.05)) = 0.001
		[Toggle]_UseTextureColor("Use Texture Color", Float) = 1
		_Texture("Texture", 2D) = "white" {}
		_Color0("Color 0", Color) = (0.3098039,0.6901961,0.3843137,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _NoiseSmooth;
		uniform float _Speed;
		uniform float _NoiseSize;
		uniform float _WindStrength;
		uniform float _WindJitterSpeed;
		uniform float _NoiseJitterSize;
		uniform float _WindJitterStrength;
		uniform float _UseTextureColor;
		uniform float4 _Color0;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult3_g7 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner10_g7 = ( ( _Speed * _Time.y ) * float2( 1,1 ) + ( appendResult3_g7 * _NoiseSize ));
			float4 clampResult17_g7 = clamp( ( tex2Dlod( _NoiseSmooth, float4( panner10_g7, 0, 0.0) ) * _WindStrength ) , float4( 0.369,0.369,0.369,0 ) , float4( 1,0,0,0 ) );
			float2 panner1_g7 = ( ( _Time.y * _WindJitterSpeed ) * float2( 1,1 ) + ( appendResult3_g7 * _NoiseJitterSize ));
			float4 temp_output_89_0 = ( clampResult17_g7 * ( tex2Dlod( _NoiseSmooth, float4( panner1_g7, 0, 0.0) ) * _WindJitterStrength ) );
			v.vertex.xyz += temp_output_89_0.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float4 tex2DNode51 = tex2D( _Texture, uv_Texture );
			o.Albedo = (( _UseTextureColor )?( tex2DNode51 ):( _Color0 )).rgb;
			o.Alpha = 1;
			clip( tex2DNode51.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.Vector3Node;60;1064.077,-246.9821;Float;False;Constant;_Frontnormalvector;Front normal vector;4;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;61;1064.077,-86.98196;Float;False;Constant;_Backnormalvector;Back normal vector;4;0;Create;True;0;0;0;False;0;False;0,0,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SwitchByFaceNode;62;1352.077,-166.9821;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1669.679,531.8192;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;40;1417.679,595.9644;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;55;-719.6493,437.2939;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1420.572,72.3718;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1204.057,72.17081;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1259.421,352.9435;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;37;-1466.421,463.9435;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1157.649,566.2938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1590.057,354.171;Inherit;False;Property;_Speed;Speed;11;0;Create;True;0;0;0;False;0;False;1;0.004;0.001;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-905.6493,434.2939;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;36;-566.421,151.9434;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-352,128;Inherit;True;Property;_NoiseSmooth;Noise Smooth;9;0;Create;True;0;0;0;False;0;False;-1;62695c15a49d430abd2ae1471c72b1c7;62695c15a49d430abd2ae1471c72b1c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;56;-453.6493,412.2939;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;4;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-90.1358,414.2501;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;203.3575,108.5711;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1592.421,254.9435;Inherit;False;Property;_NoiseSize;NoiseSize;13;0;Create;True;0;0;0;False;0;False;0;0.102;0.003;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;81;432.8973,94.10146;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.369,0.369,0.369,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-131.8304,310.1248;Inherit;False;Property;_WindStrength;WindStrength;10;0;Create;True;0;0;0;False;0;False;2;3.18;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-447.8878,619.4302;Inherit;False;Property;_WindJitterStrength;WindJitterStrength;18;0;Create;True;0;0;0;False;0;False;0;0.0003;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1630.618,572.0236;Inherit;False;Property;_NoiseJitterSize;NoiseJitterSize;14;0;Create;True;0;0;0;False;0;False;0;0.03;0.003;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;89;1450.167,516.0527;Inherit;False;Wind;1;;7;360f3fb2567afa24a86771d20d9d7926;0;0;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;52;2012.64,16.44344;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WindFoliage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-1682.503,32.3914;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;723.3798,240.8631;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1591.649,673.2938;Inherit;False;Property;_WindJitterSpeed;WindJitterSpeed;17;0;Create;True;0;0;0;False;0;False;0;0.01;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;90;1659.29,-454.9015;Inherit;False;Property;_UseTextureColor;Use Texture Color;12;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;51;1327.433,-431.9023;Inherit;True;Property;_Texture;Texture;15;0;Create;True;0;0;0;False;0;False;-1;e89d64857446e014faf04ae1aa9c9485;04353a97b1fd511479cdc899340ef67a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;1391.5,-604.0786;Inherit;False;Property;_Color0;Color 0;16;0;Create;True;0;0;0;False;0;False;0.3098039,0.6901961,0.3843137,1;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;62;0;60;0
WireConnection;62;1;61;0
WireConnection;41;0;89;0
WireConnection;41;1;40;0
WireConnection;55;0;54;0
WireConnection;55;1;57;0
WireConnection;25;0;19;1
WireConnection;25;1;19;3
WireConnection;17;0;25;0
WireConnection;17;1;35;0
WireConnection;38;0;16;0
WireConnection;38;1;37;0
WireConnection;57;0;37;0
WireConnection;57;1;58;0
WireConnection;54;0;25;0
WireConnection;54;1;63;0
WireConnection;36;0;17;0
WireConnection;36;1;38;0
WireConnection;4;1;36;0
WireConnection;56;1;55;0
WireConnection;64;0;56;0
WireConnection;64;1;65;0
WireConnection;13;0;4;0
WireConnection;13;1;14;0
WireConnection;81;0;13;0
WireConnection;52;0;90;0
WireConnection;52;10;51;4
WireConnection;52;11;89;0
WireConnection;59;0;81;0
WireConnection;59;1;64;0
WireConnection;90;0;53;0
WireConnection;90;1;51;0
ASEEND*/
//CHKSM=95F4F6BF8E9AB11F119EE7B8736D8C3447750C0C