//======================================
// Sprite effect for MikuMikuMoving
//   Generated by MMMSpriteMaker
//   Written by ruche
//======================================

//--------------------------------------
// Config values
//--------------------------------------

// 0:Sprite
// 1:Sprite (dot by dot)
// 2:Billboard
// 3:Polygon
#define SPRMAKE_CONFIG_RENDERTYPE [[ConfigRenderType]]

// 0:No
// 1:Yes
#define SPRMAKE_CONFIG_RENDERBACK [[ConfigRenderingBack]]

// 0:Disabled
// 1:Enabled
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_LIGHT [[ConfigLightSetting]]

// Size per pixel (default = 0.1f)
#define SPRMAKE_CONFIG_PIXELRATIO [[ConfigPixelRatio]]

// Sprite view width (default = 45.0f)
#define SPRMAKE_CONFIG_SPRITE_VIEWWIDTH [[ConfigSpriteViewportWidth]]

// Sprite z-order range (default = 100.0f)
#define SPRMAKE_CONFIG_SPRITE_ZRANGE [[ConfigSpriteZRange]]

// 0:LeftTop
// 1:MiddleTop
// 2:RightTop
// 3:LeftMiddle
// 4:Center
// 5:RightMiddle
// 6:LeftBottom
// 7:MiddleBottom
// 8:RightBottom
#define SPRMAKE_CONFIG_BASEPOINT [[ConfigBasePoint]]

// 0:No
// 1:Yes
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_FLIP_H [[ConfigHorizontalFlipSetting]]

// 0:No
// 1:Yes
// 2:Selectable (MMM only)
#define SPRMAKE_CONFIG_FLIP_V [[ConfigVerticalFlipSetting]]

//--------------------------------------

#if SPRMAKE_CONFIG_RENDERTYPE > 2
    #define SPRMAKE_RENDER_POLYGON
#elif SPRMAKE_CONFIG_RENDERTYPE > 1
    #define SPRMAKE_RENDER_BILLBOARD
#elif SPRMAKE_CONFIG_RENDERTYPE > 0
    #define SPRMAKE_RENDER_SPRITE
    #define SPRMAKE_RENDER_DOTBYDOT
#else
    #define SPRMAKE_RENDER_SPRITE
#endif

#ifdef SPRMAKE_RENDER_SPRITE
    #undef SPRMAKE_CONFIG_LIGHT
    #define SPRMAKE_CONFIG_LIGHT 0
#endif

//--------------------------------------
// Texture atlas parameters
//--------------------------------------

#define SPRMAKE_ATLAS_COUNT [[AtlasFrameCount]]
#define SPRMAKE_ATLAS_TEXWIDTH [[AtlasImageWidth]]
#define SPRMAKE_ATLAS_TEXHEIGHT [[AtlasImageHeight]]

#ifdef SPRMAKE_RENDER_DOTBYDOT
static float2 SprMake_AtlasSizeMul = { 0.1f, 0.1f };
#else
static float2 SprMake_AtlasSizeMul =
    {
        0.1f * SPRMAKE_CONFIG_PIXELRATIO,
        0.1f * SPRMAKE_CONFIG_PIXELRATIO,
    };
#endif

static float2 SprMake_AtlasSizes[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasSizes]]
    };

static float2 SprMake_AtlasLeftBottomPosMul =
    {
        -0.5f * fmod(SPRMAKE_CONFIG_BASEPOINT, 3),
        0.5f * (floor(SPRMAKE_CONFIG_BASEPOINT / 3) - 2),
    };

static float2 SprMake_AtlasUVMul =
    { 1.0f / SPRMAKE_ATLAS_TEXWIDTH, 1.0f / SPRMAKE_ATLAS_TEXHEIGHT };

static float2 SprMake_AtlasUVLeftTops[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVLeftTops]]
    };

static float2 SprMake_AtlasUVRightTops[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVRightTops]]
    };

static float2 SprMake_AtlasUVRightBottoms[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVRightBottoms]]
    };

static float2 SprMake_AtlasUVLeftBottoms[SPRMAKE_ATLAS_COUNT] =
    {
[[AtlasUVLeftBottoms]]
    };

//--------------------------------------
// Global parameters
//--------------------------------------

float4x4 WorldViewProjMatrix : WORLDVIEWPROJECTION;
float4x4 WorldViewMatrix : WORLDVIEW;
float4x4 WorldMatrix : WORLD;

#ifdef SPRMAKE_RENDER_BILLBOARD
// Matrices for billboard
float4x4 WorldViewMatrixInverse : WORLDVIEWINVERSE;
static float3x3 BillboardMatrix =
    {
        normalize(WorldViewMatrixInverse[0].xyz),
        normalize(WorldViewMatrixInverse[1].xyz),
        normalize(WorldViewMatrixInverse[2].xyz),
    };
#endif // SPRMAKE_RENDER_BILLBOARD

#ifdef SPRMAKE_RENDER_SPRITE

float2 ViewportSize : VIEWPORTPIXELSIZE;
#ifdef SPRMAKE_RENDER_DOTBYDOT
static float4x4 ProjMatrix =
    {
        2.0f / ViewportSize.x, 0, 0, 0,
        0, 2.0f / ViewportSize.y, 0, 0,
        0,                     0, 1, 0,
        0,                     0, 0, 1,
    };
#else // SPRMAKE_RENDER_DOTBYDOT
static float ProjMatrix11 = 2.0f / SPRMAKE_CONFIG_SPRITE_VIEWWIDTH;
static float ViewportRatio = ViewportSize.x / ViewportSize.y;
static float4x4 ProjMatrix =
    {
        ProjMatrix11,                 0, 0, 0,
        0, ProjMatrix11 * ViewportRatio, 0, 0,
        0,                            0, 1, 0,
        0,                            0, 0, 1,
    };
#endif // SPRMAKE_RENDER_DOTBYDOT

#else // SPRMAKE_RENDER_SPRITE

float4x4 ProjMatrix : PROJECTION;

#endif // SPRMAKE_RENDER_SPRITE

float3 CameraPosition : POSITION < string Object = "Camera"; >;

// Material
float4 MaterialDiffuse : DIFFUSE < string Object = "Geometry"; >;
float3 MaterialAmbient : AMBIENT < string Object = "Geometry"; >;
float3 MaterialEmmisive : EMISSIVE < string Object = "Geometry"; >;
float3 MaterialSpecular : SPECULAR < string Object = "Geometry"; >;
float SpecularPower : SPECULARPOWER < string Object = "Geometry"; >;
float4 MaterialToon : TOONCOLOR;

#if SPRMAKE_CONFIG_LIGHT != 0
#ifdef MIKUMIKUMOVING
// for MikuMikuMoving

// Light
bool LightEnables[MMM_LightCount] : LIGHTENABLES;
float3 LightDirections[MMM_LightCount] : LIGHTDIRECTIONS;
float3 LightDiffuses[MMM_LightCount] : LIGHTDIFFUSECOLORS;
float3 LightAmbients[MMM_LightCount] : LIGHTAMBIENTCOLORS;
float3 LightSpeculars[MMM_LightCount] : LIGHTSPECULARCOLORS;

// Light for SelfShadow
float4x4 LightWVPMatrices[MMM_LightCount] : LIGHTWVPMATRICES;
float3 LightPositions[MMM_LightCount] : LIGHTPOSITIONS;
float LightZFars[MMM_LightCount] : LIGHTZFARS;

// Material & Light colors
static float4 DiffuseColors[3] =
    {
        MaterialDiffuse * float4(LightDiffuses[0], 1.0f),
        MaterialDiffuse * float4(LightDiffuses[1], 1.0f),
        MaterialDiffuse * float4(LightDiffuses[2], 1.0f),
    };
static float3 AmbientColors[3] =
    {
        saturate(MaterialAmbient * LightAmbients[0]) + MaterialEmmisive,
        saturate(MaterialAmbient * LightAmbients[1]) + MaterialEmmisive,
        saturate(MaterialAmbient * LightAmbients[2]) + MaterialEmmisive,
    };
static float3 SpecularColors[3] =
    {
        MaterialSpecular * LightSpeculars[0],
        MaterialSpecular * LightSpeculars[1],
        MaterialSpecular * LightSpeculars[2],
    };

#else // MIKUMIKUMOVING
// for MikuMikuEffect

// Light
float3 LightDirection : DIRECTION < string Object = "Light"; >;
float3 LightDiffuse : DIFFUSE < string Object = "Light"; >;
float3 LightAmbient : AMBIENT < string Object = "Light"; >;
float3 LightSpecular : SPECULAR < string Object = "Light"; >;

// Light for SelfShadow
float4x4 LightWorldViewProjMatrix : WORLDVIEWPROJECTION < string Object = "Light"; >;

// Material & Light colors
static float4 DiffuseColor = MaterialDiffuse * float4(LightDiffuse, 1.0f);
static float3 AmbientColor = saturate(MaterialAmbient * LightAmbient + MaterialEmmisive);
static float3 SpecularColor = MaterialSpecular * LightSpecular;

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0

// Texture
texture ObjectTexture : MATERIALTEXTURE;
sampler ObjTexSampler =
    sampler_state
    {
        texture = <ObjectTexture>;
        MINFILTER = LINEAR;
        MAGFILTER = LINEAR;
#if !defined(MIKUMIKUMOVING) && !defined(MME_MIPMAP)
        MIPFILTER = LINEAR;
#endif
        AddressU = BORDER;
        AddressV = BORDER;
        BorderColor = float4(0, 0, 0, 0);
    };

// Accessory
#ifdef SPRMAKE_RENDER_SPRITE
float3 AccPos : CONTROLOBJECT < string name = "(self)"; >;
#endif // SPRMAKE_RENDER_SPRITE
float AccTrans : CONTROLOBJECT < string name = "(self)"; string item = "Tr"; >;

//--------------------------------------
// Controls
//--------------------------------------
#ifdef MIKUMIKUMOVING

#if SPRMAKE_ATLAS_COUNT >= 2
int SprMake_AtlasIndex <
    string UIName = "Index";
    string UIWidget = "Numeric";
    int UIMin = 0;
    int UIMax = SPRMAKE_ATLAS_COUNT - 1; > = 0;
#else
static int SprMake_AtlasIndex = 0;
#endif

#if SPRMAKE_CONFIG_FLIP_H > 1
bool SprMake_FlipHorz < string UIName = "Flip_H"; > = false;
#endif

#if SPRMAKE_CONFIG_FLIP_V > 1
bool SprMake_FlipVert < string UIName = "Flip_V"; > = false;
#endif

#if SPRMAKE_CONFIG_LIGHT > 1
bool SprMake_LightEnabled < string UIName = "Light"; > = true;
#endif

#endif // MIKUMIKUMOVING
//--------------------------------------
// Structures
//--------------------------------------

// Texture atlas info
struct SPRMAKE_ATLAS_INFO
{
    float2 LeftBottomPos;
    float2 Size;
    float2 UVLeftTop;
    float2 UVRightTop;
    float2 UVRightBottom;
    float2 UVLeftBottom;
};

// Vertex shader output
struct VS_OUTPUT
{
    float4 Pos : POSITION;
    float2 Tex : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float4 Color : COLOR0;
#if SPRMAKE_CONFIG_LIGHT != 0
    float3 Eye : TEXCOORD2;
#ifdef MIKUMIKUMOVING
    float4 SS_UV1 : TEXCOORD3;
    float4 SS_UV2 : TEXCOORD4;
    float4 SS_UV3 : TEXCOORD5;
#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0
};

//--------------------------------------
// Functions
//--------------------------------------

// Get texture atlas info
SPRMAKE_ATLAS_INFO SprMake_GetAtlasInfo()
{
    SPRMAKE_ATLAS_INFO Out = (SPRMAKE_ATLAS_INFO)0;

#ifndef MIKUMIKUMOVING
    static int SprMake_AtlasIndex = 0;
#endif

    // select atlas
[[SelectAtlasCode]]

    Out.LeftBottomPos = Out.Size * SprMake_AtlasLeftBottomPosMul;

    // flip horizontal
#if SPRMAKE_CONFIG_FLIP_H != 0
#if defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_H > 1
    if (SprMake_FlipHorz)
    {
#endif // defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_H > 1
        float2 temp = Out.UVLeftTop;
        Out.UVLeftTop = Out.UVRightTop;
        Out.UVRightTop = temp;
        temp = Out.UVLeftBottom;
        Out.UVLeftBottom = Out.UVRightBottom;
        Out.UVRightBottom = temp;
#if defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_H > 1
    }
#endif // defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_H > 1
#endif // SPRMAKE_CONFIG_FLIP_H != 0

    // flip vertical
#if SPRMAKE_CONFIG_FLIP_V != 0
#if defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_V > 1
    if (SprMake_FlipVert)
    {
#endif // defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_V > 1
        float2 temp = Out.UVLeftTop;
        Out.UVLeftTop = Out.UVLeftBottom;
        Out.UVLeftBottom = temp;
        temp = Out.UVRightTop;
        Out.UVRightTop = Out.UVRightBottom;
        Out.UVRightBottom = temp;
#if defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_V > 1
    }
#endif // defined(MIKUMIKUMOVING) && SPRMAKE_CONFIG_FLIP_V > 1
#endif // SPRMAKE_CONFIG_FLIP_V != 0

    return Out;
}

// Calculate vertex position
float4 SprMake_CalcVertexPosition(float4 Pos, float3 Eye)
{
    float4 Out = Pos;

#ifdef SPRMAKE_RENDER_SPRITE

    Out = mul(Out, WorldMatrix);
    Out = mul(Out, ProjMatrix);
    Out.z = AccPos.z / SPRMAKE_CONFIG_SPRITE_ZRANGE;

#else // SPRMAKE_RENDER_SPRITE

#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving
    if (MMM_IsDinamicProjection)
    {
        float4x4 wvpmat = mul(WorldViewMatrix, MMM_DynamicFov(ProjMatrix, length(Eye)));
        Out = mul(Out, wvpmat);
    }
    else
    {
        Out = mul(Out, WorldViewProjMatrix);
    }
#else // MIKUMIKUMOVING
    // for MikuMikuEffect
    Out = mul(Out, WorldViewProjMatrix);
#endif // MIKUMIKUMOVING

#endif // SPRMAKE_RENDER_SPRITE

    return Out;
}

// Vertex shader
VS_OUTPUT SprMake_VS(float4 Pos : POSITION, float3 Normal : NORMAL, float2 Tex : TEXCOORD0, uniform bool useSelfShadow)
{
    VS_OUTPUT Out = (VS_OUTPUT)0;

    static SPRMAKE_ATLAS_INFO atlasInfo = SprMake_GetAtlasInfo();
    Pos.xy = atlasInfo.LeftBottomPos + (Pos.xy * atlasInfo.Size);
    Tex =
        lerp(
            lerp(atlasInfo.UVLeftTop, atlasInfo.UVRightTop, Tex.x),
            lerp(atlasInfo.UVLeftBottom, atlasInfo.UVRightBottom, Tex.x),
            Tex.y);

#ifdef SPRMAKE_RENDER_BILLBOARD
    Pos.xyz = mul(Pos.xyz, BillboardMatrix);
#endif // SPRMAKE_RENDER_BILLBOARD

    float3 eye = CameraPosition - mul(Pos.xyz, WorldMatrix);

    Out.Pos = SprMake_CalcVertexPosition(Pos, eye);
    Out.Normal = normalize(mul(Normal, (float3x3)WorldMatrix));
    Out.Tex = Tex;

#if SPRMAKE_CONFIG_LIGHT == 0
    Out.Color.rgb = float3(1, 1, 1);
#else // SPRMAKE_CONFIG_LIGHT == 0
    Out.Eye = eye;

#ifdef MIKUMIKUMOVING
    // for MikuMikuMoving

#if SPRMAKE_CONFIG_LIGHT > 1
    if (SprMake_LightEnabled)
    {
#endif // SPRMAKE_CONFIG_LIGHT > 1
        float3 color = float3(0, 0, 0);
        float3 ambient = float3(0, 0, 0);
        float count = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (LightEnables[i])
            {
                color +=
                    (float3(1, 1, 1) - color) *
                    (max(0, DiffuseColors[i] * dot(Out.Normal, -LightDirections[i])));
                ambient += AmbientColors[i];
                count = count + 1.0;
            }
        }
        Out.Color.rgb = saturate(ambient / count + color);

        if (useSelfShadow)
        {
            float4 dpos = mul(Pos, WorldMatrix);
            Out.SS_UV1 = mul(dpos, LightWVPMatrices[0]);
            Out.SS_UV2 = mul(dpos, LightWVPMatrices[1]);
            Out.SS_UV3 = mul(dpos, LightWVPMatrices[2]);
            Out.SS_UV1.y = -Out.SS_UV1.y;
            Out.SS_UV2.y = -Out.SS_UV2.y;
            Out.SS_UV3.y = -Out.SS_UV3.y;
            Out.SS_UV1.z = (length(LightPositions[0] - dpos.xyz) / LightZFars[0]);
            Out.SS_UV2.z = (length(LightPositions[1] - dpos.xyz) / LightZFars[1]);
            Out.SS_UV3.z = (length(LightPositions[2] - dpos.xyz) / LightZFars[2]);
        }
#if SPRMAKE_CONFIG_LIGHT > 1
    }
    else
    {
        Out.Color.rgb = float3(1, 1, 1);
    }
#endif // SPRMAKE_CONFIG_LIGHT > 1

#else // MIKUMIKUMOVING
    // for MikuMikuEffect

    Out.Color.rgb = AmbientColor;
    Out.Color.rgb += max(0, dot(Out.Normal, -LightDirection)) * DiffuseColor.rgb;
    Out.Color.rgb = saturate(Out.Color.rgb);

#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT == 0

    Out.Color.a = MaterialDiffuse.a;

    return Out;
}

// Pixel shader
float4 SprMake_PS(VS_OUTPUT IN, uniform bool useSelfShadow) : COLOR0
{
    float4 Color = IN.Color * tex2D(ObjTexSampler, IN.Tex);

#if SPRMAKE_CONFIG_LIGHT != 0
#ifdef MIKUMIKUMOVING
#if SPRMAKE_CONFIG_LIGHT > 1
    if (SprMake_LightEnabled)
    {
#endif // SPRMAKE_CONFIG_LIGHT > 1
        if (useSelfShadow)
        {
            Color.rgb *= MMM_GetSelfShadowToonColor(MaterialToon, IN.Normal, IN.SS_UV1, IN.SS_UV2, IN.SS_UV3, false, false);
        }

        float3 specular = 0;
        float3 halfVector;
        for (int i = 0; i < 3; ++i)
        {
            if (LightEnables[i])
            {
                halfVector = normalize(normalize(IN.Eye) + -LightDirections[i]);
                specular += pow(max(0, dot(halfVector, normalize(IN.Normal))), SpecularPower) * SpecularColors[i];
            }
        }
        Color.rgb += specular;
#if SPRMAKE_CONFIG_LIGHT > 1
    }
#endif // SPRMAKE_CONFIG_LIGHT > 1
#endif // MIKUMIKUMOVING
#endif // SPRMAKE_CONFIG_LIGHT != 0

    Color.a *= AccTrans;

    return Color;
}

//--------------------------------------
// Techniques
//--------------------------------------

#ifdef MIKUMIKUMOVING
technique MainTec < string MMDPass = "object"; bool UseTexture = true; bool UseToon = false; bool UseSelfShadow = false; >
#else
technique MainTec < string MMDPass = "object"; bool UseTexture = true; bool UseToon = false; >
#endif
{
    pass DrawObject
    {
#if SPRMAKE_CONFIG_RENDERBACK > 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK > 0

        VertexShader = compile vs_2_0 SprMake_VS(false);
        PixelShader  = compile ps_2_0 SprMake_PS(false);
    }
}

#ifdef MIKUMIKUMOVING
technique MainTecSS < string MMDPass = "object"; bool UseTexture = true; bool UseToon = false; bool UseSelfShadow = true; >
#else
technique MainTecSS < string MMDPass = "object_ss"; bool UseTexture = true; bool UseToon = false; >
#endif
{
    pass DrawObject
    {
#if SPRMAKE_CONFIG_RENDERBACK > 0
        CullMode = NONE;
#endif // SPRMAKE_CONFIG_RENDERBACK > 0

        VertexShader = compile vs_2_0 SprMake_VS(true);
        PixelShader  = compile ps_2_0 SprMake_PS(true);
    }
}

technique EdgeTec < string MMDPass = "edge"; bool UseToon = false; > { }
technique ShadowTec < string MMDPass = "shadow"; bool UseToon = false; > { }
technique ZplotTec < string MMDPass = "zplot"; bool UseToon = false; > { }
