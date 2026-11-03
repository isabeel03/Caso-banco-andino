Shader "Andino/HologramaMovil" {
 Properties {
 _Color("Color holograma", Color)=(0.05,0.85,1,0.35)
 _ScanDensity("Densidad de líneas",Range(1,60))=18
 _Speed("Velocidad suave",Range(0,1))=0.12
 }
 SubShader {
 Tags {"Queue"="Transparent" "RenderType"="Transparent"}
 Blend SrcAlpha OneMinusSrcAlpha
 ZWrite Off
 Cull Back
 Pass {
 CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
 #pragma multi_compile_instancing
 #include "UnityCG.cginc"
 struct appdata {float4 vertex:POSITION;float3 normal:NORMAL; UNITY_VERTEX_INPUT_INSTANCE_ID};
 struct v2f {float4 pos:SV_POSITION;float3 normal:TEXCOORD0;float3 world:TEXCOORD1;UNITY_VERTEX_OUTPUT_STEREO};
 fixed4 _Color;float _ScanDensity,_Speed;
 v2f vert(appdata v){v2f o;UNITY_SETUP_INSTANCE_ID(v);UNITY_INITIALIZE_OUTPUT(v2f,o);UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);o.pos=UnityObjectToClipPos(v.vertex);o.normal=UnityObjectToWorldNormal(v.normal);o.world=mul(unity_ObjectToWorld,v.vertex).xyz;return o;}
 fixed4 frag(v2f i):SV_Target {UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);float3 view=normalize(_WorldSpaceCameraPos-i.world);float edge=pow(1-saturate(abs(dot(normalize(i.normal),view))),2);float scan=.85+.15*sin(i.world.y*_ScanDensity-_Time.y*_Speed*6.28);return fixed4(_Color.rgb*scan,saturate(_Color.a*(.55+.45*edge)));}
 ENDCG
 }
 }
}
