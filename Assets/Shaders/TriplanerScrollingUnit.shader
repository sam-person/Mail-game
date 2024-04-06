// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/TriplanerScrollingUnit"
{
	Properties
	{
		_Speed("Speed", Range( 0 , 1)) = 1
		_Texture0("Texture 0", 2D) = "white" {}
		_Color_BG("Color_BG", Color) = (0.8274511,1,1,1)
		_Color_FG("Color_FG", Color) = (1,1,1,1)
		_Tiling("Tiling", Range( 0 , 1)) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _Color_BG;
		uniform float4 _Color_FG;
		uniform sampler2D _Texture0;
		uniform float _Tiling;
		uniform float _Speed;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 temp_cast_0 = (_Speed).xx;
			float2 panner15 = ( 1.0 * _Time.y * temp_cast_0 + float2( 0,0 ));
			float4 tex2DNode17 = tex2D( _Texture0, (ase_worldPos*_Tiling + float3( panner15 ,  0.0 )).xy );
			float4 lerpResult37 = lerp( _Color_BG , _Color_FG , tex2DNode17);
			o.Albedo = lerpResult37.rgb;
			o.Metallic = 0.0;
			o.Smoothness = 1.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.TexturePropertyNode;18;-1261.103,-417.6404;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;0;False;0;False;efa50567f98f84246933d3f9d5c9377e;efa50567f98f84246933d3f9d5c9377e;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScaleAndOffsetNode;24;-1105.804,64.85205;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;15;-1429.069,98.01619;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-1430.12,-57.50589;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;9;-1747.918,163.7913;Inherit;False;Property;_Speed;Speed;0;0;Create;True;0;0;0;False;0;False;1;0.038;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-432.7039,-413.6414;Inherit;True;Property;_Texture;Texture;5;0;Create;True;0;0;0;False;0;False;-1;b545a0a01575344429d3580261b0e70b;b545a0a01575344429d3580261b0e70b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;477.7437,-375.6701;Inherit;False;Constant;_Float1;Float 0;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;500.7437,-267.6703;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;37;204.9627,-876.0891;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;27;-9.914099,-184.5224;Inherit;False;Property;_Emission;Emission;2;1;[HDR];Create;True;0;0;0;False;0;False;0.7019065,0.7019065,0.7019065,1;0.7019065,0.7019065,0.7019065,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;240.4678,-348.9545;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;38;-98.03726,-1016.089;Inherit;False;Property;_Color_BG;Color_BG;3;0;Create;True;0;0;0;False;0;False;0.8274511,1,1,1;0.7735849,0.7630118,0.5728907,0.2627451;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;36;-100.0373,-841.0891;Inherit;False;Property;_Color_FG;Color_FG;4;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.9245283,0.9140962,0.6585084,0.254902;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-1401.522,255.5628;Inherit;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;0;False;0;False;1;0.33;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1192.53,-446.8875;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/TriplanerScrollingUnit;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;False;0;True;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;0;False;;0;False;;0;5;False;;10;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;2;0
WireConnection;24;1;39;0
WireConnection;24;2;15;0
WireConnection;15;2;9;0
WireConnection;17;0;18;0
WireConnection;17;1;24;0
WireConnection;37;0;38;0
WireConnection;37;1;36;0
WireConnection;37;2;17;0
WireConnection;25;0;27;0
WireConnection;25;1;17;0
WireConnection;0;0;37;0
WireConnection;0;3;29;0
WireConnection;0;4;28;0
ASEEND*/
//CHKSM=5250CC88962BB5C7FC6746B9BDE10AA8B2B5E866