// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/WaterSea"
{
	Properties
	{
		_Color("Color", Color) = (0.4009434,0.6300666,1,0)
		_ColorVoronoi("ColorVoronoi", Color) = (1,1,1,0)
		_VoronoiOffset("VoronoiOffset", Float) = 1
		_VoronoiTiling("VoronoiTiling", Vector) = (1,1,0,0)
		_WaveNoiseTiling("WaveNoiseTiling", Vector) = (1,1,0,0)
		_EdgeNoiseTiling("EdgeNoiseTiling", Vector) = (1,1,0,0)
		_VoronoiSpeed("VoronoiSpeed", Vector) = (0,0,0,0)
		_VoronoiPower("VoronoiPower", Float) = 3
		_Noise("Noise", 2D) = "white" {}
		_Noise1("Noise", 2D) = "white" {}
		_WaveHeight("WaveHeight", Float) = 0.0001
		[Toggle(_USEVERTEXDISPLACEMENT_ON)] _UseVertexDisplacement("Use Vertex Displacement", Float) = 0
		_WaveSpeed("WaveSpeed", Vector) = (1,1,0,0)
		_FoamSpeed("FoamSpeed", Vector) = (1,1,0,0)
		_FoamAmount("FoamAmount", Float) = 0
		_FoamCutoff("FoamCutoff", Float) = 1
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma shader_feature _USEVERTEXDISPLACEMENT_ON
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform sampler2D _Noise;
		uniform float2 _WaveNoiseTiling;
		uniform float2 _WaveSpeed;
		uniform float _WaveHeight;
		uniform float4 _ColorVoronoi;
		uniform float _VoronoiOffset;
		uniform float2 _VoronoiTiling;
		uniform float2 _VoronoiSpeed;
		uniform float _VoronoiPower;
		uniform float4 _Color;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FoamAmount;
		uniform float _FoamCutoff;
		uniform sampler2D _Noise1;
		uniform float2 _EdgeNoiseTiling;
		uniform float2 _FoamSpeed;


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
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 temp_output_63_0 = (ase_worldPos).xz;
			float2 temp_output_44_0 = ( _Time.y * _WaveSpeed );
			float3 ase_vertexNormal = v.normal.xyz;
			#ifdef _USEVERTEXDISPLACEMENT_ON
				float4 staticSwitch45 = ( ( tex2Dlod( _Noise, float4( (temp_output_63_0*_WaveNoiseTiling + temp_output_44_0), 0, 0.0) ) - float4( 0.5,0,0,0 ) ) * float4( ( ase_vertexNormal * _WaveHeight ) , 0.0 ) );
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
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_63_0 = (ase_worldPos).xz;
			float2 coords1 = (temp_output_63_0*_VoronoiTiling + ( _Time.y * _VoronoiSpeed )) * 5.0;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0, voronoiSmoothId1 );
			float4 temp_cast_0 = (1.0).xxxx;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth68 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth68 = abs( ( screenDepth68 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamAmount ) );
			float4 temp_cast_1 = (( distanceDepth68 * _FoamCutoff )).xxxx;
			float4 clampResult82 = clamp( step( temp_cast_1 , tex2D( _Noise1, (temp_output_63_0*_EdgeNoiseTiling + ( _Time.y * _FoamSpeed )) ) ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 lerpResult84 = lerp( ( ( _ColorVoronoi * pow( voroi1 , _VoronoiPower ) ) + _Color ) , temp_cast_0 , clampResult82);
			o.Albedo = lerpResult84.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-450.5,81.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-254.2739,9.980383;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1873.306,-42.29074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2178.307,-126.2908;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-773.1747,-104.9648;Inherit;False;Property;_ColorVoronoi;ColorVoronoi;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.6509434,0.6509434,0.6509434,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;24;-946.5659,147.7376;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;37;-662.7288,791.8214;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-330.37,719.2376;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0.5,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-335.675,872.9027;Inherit;False;2;2;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-168.3658,851.7678;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1794.306,92.70926;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1756.306,220.7092;Inherit;False;Property;_VoronoiOffset;VoronoiOffset;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1532.306,107.7093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-622.5851,968.8322;Inherit;False;Property;_WaveHeight;WaveHeight;10;0;Create;True;0;0;0;False;0;False;0.0001;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1279.034,346.4838;Inherit;False;Property;_VoronoiPower;VoronoiPower;7;0;Create;True;0;0;0;False;0;False;3;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1291.306,79.70926;Inherit;True;0;1;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.ComponentMaskNode;63;-2020.541,-398.9962;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;46;-2258.312,-398.8065;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScaleAndOffsetNode;47;-1500.012,-230.9677;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;13;-1909.306,-243.2908;Inherit;False;Property;_VoronoiTiling;VoronoiTiling;3;0;Create;True;0;0;0;False;0;False;1,1;0.2,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;14;-2174.307,-21.29075;Inherit;False;Property;_VoronoiSpeed;VoronoiSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;-0.2,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;783.8302,17.49378;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/WaterSea;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.StaticSwitch;45;68.67918,801.2382;Inherit;False;Property;_UseVertexDisplacement;Use Vertex Displacement;11;0;Create;True;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;84;301.2944,10.44519;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-39.24459,-53.724;Inherit;False;Constant;_Float1;Float 1;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;68;-942.0747,-1011.864;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1243.434,-1004.782;Inherit;False;Property;_FoamAmount;FoamAmount;14;0;Create;True;0;0;0;False;0;False;0;3.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-322.4344,-956.7822;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-589.4347,-882.7822;Inherit;False;Property;_FoamCutoff;FoamCutoff;15;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-772.5,-294.5;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;0.4009434,0.6300666,1,0;0.2886253,0.8867924,0.8688544,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;34;-1708.987,931.5965;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;35;-1470.223,950.8686;Inherit;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-1184.544,868.0328;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1473.64,725.3008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;27;-756.4692,543.7325;Inherit;True;Property;_Noise;Noise;8;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;64;-1023.944,642.043;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;65;-1372.349,573.8003;Inherit;False;Property;_WaveNoiseTiling;WaveNoiseTiling;4;0;Create;True;0;0;0;False;0;False;1,1;0.02,0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;66;-1733.204,653.1139;Inherit;False;Property;_WaveSpeed;WaveSpeed;12;0;Create;True;0;0;0;False;0;False;1,1;-0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-1447.86,-1827.754;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;89;-998.1642,-1911.012;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;88;-730.6894,-2009.323;Inherit;True;Property;_Noise1;Noise;9;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;90;-1346.569,-1979.255;Inherit;False;Property;_EdgeNoiseTiling;EdgeNoiseTiling;5;0;Create;True;0;0;0;False;0;False;1,1;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;91;-1707.424,-1899.941;Inherit;False;Property;_FoamSpeed;FoamSpeed;13;0;Create;True;0;0;0;False;0;False;1,1;0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.StepOpNode;86;-105.2581,-993.9422;Inherit;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;82;291.8988,-850.3386;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
WireConnection;6;0;7;0
WireConnection;6;1;24;0
WireConnection;19;0;6;0
WireConnection;19;1;3;0
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;38;0;27;0
WireConnection;39;0;37;0
WireConnection;39;1;41;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;1;0;47;0
WireConnection;1;1;8;0
WireConnection;63;0;46;0
WireConnection;47;0;63;0
WireConnection;47;1;13;0
WireConnection;47;2;16;0
WireConnection;0;0;84;0
WireConnection;0;11;45;0
WireConnection;45;0;40;0
WireConnection;84;0;19;0
WireConnection;84;1;85;0
WireConnection;84;2;82;0
WireConnection;68;0;69;0
WireConnection;70;0;68;0
WireConnection;70;1;71;0
WireConnection;35;0;34;0
WireConnection;36;0;44;0
WireConnection;36;1;35;0
WireConnection;44;0;5;0
WireConnection;44;1;66;0
WireConnection;27;1;64;0
WireConnection;64;0;63;0
WireConnection;64;1;65;0
WireConnection;64;2;44;0
WireConnection;87;0;5;0
WireConnection;87;1;91;0
WireConnection;89;0;63;0
WireConnection;89;1;90;0
WireConnection;89;2;87;0
WireConnection;88;1;89;0
WireConnection;86;0;70;0
WireConnection;86;1;88;0
WireConnection;82;0;86;0
ASEEND*/
//CHKSM=D037E7E9F4B31206EBEF6E2A3BBCC5F60ECDB941