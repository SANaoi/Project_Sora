//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "miHoYo/CRP_Scene/Effect/Effect_DreamBubble" {
Properties {
[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
[Toggle(_USE_QTANGET)] _UseQTange ("Use Model Tangent", Float) = 1
[Enum(BaseColor,0,MixedColor,1)] _ColorMode ("Color Mode", Float) = 0
[Toggle] _UseLocalSpace ("BaseColor Use Local Space", Float) = 0
_Color1 ("BaseColor Mix", Color) = (1,1,1,1)
_VolumInts ("BaseColor Effected by Volume", Range(0, 1)) = 1
_ColorTex1 ("Base Color Partten1 Texture", 2D) = "white" { }
_NoiseSize1 ("Color Partten1 Size", Vector) = (1,1,1,1)
_ColorTex2 ("Base Color Partten2 Texture", 2D) = "white" { }
_NoiseSize2 ("Color Partten2 Size", Vector) = (1,1,1,1)
_DistTex ("Shape Distort Texture", 2D) = "black" { }
_DistAmount ("Shape Distor Amount", Float) = 1
[Toggle] _DistDir ("Shape Distor is Additive Only", Float) = 0
_TintColor ("Frsnel Color", Color) = (1,1,1,1)
_FrsnParams ("Frsnel Parameters", Vector) = (0,0,0,0)
_MainSpeed ("Base Color Texture Speed & Intesnity", Vector) = (0,0,0,0)
_SubSpeed ("Parallax Texture Speed", Vector) = (0,0,0,0)
_Reftex ("Color Reflection Cube", Cube) = "black" { }
_RefLod ("Color Reflection Cube Blur Amounmt", Float) = 0
[Toggle] _UseDither ("Use Dither Alpha", Float) = 1
_DitherAlpha ("Alpha Clip", Range(0, 1)) = 1
_DitherDistanceScale ("DitherDistanceScale", Float) = 1
[Toggle(_USE_BASECOLORTEX)] _UseColorTex ("Use Base Color Texture", Float) = 0
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _MainTex ("Base Color Texture", 2D) = "white" { }
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _TexInts ("Base Color Texture Intensity", Range(0, 1)) = 1
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _OutLineColor ("Base Color Texture Outline Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _RimEdgeWidth ("Base Color Rim Ountline Width", Range(0, 128)) = 0
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _RimEdge ("Base Color Rim Ountline Edge", Range(0, 1)) = 0
[MHYKeywordFilter(_USE_BASECOLORTEX On)] _RimInst ("Base Color Rim Ountline Intensity", Range(0, 1)) = 0
[Toggle(_USE_GPUCROWD)] _UseGPUCrowd ("Enable GPU Crowd Animation", Float) = 0
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniTexture ("Ani Texture", 2D) = "white" { }
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniTexWidth ("Tex Width", Float) = 2
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniTexHeight ("Tex Height", Float) = 2
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniDuration ("Ani Duration", Float) = 0
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniTime ("Ani Time", Range(0, 1)) = 0
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUBoneNum ("Bone Num", Float) = 1
[MHYKeywordFilter(_USE_GPUCROWD On)] _GPUAniRemapParams ("Remap Params", Vector) = (0,0,0,0)
_SoftEdgeAmount ("Sofr Edge Amount", Range(0, 2)) = 0.25
_LightedEdgeWidth ("Soft Edge Lighted Width", Range(0, 2)) = 0.35
_LightedEdgeBlend ("Soft Edge Lighted Blend", Range(0, 1)) = 0.1
_EdgeColor ("Soft Edge Lighted Color", Color) = (1,1,1,1)
_ForwardOffsetFactor ("Forward Offset Factor", Float) = 0
_ForwardOffsetUnits ("Forward Offset Units", Float) = 0
[Enum(Off,0,On,1)] _ZWriteMode ("ZWriteMode", Float) = 1
}
SubShader {
 Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
 Pass {
  Name "ForwardEmission"
  Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "CustomForwardOpaque" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
  ZWrite Off
  Cull Off
  GpuProgramID 33533
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_BASECOLORTEX" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_GPUCROWD" "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_BASECOLORTEX" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_GPUCROWD" "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_BASECOLORTEX" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_GPUCROWD" "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_BASECOLORTEX" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
Local Keywords { "_USE_GPUCROWD" "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  ColorMask 0 0
  GpuProgramID 69639
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_USE_QTANGET" }
"// shader disassembly not supported on DXBC"
}
}
}
}
}