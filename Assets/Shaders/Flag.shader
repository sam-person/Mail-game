// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Flag"
{
	Properties
	{
		_MovementScale("MovementScale", Range( 0 , 0.2)) = 0.1
		_WaveRotation("WaveRotation", Float) = 30
		_Speed("Speed", Range( 0 , 5)) = 2
		_Texture("Texture", 2D) = "white" {}
		_Roughness("Roughness", Float) = 0.5
		[Toggle(_TOPMOUNTED_ON)] _TopMounted("TopMounted", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _TOPMOUNTED_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _WaveRotation;
		uniform float _Speed;
		uniform float _MovementScale;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _Roughness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float smoothstepResult37 = smoothstep( 0.0 , 0.2 , v.texcoord.xy.x);
			float smoothstepResult58 = smoothstep( 0.55 , 0.36 , v.texcoord.xy.y);
			#ifdef _TOPMOUNTED_ON
				float staticSwitch57 = smoothstepResult58;
			#else
				float staticSwitch57 = smoothstepResult37;
			#endif
			float cos46 = cos( radians( _WaveRotation ) );
			float sin46 = sin( radians( _WaveRotation ) );
			float2 rotator46 = mul( v.texcoord.xy - float2( 0.5,0.5 ) , float2x2( cos46 , -sin46 , sin46 , cos46 )) + float2( 0.5,0.5 );
			float mulTime7 = _Time.y * _Speed;
			float3 appendResult28 = (float3(0.0 , 0.0 , ( ( staticSwitch57 * sin( ( ( rotator46.x * ( rotator46.x * 10.0 ) ) + mulTime7 ) ) ) * ( (0.1 + (staticSwitch57 - 0.0) * (1.0 - 0.1) / (1.0 - 0.0)) * _MovementScale ) )));
			v.vertex.xyz += ( ase_vertex3Pos.z + appendResult28 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			o.Albedo = tex2D( _Texture, uv_Texture ).rgb;
			o.Smoothness = _Roughness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.PosVertexDataNode;36;-383.3546,-186.9365;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;14;150.2212,-96.56296;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-92.53107,58.91745;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;820.6035,-57.75508;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Flag;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;4;-1124.81,79.53458;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;8;-845.4891,48.1905;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-602.0262,24.49669;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-358.7941,22.63842;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-775.6497,421.8175;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;40;-1043.45,438.7174;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-1415.809,247.5346;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1686.011,75.17285;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;46;-2229.804,-50.02943;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RadiansOpNode;52;-2443.553,144.5243;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2652.067,144.4837;Inherit;False;Property;_WaveRotation;WaveRotation;1;0;Create;True;0;0;0;False;0;False;30;-30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;408.064,-359.5167;Inherit;True;Property;_Texture;Texture;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;586.064,-42.51672;Inherit;False;Property;_Roughness;Roughness;4;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1726.461,249.5409;Inherit;False;Property;_Speed;Speed;2;0;Create;True;0;0;0;False;0;False;2;2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1107.082,704.2714;Inherit;False;Property;_MovementScale;MovementScale;0;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;2;-2492.807,-47.49073;Inherit;True;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;57;-1097.631,-395.3524;Inherit;False;Property;_TopMounted;TopMounted;5;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;47;-1877.522,-47.63585;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1445.278,-2.951123;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;37;-1465.664,-276.772;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-1469.593,-519.7397;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.55;False;2;FLOAT;0.36;False;1;FLOAT;0
WireConnection;14;0;36;3
WireConnection;14;1;28;0
WireConnection;28;2;20;0
WireConnection;0;0;54;0
WireConnection;0;4;55;0
WireConnection;0;11;14;0
WireConnection;4;0;3;0
WireConnection;4;1;7;0
WireConnection;8;0;4;0
WireConnection;38;0;57;0
WireConnection;38;1;8;0
WireConnection;20;0;38;0
WireConnection;20;1;41;0
WireConnection;41;0;40;0
WireConnection;41;1;21;0
WireConnection;40;0;57;0
WireConnection;7;0;42;0
WireConnection;39;0;47;0
WireConnection;46;0;2;0
WireConnection;46;2;52;0
WireConnection;52;0;53;0
WireConnection;57;1;37;0
WireConnection;57;0;58;0
WireConnection;47;0;46;0
WireConnection;3;0;47;0
WireConnection;3;1;39;0
WireConnection;37;0;2;1
WireConnection;58;0;2;2
ASEEND*/
//CHKSM=6E278706B49B1A86D5A8A7A8FAE50BD326A530C5