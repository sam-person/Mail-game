// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Puddle"
{
	Properties
	{
		_Color("Color", Color) = (0.4009434,0.6300666,1,0)
		_ColorVoronoi("ColorVoronoi", Color) = (1,1,1,0)
		_Opacity("Opacity", Float) = 0.5
		_Smoothness("Smoothness", Float) = 0.5
		_VoronoiOffset("VoronoiOffset", Float) = 1
		_VoronoiTiling("VoronoiTiling", Vector) = (1,1,0,0)
		_VoronoiSpeed("VoronoiSpeed", Vector) = (0,0,0,0)
		_VoronoiPower("VoronoiPower", Float) = 3
		_Normal("Normal", 2D) = "bump" {}
		_WaterStreamNormal("WaterStreamNormal", 2D) = "bump" {}
		_Noise3("Noise3", 2D) = "white" {}
		_WaterStreamNormalColor("WaterStreamNormalColor", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_MaskWarpTiling("MaskWarpTiling", Vector) = (2,2,0,0)
		_MaskWarpSpeed("MaskWarpSpeed", Vector) = (0.1,0,0,0)
		_MaskWarpStrength("MaskWarpStrength", Float) = 1
		_RipplesSpeed("RipplesSpeed", Vector) = (1,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float2 uv2_texcoord2;
		};

		uniform sampler2D _Normal;
		uniform sampler2D _Noise3;
		uniform float2 _MaskWarpTiling;
		uniform float2 _MaskWarpSpeed;
		uniform float _MaskWarpStrength;
		uniform sampler2D _WaterStreamNormal;
		uniform float2 _RipplesSpeed;
		uniform float4 _ColorVoronoi;
		uniform float _VoronoiOffset;
		uniform float2 _VoronoiTiling;
		uniform float2 _VoronoiSpeed;
		uniform float _VoronoiPower;
		uniform float4 _Color;
		uniform sampler2D _WaterStreamNormalColor;
		uniform float _Smoothness;
		uniform sampler2D _Mask;
		uniform float _Opacity;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord29 = i.uv_texcoord * _MaskWarpTiling + ( _Time.y * ( _MaskWarpSpeed / float2( 10,10 ) ) );
			float4 temp_output_81_0 = ( float4( i.uv_texcoord, 0.0 , 0.0 ) + ( tex2D( _Noise3, uv_TexCoord29 ) * ( _MaskWarpStrength / 100.0 ) ) );
			float2 uv2_TexCoord63 = i.uv2_texcoord2 + ( _Time.y * _RipplesSpeed );
			o.Normal = BlendNormals( UnpackNormal( tex2D( _Normal, temp_output_81_0.rg ) ) , UnpackNormal( tex2D( _WaterStreamNormal, uv2_TexCoord63 ) ) );
			float time1 = ( _Time.y * _VoronoiOffset );
			float2 voronoiSmoothId1 = 0;
			float2 uv2_TexCoord23 = i.uv2_texcoord2 * _VoronoiTiling + ( _Time.y * _VoronoiSpeed );
			float2 coords1 = uv2_TexCoord23 * 5.0;
			float2 id1 = 0;
			float2 uv1 = 0;
			float voroi1 = voronoi1( coords1, time1, id1, uv1, 0, voronoiSmoothId1 );
			float4 lerpResult89 = lerp( _Color , tex2D( _WaterStreamNormalColor, uv2_TexCoord63 ) , 0.1);
			o.Albedo = ( ( _ColorVoronoi * pow( voroi1 , _VoronoiPower ) ) + lerpResult89 ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = ( tex2D( _Mask, temp_output_81_0.rg ).a * _Opacity );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.zw = customInputData.uv2_texcoord2;
				o.customPack1.zw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack1.zw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.PowerNode;24;-946.5659,147.7376;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;1;-1291.306,79.70926;Inherit;True;0;1;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;2;False;2;FLOAT;5;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleTimeNode;5;-1794.306,92.70926;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1756.306,220.7092;Inherit;False;Property;_VoronoiOffset;VoronoiOffset;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1532.306,107.7093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1473.64,725.3008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-1873.306,-42.29074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;14;-2174.307,-21.29075;Inherit;False;Property;_VoronoiSpeed;VoronoiSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;15;-2178.307,-126.2908;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;13;-1909.306,-243.2908;Inherit;False;Property;_VoronoiTiling;VoronoiTiling;5;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1637.667,-91.58786;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;65;-1937.446,-525.3618;Inherit;False;Property;_RipplesSpeed;RipplesSpeed;16;0;Create;True;0;0;0;False;0;False;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1674.411,-495.3354;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;27;-756.4692,543.7325;Inherit;True;Property;_Noise3;Noise3;10;0;Create;True;0;0;0;False;0;False;-1;5508df388763cdf408cf60d2aac88e86;5508df388763cdf408cf60d2aac88e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;81;-95.95337,452.0674;Inherit;True;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-363.6306,717.4359;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;83;-560.9534,797.0674;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;-1916.799,754.7672;Inherit;False;Property;_MaskWarpSpeed;MaskWarpSpeed;14;0;Create;True;0;0;0;False;0;False;0.1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;84;-1614.446,752.6902;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;10,10;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1055.484,582.733;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;77;-369.0527,355.837;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;-789.6306,796.4359;Inherit;False;Property;_MaskWarpStrength;MaskWarpStrength;15;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-1397.7,-476.2914;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;53;104.8454,425.8879;Inherit;True;Property;_Mask;Mask;12;0;Create;True;0;0;0;False;0;False;-1;ba70d64fd7f028049a92c42e028d7646;ba70d64fd7f028049a92c42e028d7646;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;85;264.8184,617.2438;Inherit;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;423.3686,540.6185;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;67;651.1442,-264.543;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1172.495,76.94618;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Puddle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;462.8368,32.49837;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;49;509.2893,248.5669;Inherit;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;89;-237.6369,-215.3851;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-394.1206,-122.682;Inherit;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-518.0331,-356.872;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;0.4009434,0.6300666,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-142.1769,110.9466;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;7;-518.548,-9.696411;Inherit;False;Property;_ColorVoronoi;ColorVoronoi;1;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-1281.079,364.4469;Inherit;False;Property;_VoronoiPower;VoronoiPower;7;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;91;-1301.74,562.9459;Inherit;False;Property;_MaskWarpTiling;MaskWarpTiling;13;0;Create;True;0;0;0;False;0;False;2,2;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;55;239.5297,-474.0565;Inherit;True;Property;_Normal;Normal;8;0;Create;True;0;0;0;False;0;False;-1;0ca26edcf94b2b946a2e77221c0c25b0;0ca26edcf94b2b946a2e77221c0c25b0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;60;-1109.467,-517.7622;Inherit;True;Property;_WaterStreamNormal;WaterStreamNormal;9;0;Create;True;0;0;0;False;0;False;-1;748b7dbeaa43b694c802205a44084fba;748b7dbeaa43b694c802205a44084fba;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;88;-1104.552,-307.7248;Inherit;True;Property;_WaterStreamNormalColor;WaterStreamNormalColor;11;0;Create;True;0;0;0;False;0;False;-1;5c22b32c48db151469aab7e713df2988;5c22b32c48db151469aab7e713df2988;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;24;0;1;0
WireConnection;24;1;25;0
WireConnection;1;0;23;0
WireConnection;1;1;8;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;44;0;5;0
WireConnection;44;1;84;0
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;23;0;13;0
WireConnection;23;1;16;0
WireConnection;66;0;15;0
WireConnection;66;1;65;0
WireConnection;27;1;29;0
WireConnection;81;0;77;0
WireConnection;81;1;73;0
WireConnection;73;0;27;0
WireConnection;73;1;83;0
WireConnection;83;0;70;0
WireConnection;84;0;68;0
WireConnection;29;0;91;0
WireConnection;29;1;44;0
WireConnection;63;1;66;0
WireConnection;53;1;81;0
WireConnection;86;0;53;4
WireConnection;86;1;85;0
WireConnection;67;0;55;0
WireConnection;67;1;60;0
WireConnection;0;0;19;0
WireConnection;0;1;67;0
WireConnection;0;4;49;0
WireConnection;0;9;86;0
WireConnection;19;0;6;0
WireConnection;19;1;89;0
WireConnection;89;0;3;0
WireConnection;89;1;88;0
WireConnection;89;2;90;0
WireConnection;6;0;7;0
WireConnection;6;1;24;0
WireConnection;55;1;81;0
WireConnection;60;1;63;0
WireConnection;88;1;63;0
ASEEND*/
//CHKSM=96E70E7127DC23C9C814994A8C744A0419BCB5B9