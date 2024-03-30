//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "miHoYo/CRP_Character/CharacterSimple" {
Properties {
[MHYHeaderBox(OUTLINE)] _OutlineWidth ("Outline Width", Range(0, 1)) = 0.1
_OutlineColor0 ("Outline Color", Color) = (0,0,0,1)
_OutlinePolygonOffsetFactor ("Outline Polygon Offset Factor", Float) = 0
_OutlinePolygonOffsetUnits ("Outline Polygon Offset Units", Float) = 0
[MHYHeaderBox(DIFFUSE)] [MHYSingleLineTexture(_Color)] _MainTex ("Albedo |RGB(base color) A (emission)", 2D) = "white" { }
[Toggle(_ATLAS_FACE)] _UsingAtlasFace ("UsingAtlasFace", Float) = 0
_AtlasDivision ("Atlas Division", Vector) = (3,3,0,0)
[MHYTextureScaleOffset] _MainMaps_ST ("Main Maps ST", Vector) = (1,1,0,0)
_Color ("Color", Color) = (1,1,1,1)
_ShadowThreshold ("Diffuse Threshold", Range(0.01, 1)) = 0.337
_ShadowFeather ("Diffuse Feather", Range(0.01, 1)) = 0.337
_BrightDiffuseColor ("Bright Color", Color) = (1,1,1,1)
_ShadowDiffuseColor ("Shadow Color", Color) = (0,0,0,1)
[MHYHeaderBox(SPECULAR)] _SpecularColor0 ("Specular Color", Color) = (1,1,1,1)
_SpecularShininess0 ("Specular Shininess", Range(0.1, 500)) = 10
_SpecularRoughness0 ("Specular Roughness", Range(0, 1)) = 1
_SpecularIntensity0 ("Specular Intensity", Range(0, 50)) = 1
_SpecularThreshold ("Specular Threshold", Range(0, 1)) = 1
[MHYHeaderBox(BLOOM)] _mBloomIntensity0 ("Bloom Intensity", Float) = 1
[MHYHeaderBox(Emission)] _EmissionThreshold ("Emission Threshold", Range(0, 1)) = 1
_EmissionIntensity ("Emission Intensity", Float) = 0
[MHYHeaderBox(ScreenEffect)] [Toggle(_SCREENEFFECT)] _UsingScreenEffect ("UsingAtlasFace", Float) = 0
_ScreenNoiseInst ("ScreeEffect Noise Speed", Range(0, 1)) = 0
_ScreenNoiseST ("ScreeEffect Noise Parameters", Vector) = (1,1,0,0)
_ScreenLineInst ("ScreeEffect Line Speed", Range(0, 1)) = 0
_ScreenLineST ("ScreeEffect Line Parameters", Vector) = (1,1,0,0)
_ScreenNoiseseed2 ("ScreeEffect Noise SeedY", Float) = 0.1
[MHYHeaderBox(DITHER)] [Toggle(_USINGDITHERALPAH)] _UsingDitherAlpha ("UsingDitherAlpha", Float) = 0
_DitherAlpha ("Dither Alpha Value", Range(0, 1)) = 1
_CharacterLocalMainLightPack1 ("_CharacterLocalMainLightPack1", Vector) = (10000,0,0,0)
}
SubShader {
 Tags { "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
 Pass {
  Name "LightingGBuffer"
  Tags { "LIGHTMODE" = "LightingGBuffer" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  Stencil {
   Comp Always
   Pass Replace
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 48194
Program "vp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Name "forward"
  Tags { "LIGHTMODE" = "ForwardEmission" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  ZWrite Off
  GpuProgramID 78138
Program "vp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" "_SCREENEFFECT" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_ATLAS_FACE" "_SCREENEFFECT" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Name "OpaqueOutline"
  Tags { "LIGHTMODE" = "RPGOutline" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  Cull Front
  Stencil {
   Comp Always
   Pass Replace
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 154641
Program "vp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_OUTLINENORMALFROM_TANGENT" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_MAIN_LIGHT_SHADOWS" }
Local Keywords { "_OUTLINENORMALFROM_TANGENT" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Name "ShadowCaster"
  Tags { "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  ColorMask 0 0
  Offset 3, 1
  GpuProgramID 208401
Program "vp" {
SubProgram "d3d11 " {
Local Keywords { "_VAT_OFF" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
Local Keywords { "_VAT_OFF" }
"// shader disassembly not supported on DXBC"
}
}
}
 UsePass "Hidden/miHoYo/Character/Shared/MOTIONVECTORDUALFACE"
 UsePass "Hidden/miHoYo/Character/Shared/MOTIONVECTOROUTLINE"
}
}