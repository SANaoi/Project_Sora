//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "miHoYo/CRP_Character/CharacterTransparent" {
Properties {
[MHYHelpBox(Info, Vertex Color B (outline width))] [MHYHeaderBox(OPTIONS)] [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Float) = 2
[Toggle(_IS_DUALFACE)] _EnableDualFace ("Enable Dual Face Rendering", Float) = 0
_EnableAlphaCutoff ("Enable Alpha Cutoff", Float) = 0
_AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
_PolygonOffsetFactor ("Polygon Offset Factor", Float) = 0
_PolygonOffsetUnits ("Polygon Offset Units", Float) = 0
_OutlinePolygonOffsetFactor ("Outline Polygon Offset Factor", Float) = 0
_OutlinePolygonOffsetUnits ("Outline Polygon Offset Units", Float) = 0
[MHYHeaderBox(MAPS)] [MHYHeader(Main Maps)] [MHYSingleLineTextureNoScaleOffset(_Color)] _MainTex ("Albedo |RGB(base color) A (alpha)", 2D) = "white" { }
_VertexShadowColor ("Vertex Shadow Color", Color) = (1,1,1,1)
_Color ("Color", Color) = (1,1,1,1)
_BackColor ("BackColor", Color) = (1,1,1,1)
_EnvColor ("Env Color", Color) = (1,1,1,1)
_AddColor ("Add Color", Color) = (0,0,0,0)
[MHYSingleLineTextureNoScaleOffset] _LightMap ("Light Map |R (sepcular intensity) G (diffuse threshold) B (specular threshold) A (material id)", 2D) = "grey" { }
[MHYTextureScaleOffset] _MainMaps_ST ("Main Maps ST", Vector) = (1,1,0,0)
[MHYHeaderBox(Emission)] _EmissionThreshold ("Emission Threshold", Range(0, 1)) = 1
_EmissionIntensity ("Emission Intensity", Float) = 0
[MHYHeaderBox(DIFFUSE)] [MHYColorGradient] _DiffuseCoolRampMultiTex ("Cool Shadow Multiple Ramp", 2D) = "white" { }
[MHYColorGradient] _DiffuseRampMultiTex ("Shadow Multiple Ramp", 2D) = "white" { }
_ShadowRamp ("Shadow Ramp", Range(0.01, 1)) = 1
[MHYHeaderBox(SPECULAR)] [MHYMaterialIDFold] _SpecularColor ("Specular Color", Range(0, 1)) = 0
[MHYHeaderBox(SPECULAR)] [MHYMaterialIDFold] _SpecularColor ("Specular Color", Range(0, 1)) = 0
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor0 ("Specular Color (ID = 0)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess0 ("Specular Shininess (ID = 0)", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness0 ("Specular Roughness (ID = 0)", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity0 ("Specular Intensity (ID = 0)", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor1 ("Specular Color (ID = 31)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess1 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness1 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity1 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor2 ("Specular Color (ID = 63)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess2 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness2 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity2 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor3 ("Specular Color (ID = 95)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess3 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness3 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity3 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor4 ("Specular Color (ID = 127)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess4 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness4 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity4 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor5 ("Specular Color (ID = 159)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess5 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness5 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity5 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor6 ("Specular Color (ID = 192)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess6 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness6 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity6 ("Specular Intensity", Range(0, 50)) = 1
[MHYMaterialIDProperty(_SpecularColor)] _SpecularColor7 ("Specular Color (ID = 223)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_SpecularColor)] _SpecularShininess7 ("Specular Shininess", Range(0.1, 500)) = 10
[MHYMaterialIDProperty(_SpecularColor)] _SpecularRoughness7 ("Specular Roughness", Range(0, 1)) = 0.02
[MHYMaterialIDProperty(_SpecularColor)] _SpecularIntensity7 ("Specular Intensity", Range(0, 50)) = 1
_SpecularShininess ("Specular Shininess", Range(0.1, 500)) = 10
_SpecularRoughness ("Specular Roughness", Range(0, 1)) = 0
_SpecularIntensity ("Specular Intensity", Range(0, 50)) = 1
[MHYHeaderBox(OUTLINE)] [MHYMaterialIDFold] _Outline ("Outline", Range(0, 1)) = 0
[MHYMaterialIDProperty(_Outline)] _OutlineColor0 ("Outline Color 0 (ID = 0)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor1 ("Outline Color 1 (ID = 31)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor2 ("Outline Color 2 (ID = 63)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor3 ("Outline Color 3 (ID = 95)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor4 ("Outline Color 4 (ID = 127)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor5 ("Outline Color 5 (ID = 159)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor6 ("Outline Color 6 (ID = 192)", Color) = (0,0,0,1)
[MHYMaterialIDProperty(_Outline)] _OutlineColor7 ("Outline Color 7 (ID = 223)", Color) = (0,0,0,1)
_OutlineWidth ("Outline Width", Range(0, 1)) = 0.1
[KeywordEnum(Normal, Tangent, UV2)] _OutlineNormalFrom ("Outline Normal From", Float) = 0
_OutlineExtdMode ("Outline Extend Max Distance", Float) = 0
_OutlineOffset ("Outline Offset", Range(-1, 1)) = 0
[MHYHeaderBox(RIMLIGHT)] _RimLightMode ("0:don't use lightmap.r, 1:use", Range(0, 1)) = 0
[MHYMaterialIDFold] _RimLight ("Rim Light", Range(0, 1)) = 0
[MHYMaterialIDProperty(_RimLight)] _RimWidth0 ("RimWidth 0 (ID = 0)", Float) = 1
[MHYMaterialIDProperty(_RimLight)] _RimColor0 ("RimColor 0 (ID = 0)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor1 ("RimColor 1 (ID = 31)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor2 ("RimColor 2 (ID = 63)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor3 ("RimColor 3 (ID = 95)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor4 ("RimColor 4 (ID = 127)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor5 ("RimColor 5 (ID = 159)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor6 ("RimColor 6 (ID = 192)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimColor7 ("RimColor 7 (ID = 223)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness0 ("RimWidth 0 (ID = 0)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType0 ("RimWidth 0 (ID = 0)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark0 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness1 ("RimWidth 1 (ID = 1)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType1 ("RimWidth 1 (ID = 1)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark1 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness2 ("RimWidth 2 (ID = 2)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType2 ("RimWidth 2 (ID = 2)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark2 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness3 ("RimWidth 3 (ID = 3)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType3 ("RimWidth 3 (ID = 3)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark3 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness4 ("RimWidth 4 (ID = 4)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType4 ("RimWidth 4 (ID = 4)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark4 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness5 ("RimWidth 5 (ID = 5)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType5 ("RimWidth 5 (ID = 5)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark5 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness6 ("RimWidth 6 (ID = 6)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType6 ("RimWidth 6 (ID = 6)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark6 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimEdgeSoftness7 ("RimWidth 7 (ID = 7)", Range(0.01, 0.9)) = 0.1
[MHYMaterialIDProperty(_RimLight)] _RimType7 ("RimWidth 7 (ID = 7)", Range(0, 1)) = 1
[MHYMaterialIDProperty(_RimLight)] _RimDark7 ("RimWidth 0 (ID = 0)", Range(0, 8)) = 1
_RimFeatherWidth ("Rim Feather Width", Float) = 0.01
_RimWidth ("RimWidth", Float) = 1
_RimOffset ("Rim Offset", Vector) = (0,0,0,0)
_FresnelColor ("FresnelColor", Color) = (0,0,0,0)
_FresnelBSI ("Fresnel BSI", Vector) = (1,1,1,0)
_FresnelColorStrength ("FresnelColorStrength", Float) = 1
[Toggle(_RIM_BACKLIGHT_ON)] _EnableBackRimLight ("Enable Back Rim Light", Float) = 1
_RimShadowCt ("Rim Shadow Ct", Float) = 1
_RimShadowIntensity ("Rim Shadow Intensity", Float) = 1
[MHYHeaderBox(RIMSHADOW)] [MHYMaterialIDFold] _RimShadow ("Rim Shadow", Range(0, 1)) = 0
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor0 ("Rim Shadow Color 0 (ID = 0)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth0 ("Rim Shadow Width 0 (ID = 0)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather0 ("Rim Shadow Feather 0 (ID = 0)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor1 ("Rim Shadow Color 1 (ID = 1)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth1 ("Rim Shadow Width 1 (ID = 1)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather1 ("Rim Shadow Feather 1 (ID = 1)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor2 ("Rim Shadow Color 2 (ID = 2)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth2 ("Rim Shadow Width 2 (ID = 2)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather2 ("Rim Shadow Feather 2 (ID = 2)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor3 ("Rim Shadow Color 3 (ID = 3)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth3 ("Rim Shadow Width 3 (ID = 3)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather3 ("Rim Shadow Feather 3 (ID = 3)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor4 ("Rim Shadow Color 4 (ID = 4)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth4 ("Rim Shadow Width 4 (ID = 4)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather4 ("Rim Shadow Feather 4 (ID = 4)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor5 ("Rim Shadow Color 5 (ID = 5)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth5 ("Rim Shadow Width 5 (ID = 5)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather5 ("Rim Shadow Feather 5 (ID = 5)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor6 ("Rim Shadow Color 6 (ID = 6)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth6 ("Rim Shadow Width 6 (ID = 6)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather6 ("Rim Shadow Feather 6 (ID = 6)", Range(0.01, 0.99)) = 0.01
[MHYMaterialIDProperty(_RimShadow)] _RimShadowColor7 ("Rim Shadow Color 7 (ID = 7)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_RimShadow)] _RimShadowWidth7 ("Rim Shadow Width 7 (ID = 7)", Float) = 1
[MHYMaterialIDProperty(_RimShadow)] _RimShadowFeather7 ("Rim Shadow Feather 7 (ID = 7)", Range(0.01, 0.99)) = 0.01
_RimShadowOffset ("Rim Shadow Offset", Vector) = (0,0,0,0)
[MHYHeaderBox(_USE_FUR)] [MHYSingleLineTextureKeywordDrawer(_FUR_ON)] _FurTex ("FurTex", 2D) = "white" { }
[MHYKeywordFilter(_FUR_ON On)] _FurTexSmooth ("FurTexSmooth", Float) = 0.95
[MHYKeywordFilter(_FUR_ON On)] _FurColor ("FurColor", Color) = (0,0,0,0)
[MHYKeywordFilter(_FUR_ON On)] _FurTile ("Fur Tile", Float) = 10
[MHYHeaderBox(STOCKINGS)] [Toggle(_WITHSTOCKINGS)] _EnableStocking ("With Stockings", Float) = 0
[MHYKeywordFilter(_WITHSTOCKINGS On)] _StockRangeTex ("Stocking Range Texutre", 2D) = "black" { }
[MHYKeywordFilter(_WITHSTOCKINGS On)] _Stockcolor ("Stockings Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_WITHSTOCKINGS On)] _StockDarkcolor ("Stockings Darkend Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_WITHSTOCKINGS On)] _StockDarkWidth ("Stockings Rim Width", Range(0, 0.96)) = 0.5
[MHYKeywordFilter(_WITHSTOCKINGS On)] _Stockpower ("Stockings Power", Range(0.04, 1)) = 1
[MHYKeywordFilter(_WITHSTOCKINGS On)] _Stockpower1 ("Stockings Lighted Width", Range(1, 32)) = 1
[MHYKeywordFilter(_WITHSTOCKINGS On)] _StockSP ("Stockings Lighted Intensity", Range(0, 1)) = 0.25
[MHYKeywordFilter(_WITHSTOCKINGS On)] _StockRoughness ("Stockings Texture Intensity", Range(0, 1)) = 1
[MHYKeywordFilter(_WITHSTOCKINGS On)] _Stockthickness ("Stockings Thickness", Range(0, 1)) = 0
[MHYHeaderBox(STATTYSKY)] [Toggle(_STATTYSKY)] _StarrySky ("With StarrySky", Float) = 0
[MHYKeywordFilter(_STATTYSKY On)] _SkyTex ("StarrySky Base Texture", 2D) = "black" { }
[MHYKeywordFilter(_STATTYSKY On)] _SkyMask ("StarrySky Mask Texture", 2D) = "black" { }
[MHYKeywordFilter(_STATTYSKY On)] _SkyRange ("StarrySky Range", Range(-1, 1)) = 0
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarColor ("StarrySky Star Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarTex ("StarrySky Star Texture", 2D) = "black" { }
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarTexScale ("StarrySky Star Texture Scale", Float) = 1
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarSpeed ("StarrySky Star Speed(XY)", Vector) = (0,0,0,0)
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarDepthScale ("StarrySky Star DepthScale", Float) = 1
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarMaskTex ("StarrySky Star Mask Texture", 2D) = "whilte" { }
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarMaskTexScale ("StarrySky Star Mask Texture Scale", Float) = 1
[MHYKeywordFilter(_STATTYSKY On)] _SkyStarMaskTexSpeed ("StarrySky Star flicker frequency", Range(0, 20)) = 0
[MHYKeywordFilter(_STATTYSKY On)] _SkyFresnelColor ("StarrySky FresnelColor", Color) = (0,0,0,1)
[MHYKeywordFilter(_STATTYSKY On)] _SkyFresnelBaise ("StarrySky FresnelBaise", Float) = 0
[MHYKeywordFilter(_STATTYSKY On)] _SkyFresnelScale ("StarrySky FresnelScale", Float) = 0
[MHYKeywordFilter(_STATTYSKY On)] _SkyFresnelSmooth ("StarrySky FresnelSmooth", Range(0, 0.5)) = 0
[MHYHeaderBox(_FLAMECRYSTALEFFECT)] [Toggle(_FLAMECRYSTALEFFECT)] _FlameCrystal ("With FlameCrystal", Float) = 0
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _TangentDirTex ("Tangent Direction Texture", 2D) = "bump" { }
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameTex ("Fmale Texture", 2D) = "black" { }
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _CrystalTex ("Crystal Texture", 2D) = "black" { }
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameID ("Material ID for Flame", Float) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameColorOut ("OutSide Flame Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameColorIn ("Inside Flame Color", Color) = (1,1,1,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameHeight ("Flame Height", Range(0, 1)) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameWidth ("Flame Width", Range(0, 1)) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameSpeed ("Flame Waving Speed", Float) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameSwirilTexScale ("Flame Swiril Texture Scale", Float) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameSwirilSpeed ("Flame Swiril Speed", Float) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _FlameSwirilScale ("Flame Swiril Scale", Float) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _CrystalTransparency ("Crystal Transparency", Range(0, 1)) = 0.35
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _CrystalRange1 ("Effect Progress in", Range(0, 1)) = 0
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _CrystalRange2 ("Effect Progress out", Range(0, 1)) = 1
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor0 ("Effect Color 0 (ID = 0)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor1 ("Effect Color 1 (ID = 31)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor2 ("Effect Color 2 (ID = 63)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor3 ("Effect Color 3 (ID = 95)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor4 ("Effect Color 4 (ID = 127)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor5 ("Effect Color 5 (ID = 159)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor6 ("Effect Color 6 (ID = 192)", Color) = (0,0,0,1)
[MHYKeywordFilter(_FLAMECRYSTALEFFECT On)] _EffectColor7 ("Effect Color 7 (ID = 223)", Color) = (0,0,0,1)
[MHYHeaderBox(_SUNGLASSES)] [Toggle(_SUNGLASSES)] _Sunglasses ("Sunglasses", Float) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _SunGlassesTilingOffset ("SunGlassesTilingOffset", Vector) = (1,1,0,0)
[MHYKeywordFilter(_SUNGLASSES On)] _SunglassesSpecluarColor ("SunglassesSpecluarColor", Color) = (1,1,1,1)
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightWidthL ("HighlightWidthL", Range(0, 2)) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightWidthR ("HighlightWidthR", Range(0, 2)) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _TotalSizeL ("TotalSizeL", Range(0, 2)) = 0.5
[MHYKeywordFilter(_SUNGLASSES On)] _TotalSizeR ("TotalSizeR", Range(0, 2)) = 0.5
[MHYKeywordFilter(_SUNGLASSES On)] _BlendRadiusL ("BlendRadiusL", Range(-1.1, 1.1)) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _BlendRadiusR ("BlendRadiusR", Range(-1.1, 1.1)) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightAngleL ("HighlightL", Float) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightAngleR ("HighlightR", Float) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightOffsetL ("_HighlightOffsetL", Float) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _HighlightOffsetR ("_HighlightOffsetR", Float) = 0
[MHYKeywordFilter(_SUNGLASSES On)] _BendValue ("BendValue", Range(-1, 1)) = 1
[MHYHeaderBox(Dissolve)] _DissoveON ("Enable Dissolve", Float) = 0
_DissolveRate ("Dissolve Rate", Range(0, 1)) = 0
_DissolveMap ("Dissolve Map", 2D) = "white" { }
_DissolveST ("Dissolve ST", Vector) = (1,1,0,0)
_DistortionST ("Distortion ST", Vector) = (1,1,0,0)
_DissolveDistortionIntensity ("", Float) = 0.01
_DissolveOutlineSize1 ("", Float) = 0.05
_DissolveOutlineSize2 ("", Float) = 0
_DissolveOutlineOffset ("", Float) = 0
_DissolveOutlineColor1 ("", Color) = (1,1,1,1)
_DissolveOutlineColor2 ("", Color) = (0,0,0,0)
_DissoveDirecMask ("", Float) = 2
_DissolveOutlineSmoothStep ("", Vector) = (0,0,0,0)
_DissolveUV ("", Range(0, 1)) = 0
_DissolveUVSpeed ("", Vector) = (0,0,0,0)
_DissolveMask ("Dissolve Mask", 2D) = "white" { }
_DissolveComponent ("MaskChannel RGBA=0/1", Vector) = (1,0,0,0)
_DissolvePosMaskPos ("", Vector) = (1,0,0,1)
_DissolvePosMaskWorldON ("", Float) = 0
_DissolvePosMaskRootOffset ("", Vector) = (0,0,0,0)
_DissolvePosMaskFilpOn ("", Float) = 0
_DissolvePosMaskOn ("", Float) = 0
[MHYHeaderBox(ADDLIGHT)] _AddLightOffset ("Add Light Offset", Range(0, 1)) = 0.5
_AddLightStrengthen ("Add Light Strengthen", Range(0, 3)) = 0.3
_AddLightFeather ("Add Light Feather", Range(0, 0.1)) = 0.03
_Tst ("test", Range(0, 0.1)) = 0.03
[MHYHeaderBox(BLOOM INSTENSITY)] [MHYMaterialIDFold] _BloomIntensity ("Bloom Intensity", Range(0, 1)) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity0 ("Bloom Intensity 0 (ID = 0)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity1 ("Bloom Intensity 1 (ID = 31)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity2 ("Bloom Intensity 2 (ID = 63)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity3 ("Bloom Intensity 3 (ID = 95)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity4 ("Bloom Intensity 4 (ID = 127)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity5 ("Bloom Intensity 5 (ID = 159)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity6 ("Bloom Intensity 6 (ID = 192)", Float) = 0
[MHYMaterialIDProperty(_BloomIntensity)] _mBloomIntensity7 ("Bloom Intensity 7 (ID = 223)", Float) = 0
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor0 ("Bloom Color 0 (ID = 0)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor1 ("Bloom Color 1 (ID = 31)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor2 ("Bloom Color 2 (ID = 63)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor3 ("Bloom Color 3 (ID = 95)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor4 ("Bloom Color 4 (ID = 127)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor5 ("Bloom Color 5 (ID = 159)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor6 ("Bloom Color 6 (ID = 192)", Color) = (1,1,1,1)
[MHYMaterialIDProperty(_BloomColor)] _mBloomColor7 ("Bloom Color 7 (ID = 223)", Color) = (1,1,1,1)
_Opaqueness ("Opaqueness", Range(0, 1)) = 1
[MHYHeaderBox(DITHER)] _UsingDitherAlpha ("UsingDitherAlpha", Float) = 0
_DitherAlpha ("Dither Alpha Value", Range(0, 1)) = 1
[Toggle(_RECEIVE_SHADOWS)] _ReceiveShadows ("Receive Shadows", Float) = 1
_StencilRef ("Stencil Ref", Float) = 16
_StencilOP ("Stencil OP", Float) = 2
_StencilComp ("Stencil Comp", Float) = 8
_RenderingMode ("Rendering Mode", Float) = 0
_ZWrite ("ZWrite", Float) = 1
_SrcBlend ("Src Blend", Float) = 1
_DstBlend ("Src Blend", Float) = 0
_CullMode ("Src Blend", Float) = 0
[MHYHeaderBox(VAT)] [MHYSingleLineTextureNoScaleOffset] _VAT_PosTex ("Pos", 2D) = "white" { }
[MHYSingleLineTextureNoScaleOffset] _VAT_NormalTex ("Normal", 2D) = "white" { }
[MHYSingleLineTextureNoScaleOffset] _VAT_TangentTex ("Tangent", 2D) = "white" { }
_VAT_PosRangeMin ("Bbox Min", Vector) = (0,0,0,0)
_VAT_PosRangeMax ("Bbox Max", Vector) = (0,0,0,0)
_VAT_PrimiteCount ("Prim Count", Float) = 1
_VAT_AnimSpeed ("Animation Speed", Float) = 0
[MHYSingleLineTextureNoScaleOffset(_EmissionTintColor)] _EmissionTex ("Emission Tex", 2D) = "black" { }
_EmissionTintColor ("Emission TintColor", Color) = (1,1,1,1)
_Android_Debug_ToonSpecular ("", Float) = 1
_UseMaterialValuesLUT ("Use Mat Lut", Float) = 0
_MaterialValuesPackLUT ("Mat Pack LUT", 2D) = "white" { }
}
SubShader {
 Tags { "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
 Pass {
  Name "CustomForward"
  Tags { "LIGHTMODE" = "CustomRPTransparent" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  Blend Zero Zero, Zero Zero
  Cull Off
  Stencil {
   Comp Always
   Pass Replace
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 30641
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
Local Keywords { "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
Local Keywords { "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
Local Keywords { "_FRAGMENT_CLIP" "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_GBUFFER_EMISSION" }
Local Keywords { "_FRAGMENT_CLIP" "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
Local Keywords { "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
Local Keywords { "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
Local Keywords { "_FRAGMENT_CLIP" "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "_USE_LOCAL_LIGHT_SHADOW" }
Local Keywords { "_FRAGMENT_CLIP" "_SUNGLASSES" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Name "CustomReflectionTransparent"
  Tags { "LIGHTMODE" = "CustomReflection" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  ZWrite Off
  Cull Off
  GpuProgramID 97933
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" "_IS_DUALFACE" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  Name "TransparentOutline"
  Tags { "LIGHTMODE" = "CustomRPTransparent2" "QUEUE" = "Geometry+40" "RenderType" = "Opaque" }
  ZWrite Off
  Cull Front
  Stencil {
   Comp Disabled
   Pass Keep
   Fail Keep
   ZFail Keep
  }
  GpuProgramID 145781
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Local Keywords { "_FRAGMENT_CLIP" }
"// shader disassembly not supported on DXBC"
}
}
}
 UsePass "Hidden/miHoYo/Character/Shared/MOTIONVECTORDUALFACE"
 UsePass "Hidden/miHoYo/Character/Shared/MOTIONVECTOROUTLINE"
 UsePass "Hidden/miHoYo/Character/Shared/CUSTOMREFLECTIONOPAQUE"
 UsePass "Hidden/miHoYo/Common/Shared/SHADOWCASTER"
 UsePass "Hidden/miHoYo/Common/Shared/DEPTHONLYCULLOFF"
}
CustomEditor "RPG.Editor.CharacterBaseShaderGUI"
}