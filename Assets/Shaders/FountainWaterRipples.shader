// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterFountainRipples"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_ColorVoronoi("ColorVoronoi", Color) = (1,1,1,0)
		_Tiling1("Tiling1", Vector) = (1,1,0,0)
		_Tiling2("Tiling2", Vector) = (1,1,0,0)
		_Speed1("Speed1", Vector) = (0.5,0.5,0,0)
		_Speed2("Speed2", Vector) = (0.5,0.5,0,0)
		_WaterRipples("WaterRipples", 2D) = "white" {}
		_Float1("Float 1", Float) = 0.5
		_Gradient("Gradient", 2D) = "white" {}
		_Noise3("Noise3", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorVoronoi;
		uniform sampler2D _WaterRipples;
		uniform float2 _Tiling1;
		uniform float2 _Speed1;
		uniform float2 _Tiling2;
		uniform float2 _Speed2;
		uniform float _Float1;
		uniform sampler2D _Gradient;
		uniform float4 _Gradient_ST;
		uniform sampler2D _Noise3;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _ColorVoronoi.rgb;
			o.Alpha = 1;
			float2 uv_TexCoord23 = i.uv_texcoord * _Tiling1 + ( _Time.y * _Speed1 );
			float2 uv_TexCoord50 = i.uv_texcoord * _Tiling2 + ( _Time.y * _Speed2 );
			float clampResult57 = clamp( _Float1 , 0.0 , 1.0 );
			float clampResult69 = clamp( ( ( ( tex2D( _WaterRipples, uv_TexCoord23 ).g + tex2D( _WaterRipples, uv_TexCoord50 ).r ) - _Float1 ) / clampResult57 ) , 0.0 , 1.0 );
			float2 uv_Gradient = i.uv_texcoord * _Gradient_ST.xy + _Gradient_ST.zw;
			float4 tex2DNode61 = tex2D( _Gradient, uv_Gradient );
			float4 clampResult79 = clamp( ( tex2DNode61 / ( tex2D( _Noise3, uv_TexCoord50 ) * 4.28 ) ) , float4( 0,0,0,0 ) , float4( 0.9056604,0.9056604,0.9056604,0 ) );
			clip( ( clampResult69 + clampResult79 ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.SamplerNode;45;-1398.727,73.40587;Inherit;True;Property;_WaterRipples;WaterRipples;6;0;Create;True;0;0;0;False;0;False;-1;6c2c5e94abbfa9d4f97b8faad68b9119;6c2c5e94abbfa9d4f97b8faad68b9119;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-987.8038,190.8414;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;59;-672,192;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;56;-352,192;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;57;-656,432;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-880,432;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;0.5;-0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1873.306,-42.29074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2178.307,-126.2908;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1637.667,-91.58786;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;13;-1909.306,-243.2908;Inherit;False;Property;_Tiling1;Tiling1;2;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;14;-2174.307,-21.29075;Inherit;False;Property;_Speed1;Speed1;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1966.353,411.8348;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;-1730.714,362.5378;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;51;-2002.354,210.835;Inherit;False;Property;_Tiling2;Tiling2;3;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;47;-2199.754,431.5349;Inherit;False;Property;_Speed2;Speed2;5;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;53;-1405.704,346.5419;Inherit;True;Property;_WaterRipples1;WaterRipples;6;0;Create;True;0;0;0;False;0;False;-1;6c2c5e94abbfa9d4f97b8faad68b9119;6c2c5e94abbfa9d4f97b8faad68b9119;True;0;False;white;Auto;False;Instance;45;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;69;-92.86316,191.197;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;75;-639.7727,986.2792;Inherit;True;Property;_Noise3;Noise3;9;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;78;-44.62079,681.2225;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;79;236.2792,676.8226;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0.9056604,0.9056604,0.9056604,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1239.552,2.699015;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterFountainRipples;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;False;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.ColorNode;7;930.4343,-245.0252;Inherit;False;Property;_ColorVoronoi;ColorVoronoi;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;67;733.1264,208.5459;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;80;-435.4277,671.9406;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;4.08;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-632.1235,867.4073;Inherit;False;Property;_GradientPower;GradientPower;10;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-539.9137,1183.794;Inherit;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;0;False;0;False;4.28;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-302.0139,984.8942;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;61;-759.3893,671.6249;Inherit;True;Property;_Gradient;Gradient;8;0;Create;True;0;0;0;False;0;False;-1;a1c529c9412d646488c90267657c736b;660538eebd1b9fa46a71015a6c9fd4e6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;45;1;23;0
WireConnection;52;0;45;2
WireConnection;52;1;53;1
WireConnection;59;0;52;0
WireConnection;59;1;55;0
WireConnection;56;0;59;0
WireConnection;56;1;57;0
WireConnection;57;0;55;0
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;23;0;13;0
WireConnection;23;1;16;0
WireConnection;46;0;15;0
WireConnection;46;1;47;0
WireConnection;50;0;51;0
WireConnection;50;1;46;0
WireConnection;53;1;50;0
WireConnection;69;0;56;0
WireConnection;75;1;50;0
WireConnection;78;0;61;0
WireConnection;78;1;82;0
WireConnection;79;0;78;0
WireConnection;0;0;7;0
WireConnection;0;10;67;0
WireConnection;67;0;69;0
WireConnection;67;1;79;0
WireConnection;80;0;61;0
WireConnection;80;1;81;0
WireConnection;82;0;75;0
WireConnection;82;1;83;0
ASEEND*/
//CHKSM=A06B2BD258813CCB078D0547696FEF08B508E282