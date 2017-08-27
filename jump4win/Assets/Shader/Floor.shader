// Created by zeo: http://blog.naver.com/zeodtr
Shader "Custom/Floor"
{
 Properties
 {
 	_Color ("Color", Color) = (1, 0.5, 0.5, 1)
 }
 
 SubShader
 {
  Tags { "Queue" = "Background" }
  Pass
  {
   ZWrite Off

   CGPROGRAM
   	#pragma vertex vert
   	#pragma fragment frag

   	float4 vert(float4 vertexPos : POSITION) : SV_POSITION
   	{
   		return UnityObjectToClipPos(vertexPos);
   	}

   	float4 frag(void) : COLOR
   	{
   		return float4(0.1, 0.1, 0.1, 0.7);
   	}

   	ENDCG
 }
}
}