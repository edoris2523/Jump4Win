// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'


// Created by zeo: http://blog.naver.com/zeodtr
Shader "Custom/Shadow Mask"
{
 Properties
 {
 }
 
 SubShader
 {
  Tags { "Queue" = "Background+1" }
  Pass
  {
   Blend Zero One
   ZWrite On
  
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "UnityCG.cginc"
   
   struct vin
   {
       float4 vertex : POSITION;
   };
   
   struct v2f
   {
    float4 pos: POSITION;
   };
   
   v2f vert(vin a_in)
   {
    v2f o;
    float4 vpos = mul(unity_ObjectToWorld, a_in.vertex);
    vpos.y = 0;
    o.pos = mul(UNITY_MATRIX_VP, vpos);
    return o;
   }
   
   void frag(out fixed4 o_color : COLOR)
   {
    o_color = 0;
   }
   
   ENDCG
  }
 }
 
 FallBack "Diffuse"
}