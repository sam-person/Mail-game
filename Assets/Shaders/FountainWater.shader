// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterFountain"
{
	Properties
	{
		_Color("Color", Color) = (0.4009434,0.6300666,1,0)
		_ColorVoronoi("ColorVoronoi", Color) = (1,1,1,0)
		_VoronoiOffset("VoronoiOffset", Float) = 1
		_WaveSpeed("WaveSpeed", Float) = 1
		_VoronoiTiling("VoronoiTiling", Vector) = (1,1,0,0)
		_VoronoiSpeed("VoronoiSpeed", Vector) = (0,0,0,0)
		_VoronoiPower("VoronoiPower", Float) = 3
		_Noise3("Noise3", 2D) = "white" {}
		_WaveHeight("WaveHeight", Float) = 0.0001
		[Toggle(_USEVERTEXDISPLACEMENT_ON)] _UseVertexDisplacement("Use Vertex Displacement", Float) = 0
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEVERTEXDISPLACEMENT_ON
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv2_texcoord2;
		};

		uniform sampler2D _Noise3;
		uniform float _WaveSpeed;
		uniform float _WaveHeight;
		uniform float4 _ColorVoronoi;
		uniform float _VoronoiOffset;
		uniform float2 _VoronoiTiling;
		uniform float2 _VoronoiSpeed;
		uniform float _VoronoiPower;
		uniform float4 _Color;


		float2 voronoihash1( float2 p )
		{
			
			p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
			return frac( sin( p ) *43758.5453);
		}


		float voronoi1( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
		{
			float2 n = floor( v );
			float2 f = frac( v );
			float F1 = 8.0;
			float F2 = 8.0; float2 mg = 0;
			for ( int j = -1; j <= 1; j++ )
			{
				for ( int i = -1; i <= 1; i++ )
			 	{
			 		float2 g = float2( i, j );
			 		float2 o = voronoihash1( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
					float d = 0.707 * sqrt(dot( r, r ));
			 		if( d<F1 ) {
			 			F2 = F1;
			 			F1 = d; mg = g; mr = r; id = o;
			 		} else if( d<F2 ) {
			 			F2 = d;
			
			 		}
			 	}
			}
			return F1;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 temp_cast_0 = (( ( _Time.y * _WaveSpeed ) + (ase_vertex3Pos).y )).xx;
			float2 uv_TexCoord29 = v.texcoord.xy + temp_cast_0;
			float3 ase_vertexNormal = v.normal.xyz;
			#ifdef _USEVERTEXDISPLACEMENT_ON
				float4 staticSwitch45 = ( ( tex2Dlod( _Noise3, float4( uv_TexCoord29, 0, 0.0) ) - float4( 0.5,0,0,0 ) ) * float4( ( ase_vertexNormal * _WaveHeight ) , 0.0 ) );
			#else
				float4 staticSwitch45 = float4( 0,0,0,0 );
			#endif
			v.vertex.xyz += staticSwitch45.rgb;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float time1 = ( _Time.y * _VoronoiOffset );
			float2 voronoiSmoothId1 = 0;
			float2 uv2_TexCoord23 = i.uv2_texcoord2 * _VoronoiTiling + ( _Time.y * _VoronoiSpeed );
			float2 coords1 = uv2_TexCoord23 * 5.0;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0, voronoiSmoothId1 );
			o.Albedo = ( ( _ColorVoronoi * pow( voroi1 , _VoronoiPower ) ) + _Color ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.RangedFloatNode;2;-804.5,-370.5;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-450.5,81.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;3;-538.5,-364.5;Inherit;False;Property;_Color;Color;1;0;Create;True;0;0;0;False;0;False;0.4009434,0.6300666,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-254.2739,9.980383;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1873.306,-42.29074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;14;-2174.307,-21.29075;Inherit;False;Property;_VoronoiSpeed;VoronoiSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2178.307,-126.2908;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-1909.306,-243.2908;Inherit;False;Property;_VoronoiTiling;VoronoiTiling;5;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;7;-773.1747,-104.9648;Inherit;False;Property;_ColorVoronoi;ColorVoronoi;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;24;-946.5659,147.7376;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1176.566,459.7376;Inherit;False;Property;_VoronoiPower;VoronoiPower;7;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1291.306,79.70926;Inherit;True;0;1;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;27;-756.4692,543.7325;Inherit;True;Property;_Noise3;Noise3;8;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;WaterFountain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.PosVertexDataNode;34;-1708.987,931.5965;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;35;-1470.223,950.8686;Inherit;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-1184.544,868.0328;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;37;-662.7288,791.8214;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-330.37,719.2376;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-335.675,872.9027;Inherit;False;2;2;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-168.3658,851.7678;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1794.306,92.70926;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1756.306,220.7092;Inherit;False;Property;_VoronoiOffset;VoronoiOffset;3;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1532.306,107.7093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1473.64,725.3008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1697.64,838.3007;Inherit;False;Property;_WaveSpeed;WaveSpeed;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-622.5851,968.8322;Inherit;False;Property;_WaveHeight;WaveHeight;9;0;Create;True;0;0;0;False;0;False;0.0001;0.0001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1055.484,582.733;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1637.667,-91.58786;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;45;-293.5006,387.0082;Inherit;False;Property;_UseVertexDisplacement;Use Vertex Displacement;10;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
WireConnection;6;0;7;0
WireConnection;6;1;24;0
WireConnection;19;0;6;0
WireConnection;19;1;3;0
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;1;0;23;0
WireConnection;1;1;8;0
WireConnection;27;1;29;0
WireConnection;0;0;19;0
WireConnection;0;11;45;0
WireConnection;35;0;34;0
WireConnection;36;0;44;0
WireConnection;36;1;35;0
WireConnection;38;0;27;0
WireConnection;39;0;37;0
WireConnection;39;1;41;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;44;0;5;0
WireConnection;44;1;43;0
WireConnection;29;1;36;0
WireConnection;23;0;13;0
WireConnection;23;1;16;0
WireConnection;45;0;40;0
ASEEND*/
//CHKSM=383F779049B1718093DED7FF1504F328F6F83F76