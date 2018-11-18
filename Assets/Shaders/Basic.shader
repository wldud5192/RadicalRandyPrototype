Shader "Me/Basic"
{

	Properties {

	//As soon as you done this, a texture picker will appear in the inspector
			_MainTex("Texture", 2D) = "white"{}
			_Color("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags
			{
				"Queue" = "Transparent"
			}
	//Traditional graphics use 2 programs: Vertex Shaders & Fragment Shaders (=Potential Pixels)
	//4 Key parts: appdata, v2f
	//struct appdata {} gathers vertex information from each vertext on the mesh
	//struct v2f {} defines what information we are passing into the fragment function
	//v2f vert(appdata v) takes our appdata as a parameter and returns a v2f parameter
	//float4 frag(v2f i) takes v2f as a parameter and returns colour
	 
		Pass
		{

			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				//Wiki: Providing vertex data to vertex programs
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			
			v2f vert (appdata v)
			{
				v2f o;
								//MATRIXS multiplication on the local vertex
								//To take it from a point relative to the object and
								//transform it into a point on the screen
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = v.uv;

				return o;
			}

			sampler2D _MainTex;

			//Turns potential pixels into colours
			fixed4 frag (v2f i) : SV_Target

			{
				float4 color = tex2D(_MainTex, i.uv);
				return color;

			//fed in the x and y value of the uv into the red and green parts of the colour
				//return float4(i.uv.r, i.uv.g, 1 , 1);

			}
			ENDCG
		}
	}
}
