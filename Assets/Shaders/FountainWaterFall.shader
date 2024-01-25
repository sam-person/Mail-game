// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FountainWaterFall"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.62
		_Tiling1("Tiling1", Vector) = (1,0.3,0,0)
		_Noise1("Noise1", 2D) = "white" {}
		_Tiling2("Tiling2", Vector) = (1,0.2,0,0)
		_Speed1("Speed1", Vector) = (0,0.5,0,0)
		_Speed2("Speed2", Vector) = (0,0.6,0,0)
		_OutlineStrength("OutlineStrength", Float) = 0.5
		_StreamSolidity("StreamSolidity", Float) = 4.46
		_FountainGradient2("FountainGradient2", 2D) = "white" {}
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Noise1;
		uniform float2 _Tiling1;
		uniform float2 _Speed1;
		uniform float2 _Tiling2;
		uniform float2 _Speed2;
		uniform float _StreamSolidity;
		uniform sampler2D _FountainGradient2;
		uniform float4 _FountainGradient2_ST;
		uniform float _OutlineStrength;
		uniform float _Cutoff = 0.62;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color14 = IsGammaSpace() ? float4(0.4103774,0.7152898,1,0) : float4(0.1402577,0.4700716,1,0);
			float4 color13 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float2 uv_TexCoord3 = i.uv_texcoord * _Tiling1 + ( _Time.y * _Speed1 );
			float2 uv_TexCoord9 = i.uv_texcoord * _Tiling2 + ( _Time.y * _Speed2 );
			float2 uv_FountainGradient2 = i.uv_texcoord * _FountainGradient2_ST.xy + _FountainGradient2_ST.zw;
			float4 lerpResult85 = lerp( float4( 0,0,0,0 ) , ( ( tex2D( _Noise1, uv_TexCoord3 ) + tex2D( _Noise1, uv_TexCoord9 ) ) / _StreamSolidity ) , ( i.vertexColor.g + tex2D( _FountainGradient2, uv_FountainGradient2 ) ));
			float4 clampResult81 = clamp( lerpResult85 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 temp_cast_0 = (_OutlineStrength).xxxx;
			float4 clampResult26 = clamp( ( 1.0 - ( clampResult81 / ( clampResult81 - temp_cast_0 ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult16 = lerp( color14 , color13 , clampResult26);
			o.Albedo = lerpResult16.rgb;
			o.Alpha = 1;
			clip( clampResult81.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.ColorNode;14;1990.673,-673.2325;Inherit;False;Constant;_Color1;Color 0;6;0;Create;True;0;0;0;False;0;False;0.4103774,0.7152898,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;2268.033,-499.8819;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;13;1937.288,-487.8269;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;24;1202.771,-343.3692;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1;-1649.736,-68.15272;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;2;-1954.737,-152.1527;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;5;-1950.737,-47.15273;Inherit;False;Property;_Speed1;Speed1;4;0;Create;True;0;0;0;False;0;False;0,0.5;0,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;4;-1685.736,-269.1526;Inherit;False;Property;_Tiling1;Tiling1;1;0;Create;True;0;0;0;False;0;False;1,0.3;3,0.6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1744.46,441.3748;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;10;-1780.461,240.375;Inherit;False;Property;_Tiling2;Tiling2;3;0;Create;True;0;0;0;False;0;False;1,0.2;1,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;11;-1977.861,461.075;Inherit;False;Property;_Speed2;Speed2;5;0;Create;True;0;0;0;False;0;False;0,0.6;0,0.4;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;7;-1059.1,170.9133;Inherit;True;Property;_Noise2;Noise1;2;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;8e665e4e13911c242a7b664835b47a52;True;0;False;white;Auto;False;Instance;6;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;81;872.6044,80.54513;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;26;1976.19,-167.1907;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;23;1494.369,-166.2377;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;96;26.85083,88.55737;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-594.6775,331.376;Inherit;False;Property;_StreamSolidity;StreamSolidity;7;0;Create;True;0;0;0;False;0;False;4.46;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;85;496.2803,82.66395;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;34;1760.438,-162.1191;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;6;-1062.115,-95.63895;Inherit;True;Property;_Noise1;Noise1;2;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;82;-148.1197,391.4287;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;99;-234.9465,635.0168;Inherit;True;Property;_FountainGradient2;FountainGradient2;8;0;Create;True;0;0;0;False;0;False;-1;9e0c42b6ce2c66e4aaa64ca945213558;9e0c42b6ce2c66e4aaa64ca945213558;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;98;145.6217,452.9115;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;1010.913,-339.656;Inherit;False;Property;_OutlineStrength;OutlineStrength;6;0;Create;True;0;0;0;False;0;False;0.5;0.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-610.8322,100.4874;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1508.821,392.0779;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1414.097,-117.4498;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3256.017,-52.13444;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;FountainWaterFall;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.62;True;True;0;True;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;14;0
WireConnection;16;1;13;0
WireConnection;16;2;26;0
WireConnection;24;0;81;0
WireConnection;24;1;25;0
WireConnection;1;0;2;0
WireConnection;1;1;5;0
WireConnection;8;0;2;0
WireConnection;8;1;11;0
WireConnection;7;1;9;0
WireConnection;81;0;85;0
WireConnection;26;0;34;0
WireConnection;23;0;81;0
WireConnection;23;1;24;0
WireConnection;96;0;12;0
WireConnection;96;1;80;0
WireConnection;85;1;96;0
WireConnection;85;2;98;0
WireConnection;34;0;23;0
WireConnection;6;1;3;0
WireConnection;98;0;82;2
WireConnection;98;1;99;0
WireConnection;12;0;6;0
WireConnection;12;1;7;0
WireConnection;9;0;10;0
WireConnection;9;1;8;0
WireConnection;3;0;4;0
WireConnection;3;1;1;0
WireConnection;0;0;16;0
WireConnection;0;10;81;0
ASEEND*/
//CHKSM=2E1329E5BD982A883AC06E6C6704D537DDDA157B