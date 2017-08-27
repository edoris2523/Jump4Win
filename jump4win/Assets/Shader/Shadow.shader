// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Created by zeo: http://blog.naver.com/zeodtr
Shader "Custom/Shadow"
{
 Properties
 {
  _ShadowColor ("Shadow's Color", Color) = (0.8, 0.8, 0.8, 1)
 }
 
 SubShader
 {
  Tags { "Queue" = "Background+2" }
  Pass
  {
   Blend DstColor Zero
   ZTest GEqual
  
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #include "UnityCG.cginc"
   
   fixed4 _ShadowColor;
   
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
    o.pos = UnityObjectToClipPos(a_in.vertex);
    return o;
   }
   
   void frag(out fixed4 o_color : COLOR)
   {
    o_color = _ShadowColor;
   }
   
   ENDCG
  }
 }
 
 FallBack "Diffuse"
}