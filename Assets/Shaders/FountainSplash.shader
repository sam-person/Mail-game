// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/FountainSplash"
{
	Properties
	{
		_Tiling1("Tiling1", Vector) = (2,2,0,0)
		_Noise1("Noise1", 2D) = "white" {}
		_Speed1("Speed1", Vector) = (1,1,0,0)
		_MaskStrength("MaskStrength", Range( 0 , 0.1)) = 0.1
		_Float6("Float 6", Float) = 10
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
			half filler;
		};

		uniform sampler2D _Noise1;
		uniform float2 _Tiling1;
		uniform float2 _Speed1;
		uniform float _Float6;
		uniform float _MaskStrength;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 uv_TexCoord6 = v.texcoord.xy * _Tiling1 + ( _Time.y * _Speed1 );
			float4 lerpResult23 = lerp( tex2Dlod( _Noise1, float4( uv_TexCoord6, 0, 0.0) ) , float4( 0,0,0,0 ) , ( v.color.r * _Float6 ));
			v.vertex.xyz += ( float4( ase_vertexNormal , 0.0 ) * ( lerpResult23 * _MaskStrength ) ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 temp_cast_0 = (1.0).xxx;
			o.Albedo = temp_cast_0;
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/FountainSplash;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-213.0748,85.53156;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-210.2983,12.87384;Inherit;False;Constant;_Float7;Float 7;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-213.0748,156.5316;Inherit;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1;-2872.219,-364.169;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-3177.22,-448.1692;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;3;-3173.22,-343.169;Inherit;False;Property;_Speed1;Speed1;3;0;Create;True;0;0;0;False;0;False;1,1;0.5,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;4;-2908.219,-565.169;Inherit;False;Property;_Tiling1;Tiling1;1;0;Create;True;0;0;0;False;0;False;2,2;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;5;-2284.599,-391.6552;Inherit;True;Property;_Noise1;Noise1;2;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-2636.581,-413.4661;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-932.7729,409.2629;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-602.446,311.7907;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;49;-964.5443,207.9326;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;23;-1436.984,-88.17935;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1273.49,432.761;Inherit;False;Property;_MaskStrength;MaskStrength;4;0;Create;True;0;0;0;False;0;False;0.1;0.0171;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;21;-1959.158,-57.89016;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1644.496,-33.67213;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1924.548,118.8566;Inherit;False;Property;_Float6;Float 6;5;0;Create;True;0;0;0;False;0;False;10;1.2;0;0;0;1;FLOAT;0
WireConnection;0;0;55;0
WireConnection;0;3;16;0
WireConnection;0;4;17;0
WireConnection;0;11;50;0
WireConnection;1;0;2;0
WireConnection;1;1;3;0
WireConnection;5;1;6;0
WireConnection;6;0;4;0
WireConnection;6;1;1;0
WireConnection;46;0;23;0
WireConnection;46;1;47;0
WireConnection;50;0;49;0
WireConnection;50;1;46;0
WireConnection;23;0;5;0
WireConnection;23;2;53;0
WireConnection;53;0;21;1
WireConnection;53;1;54;0
ASEEND*/
//CHKSM=F05AD8EBC389205E6EA5BDB0286618826DD47723