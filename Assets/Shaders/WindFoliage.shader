// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WindFoliage"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_NoiseSmooth("Noise Smooth", 2D) = "white" {}
		_WindStrength("WindStrength", Range( 0 , 0.003)) = 2
		_Speed("Speed", Range( 0.001 , 0.01)) = 1
		_NoiseSize("NoiseSize", Range( 0.003 , 0.05)) = 0
		_LeafAlpha4_basecolor("LeafAlpha4_basecolor", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_WindJitter("WindJitter", Float) = 0
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
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _NoiseSmooth;
		uniform float _Speed;
		uniform float _NoiseSize;
		uniform float _WindJitter;
		uniform float _WindStrength;
		uniform float4 _Color0;
		uniform sampler2D _LeafAlpha4_basecolor;
		uniform float4 _LeafAlpha4_basecolor_ST;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 appendResult25 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 temp_output_17_0 = ( appendResult25 * _NoiseSize );
			float2 panner36 = ( ( _Speed * _Time.y ) * float2( 1,1 ) + temp_output_17_0);
			float2 panner55 = ( ( _Time.y * _WindJitter ) * float2( 1,1 ) + ( temp_output_17_0 * float2( 0,0 ) ));
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( ( ( tex2Dlod( _NoiseSmooth, float4( panner36, 0, 0.0) ) * tex2Dlod( _NoiseSmooth, float4( panner55, 0, 0.0) ) ) * _WindStrength ) * float4( ase_vertexNormal , 0.0 ) ).rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			o.Alpha = 1;
			float2 uv_LeafAlpha4_basecolor = i.uv_texcoord * _LeafAlpha4_basecolor_ST.xy + _LeafAlpha4_basecolor_ST.zw;
			clip( tex2D( _LeafAlpha4_basecolor, uv_LeafAlpha4_basecolor ).a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.SamplerNode;4;-352,128;Inherit;True;Property;_NoiseSmooth;Noise Smooth;1;0;Create;True;0;0;0;False;0;False;-1;62695c15a49d430abd2ae1471c72b1c7;62695c15a49d430abd2ae1471c72b1c7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;36;-566.421,151.9434;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;19;-1682.503,32.3914;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1420.572,72.3718;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1204.057,72.17081;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1259.421,352.9435;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;37;-1466.421,463.9435;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1592.421,254.9435;Inherit;False;Property;_NoiseSize;NoiseSize;4;0;Create;True;0;0;0;False;0;False;0;0.0128;0.003;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1590.057,354.171;Inherit;False;Property;_Speed;Speed;3;0;Create;True;0;0;0;False;0;False;1;0.00585;0.001;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-905.6493,434.2939;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;56;-453.6493,412.2939;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;4;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1157.649,566.2938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;52;1437.107,43.36895;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WindFoliage;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SamplerNode;51;716.8665,-242.2622;Inherit;True;Property;_LeafAlpha4_basecolor;LeafAlpha4_basecolor;5;0;Create;True;0;0;0;False;0;False;-1;e89d64857446e014faf04ae1aa9c9485;c8a6f841a644ec644bab0a6baae42ea8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;1083.934,-258.4384;Inherit;False;Property;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.3098038,0.6901961,0.3843137,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;751.3335,130.751;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;96.79881,137.853;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;1235.146,574.7445;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;40;983.1461,557.7445;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;55;-719.6493,437.2939;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1378.649,664.2938;Inherit;False;Property;_WindJitter;WindJitter;7;0;Create;True;0;0;0;False;0;False;0;-0.003;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;468.3333,372.7509;Inherit;False;Property;_WindStrength;WindStrength;2;0;Create;True;0;0;0;False;0;False;2;0.00094;0;0.003;0;1;FLOAT;0
WireConnection;4;1;36;0
WireConnection;36;0;17;0
WireConnection;36;1;38;0
WireConnection;25;0;19;1
WireConnection;25;1;19;3
WireConnection;17;0;25;0
WireConnection;17;1;35;0
WireConnection;38;0;16;0
WireConnection;38;1;37;0
WireConnection;54;0;17;0
WireConnection;56;1;55;0
WireConnection;57;0;37;0
WireConnection;57;1;58;0
WireConnection;52;0;53;0
WireConnection;52;10;51;4
WireConnection;52;11;41;0
WireConnection;13;0;59;0
WireConnection;13;1;14;0
WireConnection;59;0;4;0
WireConnection;59;1;56;0
WireConnection;41;0;13;0
WireConnection;41;1;40;0
WireConnection;55;0;54;0
WireConnection;55;1;57;0
ASEEND*/
//CHKSM=1AB237D6241F35637576CF23FCC470623527A51E