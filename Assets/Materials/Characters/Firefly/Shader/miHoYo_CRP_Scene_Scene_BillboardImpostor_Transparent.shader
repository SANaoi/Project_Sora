//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "miHoYo/CRP_Scene/Scene_BillboardImpostor_Transparent" {
Properties {
[Toggle] _QTangent_Off ("Qtangent Off?", Float) = 0
[MHYHeaderBox(Base)] [Enum(HemiOctahedron,0, Octahedron,1, Spherical,1)] _ImpostorType ("ImpostorType", Float) = 0
[MHYSingleLineTextureNoScaleOffset] _AlbedoAlphaMap ("Albedo Alpha Map", 2D) = "gray" { }
[Toggle(PACKALBEDO)] _PackAlbedo ("PackAlbedo", Float) = 1
_ImpostorAlbedoScale ("ImpostorAlbedoScale", Range(0, 5)) = 1
_ImpostorAlphaScale ("ImpostorAlphaScale", Range(0, 4)) = 1
_ImpostorFrames ("Impostor Frames", Float) = 8
_ImpostorSize ("Impostor Size", Float) = 1
_ImpostorOffset ("Impostor Offset", Vector) = (0,0,0,0)
_ImpostorBorderClamp ("Impostor Border Clamp", Float) = 2
[MHYHeaderBox(Render Setting)] [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend Mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend Mode", Float) = 10
[Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
[Enum(Off, 0, On, 1)] _ZWriteMode ("ZWriteMode", Float) = 0
_StencilMode ("StencilMode", Float) = 0
_StencilRef ("Stencil", Float) = 128
_StencilRefGBuffer ("Stencil", Float) = 224
[Enum(UnityEngine.Rendering.StencilOp)] _StencilOP ("Stencil OP", Float) = 2
_GBufferOffsetFactor ("Offset Factor", Float) = 0
_GBufferOffsetUnits ("Offset Units", Float) = 0
_BlendSrcModeForward ("Blend Src Forward", Float) = 1
_BlendDstModeForward ("Blend Dst Forward", Float) = 0
_ForwardOpaqueOffsetX ("Forward Opaque Offset X", Float) = -1
_ForwardOpaqueOffsetY ("Forward Opaque Offset X", Float) = -1
_DitherAlpha ("_DitherAlpha", Float) = 1
}
SubShader {
 Tags { "QUEUE" = "Transparent-100" "RenderType" = "Transparent" }
 Pass {
  Tags { "LIGHTMODE" = "CustomRPTransparent" "QUEUE" = "Transparent-100" "RenderType" = "Transparent" }
  Blend Zero Zero, Zero Zero
  ZWrite Off
  Cull Off
  Stencil {
   Comp Always
   Pass Replace
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 33998
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "INSTANCING_ON" }
"// shader disassembly not supported on DXBC"
}
}
}
}
}